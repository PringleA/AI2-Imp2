using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RayController : MonoBehaviour
{
    public Vector3 rayPosition;
	public Vector3 rayForward;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rayPosition = gameObject.transform.position;
		rayForward = gameObject.transform.forward;
	}
}
