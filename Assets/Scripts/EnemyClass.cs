using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyClass : MonoBehaviour
{
	// public
	public float maxHealth = 100;
	public float damage = 10;
	public float lookSpeed = 1.0f;
	public float maxLookTime = 30.0f;
	public float maxRange = 300.0f;
	public float fireSpdMult = 0.15f;
	public float healthBarTimer = 0.0f;
	public bool isMoving = false;
	public bool isHiding = false;
	public bool isRotating = false;
	public bool playerVisible = false;
	public Slider healthBar;
	public BehaviourHandler behaviour;
	// private
	private float health = 0;
	private float rotLength = 0;
	private float currentYrot = 0;
	private float nextShotTime = 0;
	private float fireRate = 10.0f;
	private float barVisibilityTime = 5.0f;
	private bool rotReversed = false;
	private bool hidden = false;
	private GameObject player;
	private GameObject playerCam;
	private GameObject[] cover;
	private NavMeshAgent agent;
	private RayController raycastSpot;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("PlayerCapsule");
		playerCam = GameObject.FindGameObjectWithTag("MainCamera");
		cover = GameObject.FindGameObjectsWithTag("HidePosition");
		behaviour = gameObject.GetComponent<BehaviourHandler>();
		health = maxHealth;
		agent = gameObject.GetComponent<NavMeshAgent>();
		raycastSpot = gameObject.GetComponentInChildren<RayController>();
	}

	// Update is called once per frame
	void Update()
    {
		// make health bar point at player cam at all times
		if (playerCam != null)
			healthBar.transform.LookAt(playerCam.transform.position);

		// set health bar to current hp
		healthBar.value = health / maxHealth;

		if (health <= 0)
		{
			Destroy(gameObject);
			Scores.AddKill(1);
		}

		//ShootRaycast();
		StateTransform();

		nextShotTime += Time.fixedDeltaTime;
		healthBarTimer += Time.fixedDeltaTime;

		// reset timer so healthbar stays visible if enemy sees player
		if (playerVisible)
			healthBarTimer = 0;

		// set inactive after alloted visibility time
		if (healthBarTimer >= barVisibilityTime)
			healthBar.gameObject.SetActive(false);

		// set active if before alloted visibility time
		else if (healthBarTimer < barVisibilityTime)
			healthBar.gameObject.SetActive(true);
	}

	private void StateTransform()
	{
		switch (behaviour.state)
		{
			case EnemyState.HIDE:
				{
					HideState();
					break;
				}
			case EnemyState.MOVE:
				{
					MoveState();
					break;
				}
			case EnemyState.LOOK:
				{
					LookState();
					break;
				}
			case EnemyState.SHOOT:
				{
					ShootState();
					break;
				}
			default:
				break;
		}
	}

	private void HideState()
	{
		isMoving = false;
		isRotating = false;
		//check next behaviour
		if (!hidden)
		{
			GameObject nearestCover = FindNearestCover();
			Vector3 newPos = new Vector3(nearestCover.transform.position.x,
			gameObject.transform.position.y,
			nearestCover.transform.position.z);
			if (!isHiding)
				MoveTowardsCover(newPos);

			if (Vector3.Distance(transform.position, newPos) < 0.001f)
				hidden = true;
		}
	}

	private void MoveState()
	{
		isHiding = false;
		isRotating = false;
		if (!isMoving)
			MoveToRandomSpot();
	}

	private void LookState()
	{
		isMoving = false;
		isHiding = false;
		IdleLook();
	}
	private void ShootState()
	{
		isMoving = false;
		isHiding = false;
		isRotating = false;
		// if alerted and not hiding
		if (playerVisible)
		{
			transform.LookAt(player.transform.position);
			TryDamage();
			behaviour.currentDelay = 0;
		}		
		//shoot at player
	}

	private void TryDamage()
	{
		//input distance between player and enemy
		float distApart = Vector3.Distance(transform.position, player.transform.position);
		// if shot is hit
		bool shotAttempt = behaviour.prob.AttemptShot(distApart, behaviour.mood);
		if (shotAttempt == true)
		{
			if (nextShotTime > fireRate)
			{
				PlayerController playerController = player.GetComponentInParent<PlayerController>();
				if (playerController != null)
				{
					Debug.Log("Shot Hit");
					playerController.TakeDamage(damage);
					nextShotTime = 0;
				}
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.transform.GetComponent<PlayerController>() is PlayerController playerCheck)
		{
			transform.LookAt(player.transform.position);
			ShootRaycast();
		}
	}

	public void ShootRaycast()
	{
		RaycastHit hit;

		//test looking at player
		//GameObject testLook = gameObject;
		//testLook.transform.LookAt(player.transform.position);



		// set shoot direction and origin
		Vector3 enemyPosition = raycastSpot.transform.position;
		Vector3 enemyForward = raycastSpot.transform.forward;

		if (Physics.Raycast(enemyPosition, enemyForward, out hit, maxRange))
		{
			Debug.DrawRay(enemyPosition, enemyForward, Color.green, 5000.0f);

			PlayerController playerTest = hit.transform.GetComponent<PlayerController>();

			if (playerTest != null)
			{
				transform.LookAt(player.transform.position);
				agent.ResetPath();
				playerVisible = true;
				behaviour.state = EnemyState.SHOOT;
				behaviour.findNewState = false;
				ShootState();
				behaviour.currentDelay = 0;
			}
			else
			{
				playerVisible = false;
			}
		}
	}

	public void TakeDamage(float amount)
    {
		float currentHP = health;
		health = currentHP - amount;
	}

	private void IdleLook()
	{
		// initial setup if just moved into idle movement
		if (!isRotating && !playerVisible)
		{
			agent.ResetPath();
			isRotating = true;
		}

		float rotAdd = lookSpeed * Time.fixedDeltaTime;
		rotLength += Time.fixedDeltaTime;

		// rotate left and right at set look speed
		if (isRotating)
		{
			if (!rotReversed)
			{
				currentYrot = rotAdd;
			}
			else if (rotReversed)
			{
				currentYrot = -rotAdd;
			}

			// flip rotation if max time reached
			if (rotLength >= maxLookTime)
			{
				rotReversed = !rotReversed;
				// reset timer
				rotLength = 0;
			}

			// set new rotation each frame
			gameObject.transform.Rotate(0, currentYrot, 0);

		}
	}

	private GameObject FindNearestCover()
	{
		GameObject nearestObj = cover[0];
		Vector2 nearestPos;
		Vector2 testPos;
		float testDistance;
		float nearestDistance;

		if (cover != null)
		{
			nearestPos.x = gameObject.transform.position.x - cover[0].transform.position.x;
			nearestPos.y = gameObject.transform.position.x - cover[0].transform.position.z;
			nearestDistance = Mathf.Sqrt((nearestPos.x * nearestPos.x) + (nearestPos.y * nearestPos.y));

			for (int i = 1; i < cover.Length; i++)
			{
				testPos.x = gameObject.transform.position.x - cover[i].transform.position.x;
				testPos.y = gameObject.transform.position.x - cover[i].transform.position.z;
				testDistance = Mathf.Sqrt((testPos.x * testPos.x) + (testPos.y * testPos.y));

				if (testDistance < nearestDistance)
				{
					nearestDistance = testDistance;
					nearestObj = cover[i];
				}
			}
		}
		return nearestObj;
	}

	private void MoveToRandomSpot()
	{
		// extents of level
		float minPos = -15.0f;
		float maxPos = 15.0f;

		// get random spot within extents to move towards
		float moveX = Random.Range(minPos, maxPos);
		float moveY = gameObject.transform.position.y;
		float moveZ = Random.Range(minPos, maxPos);

		// create vector3 for moving towards
		Vector3 movePos = new Vector3(moveX, moveY, moveZ);

		// move enemy this direction
		agent.SetDestination(movePos);
		//gameObject.transform.LookAt(movePos);

		isMoving = true;
	}

	private void MoveTowardsCover(Vector3 coverPos)
	{
		//var step = speed * Time.deltaTime;
		//transform.position = Vector3.MoveTowards(transform.position, coverPos, step);

		agent.SetDestination(coverPos);
		//gameObject.transform.LookAt(coverPos);
		isHiding = true;
	}
}
