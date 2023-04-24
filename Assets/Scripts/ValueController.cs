using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueController : MonoBehaviour
{
	public States[] states = new States[3];
	public float uncertainty = 0.2f;

	public DistRange dist;
	public ShotChance shotChance;
	public PlayerDist playerDist;

	// Start is called before the first frame update
	void Awake()
    {
		CreateValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void CreateValues()
	{
		dist.far = 25.0f;
		dist.medium = 18.0f;
		dist.near = 10.5f;

		shotChance.high = 0.8f;
		shotChance.medium = 0.5f;
		shotChance.low = 0.3f;

		// aggressive chances (lowered by uncertainty later)
		states[2].hide = 0.8f;
		states[2].look = 0.8f;
		states[2].move = 1.0f;
		states[2].shoot = 1.0f;

		// neutral chances (lowered by uncertainty later)
		states[1].hide = 1.0f;
		states[1].look = 1.0f;
		states[1].move = 1.0f;
		states[1].shoot = 1.0f;

		// passive chances (lowered by uncertainty later)
		states[0].hide = 1.0f;
		states[0].look = 1.0f;
		states[0].move = 0.8f;
		states[0].shoot = 0.8f;
	}

	public bool AttemptShot(float distance, EnemyMood mood)
	{
		PlayerDist testDist = FindPlayerDist(distance);

		// depending on mood, enemy will actually shoot at player or not
		switch (mood)
		{
			case EnemyMood.PASSIVE:
				{
					switch (testDist)
					{
						case PlayerDist.NEAR:
							return CalculateShot(testDist);
						case PlayerDist.MEDIUM:
							return CalculateShot(testDist);
						case PlayerDist.FAR:
							return false;
						default:
							return false;
					}
				}
			case EnemyMood.NEUTRAL:
				{
					switch (testDist)
					{
						case PlayerDist.NEAR:
							return CalculateShot(testDist);
						case PlayerDist.MEDIUM:
							return CalculateShot(testDist);
						case PlayerDist.FAR:
							return false;
						default:
							return false;
					}
				}
			case EnemyMood.AGGRESSIVE:
				{
					switch (testDist)
					{
						case PlayerDist.NEAR:
							return CalculateShot(testDist);
						case PlayerDist.MEDIUM:
							return CalculateShot(testDist);
						case PlayerDist.FAR:
							return CalculateShot(testDist);
						default:
							return false;
					}
				}
			default:
				return false;
		}
	}

	public bool CalculateShot(PlayerDist dist)
	{
		switch (dist)
		{
			case PlayerDist.NEAR:
				{
					if (Random.Range(0.0f, 1.0f) <= shotChance.high)
						return true;
					else return false;
				}
			case PlayerDist.MEDIUM:
				{
					if (Random.Range(0.0f, 1.0f) <= shotChance.medium)
						return true;
					else return false;
				}
			case PlayerDist.FAR:
				{
					if (Random.Range(0.0f, 1.0f) <= shotChance.low)
						return true;
					else return false;
				}
			default:
				return false;
		}
	}

	public PlayerDist FindPlayerDist(float distance)
	{
		if (0 <= distance && distance < dist.near)
			return PlayerDist.NEAR;

		if (distance >= dist.near && distance < dist.medium)
			return PlayerDist.MEDIUM;

		if (distance >= dist.medium && distance <= dist.far)
			return PlayerDist.FAR;

		else
			return PlayerDist.TOOFAR;
	}
}
public struct States
{
	public float look;
	public float hide;
	public float move;
	public float shoot;
}

public struct DistRange
{
	public float far;
	public float medium;
	public float near;
}

public struct ShotChance
{
	public float high;
	public float medium;
	public float low;
}

public enum EnemyMood
{
	PASSIVE = 0,
	NEUTRAL = 1,
	AGGRESSIVE = 2
}

public enum EnemyState
{
	HIDE = 0,
	MOVE = 1,
	LOOK = 2,
	SHOOT = 3
}

public enum PlayerDist
{
	FAR = 0,
	MEDIUM = 1,
	NEAR = 2,
	TOOFAR = 3
}

