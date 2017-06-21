using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRayTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        RaycastHit hit;

        Vector3 rayPosition= transform.position;
        float radius = 1;

        Gizmos.DrawSphere(transform.position, radius);

        if (Physics.SphereCast(rayPosition, radius, transform.forward, out hit, 0.1f))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, hit.point);
        }
    }


}
