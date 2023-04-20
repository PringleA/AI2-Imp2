using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public float damage = 5.0f;
	public float range = 40.0f;
    public float fireRate = 0.4f;
    public float dmgFarMult = 0.4f;
	public float dmgMedMult = 0.8f;
    public DistRange dist;
	public PlayerController player;
    float shootStart;
    bool canFire = true;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

	private PlayerDist shotDist = PlayerDist.TOOFAR;

	private void Awake()
    {
		dist.far = 30.0f;
		dist.medium = 23.0f;
		dist.near = 12.5f;
	}

    void Update()
    {
        if (shootStart < fireRate)
            shootStart += Time.fixedDeltaTime;

        else if (shootStart >= fireRate)
            canFire = true;

        if (Input.GetButton("Fire1"))
        {
			muzzleFlash.Play();
            if (canFire)
            {
                Shoot();
                canFire = false;
                shootStart = 0;
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
		int mask = (1 << LayerMask.NameToLayer("EnemyCollider"));
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            EnemyClass enemy = hit.transform.GetComponentInParent<EnemyClass>();

			if (enemy != null)
            {
				
				float overallDmg = damage;
                float distBetween = Vector3.Distance(player.transform.position, enemy.transform.position);
				shotDist = FindEnemyDist(distBetween);

                switch (shotDist)
                {
					// do no damage if enemy is too far away
					case PlayerDist.TOOFAR:
                        {
                            overallDmg = 0;
                            break;
                        }
                    // lower damage if far or medium dist away
					case PlayerDist.FAR:
                        {
                            overallDmg *= dmgFarMult;
                            break;
                        }
					case PlayerDist.MEDIUM:
                        {
                            overallDmg *= dmgMedMult;
                            break;
                        }
                    // else do nothing to dmg
					default:
                        break;
                }

                enemy.TakeDamage(overallDmg);
				//enemy.playerVisible = true;
				enemy.transform.LookAt(player.transform.position);
				enemy.behaviour.state = EnemyState.LOOK;
                enemy.ShootRaycast();
                enemy.healthBarTimer = 0;


                string sDamage = "Enemy hit for ";
                sDamage += overallDmg;
                sDamage += " damage.";
                Debug.Log(sDamage);
			}
        }
    }
	private PlayerDist FindEnemyDist(float distance)
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
