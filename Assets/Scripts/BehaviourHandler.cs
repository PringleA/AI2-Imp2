using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourHandler : MonoBehaviour
{
    public EnemyMood mood;
    public EnemyState state;
    private EnemyClass enemyClass;
    public float stateSwitchDelay = 2.0f;
    public float currentDelay = 0;
	public bool findNewState = false;
	private float randStartDelay = 0;
    public ProbController prob;
	private NavMeshAgent agent;
	private GameObject player;

	//private float min

	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindGameObjectWithTag("PlayerCapsule");
		// varying initial decision for enemies
		float minStartDelay = 0.0f;
	    float maxStartDelay = 1.0f;
		randStartDelay = Random.Range(minStartDelay, maxStartDelay);
		currentDelay = randStartDelay;

		prob = gameObject.transform.parent.GetComponent<ProbController>();
		agent = gameObject.GetComponent<NavMeshAgent>();
        enemyClass = gameObject.GetComponent<EnemyClass>();

		if (prob == null)
            prob = new ProbController();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyClass.playerVisible)
        {
            // iterate delay for state switching
            if (currentDelay < stateSwitchDelay)
                currentDelay += Time.fixedDeltaTime;

            // allow state change if max delay is reached
            else if (currentDelay >= stateSwitchDelay)
            {
                findNewState = true;
                currentDelay = 0;
            }

            if (findNewState)
            {
                state = CalculateNextState(state, mood);
                findNewState = false;
            }
        } 
	}

	public EnemyState CalculateNextState(EnemyState state, EnemyMood mood)
	{
		EnemyState newState = FindRandomDecision(state, mood);
		return newState;
	}

	private EnemyState FindRandomDecision(EnemyState state, EnemyMood mood)
	{
		switch (state)
		{
			case EnemyState.HIDE:
				{
					float peek = prob.states[(int)mood].look - prob.uncertainty;
					if (Random.Range(0.0f, 1.0f) < peek)
						return EnemyState.LOOK;
					else
						return EnemyState.HIDE;
				}
			case EnemyState.MOVE:
				{
					// check to hide if player is not visible, else look around
					if (!enemyClass.playerVisible)
					{
						float hideCheck = prob.states[(int)mood].hide - prob.uncertainty;
						if (Random.Range(0.0f, 1.0f) < hideCheck)
							return EnemyState.HIDE;
						else
							return EnemyState.LOOK;
					}
					else
					{
						float shootCheck = prob.states[(int)mood].shoot - prob.uncertainty;
						if (Random.Range(0.0f, 1.0f) < shootCheck)
							return EnemyState.SHOOT;
						else
							return EnemyState.LOOK;
					}
				}

			case EnemyState.LOOK:
				{
					// check to shoot if player is visible
					if (enemyClass.playerVisible)
					{
						//find player distance
						float distApart = Vector3.Distance(transform.position, player.transform.position);
						PlayerDist pDist = prob.FindPlayerDist(distApart);

						// do not shoot if player is too far out of range
						if (pDist == PlayerDist.TOOFAR)
							return EnemyState.LOOK;

						// otherwise attempt to shoot
						else
						{
							float shootCheck = prob.states[(int)mood].shoot - prob.uncertainty;
							if (Random.Range(0.0f, 1.0f) < shootCheck)
								return EnemyState.SHOOT;
							else
								return EnemyState.LOOK;
						}
					}

					// if player is not visible
					else
					{
						float moveCheck = prob.states[(int)mood].move - prob.uncertainty;
						if (Random.Range(0.0f, 1.0f) < moveCheck)
							return EnemyState.MOVE;
						else
							return EnemyState.LOOK;
					}

				}
			default:
				return EnemyState.HIDE;
		}
				
		
	}
}
