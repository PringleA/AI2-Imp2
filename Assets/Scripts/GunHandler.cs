using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public float damage = 5.0f;
	public float range = 999.0f;
    public float fireRate = 0.5f;
    public PlayerController player;
    float shootStart;
    bool canFire = true;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

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
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, mask))
        {
            EnemyClass enemy = hit.transform.GetComponentInParent<EnemyClass>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
				//enemy.playerVisible = true;
				enemy.transform.LookAt(player.transform.position);
				enemy.behaviour.state = EnemyState.LOOK;
                enemy.ShootRaycast();
                enemy.healthBarTimer = 0;
			}
        }
    }
}
