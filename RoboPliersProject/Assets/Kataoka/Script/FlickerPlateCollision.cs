using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerPlateCollision : MonoBehaviour {

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //速度で回す
    public void FlickerPlateRotate(float angleVelo)
    {
        transform.Rotate(transform.right, angleVelo);
    }
}
