using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CoinCollected : MonoBehaviour
{
	[SerializeField] float _degreesPerSecond = 30f;
	[SerializeField] Vector3 _axis = Vector3.forward;

    private void Update()
    {
		transform.Rotate(_axis.normalized * _degreesPerSecond * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        Scores.AddScore(1);
        Destroy(gameObject);
    }
}
