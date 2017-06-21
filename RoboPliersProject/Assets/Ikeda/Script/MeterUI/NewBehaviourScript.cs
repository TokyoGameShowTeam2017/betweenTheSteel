using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public Vector3 startpos;
    public GameObject a;

    public float b = 0;
    // Use this for initialization
    void Start () {
        startpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(startpos, a.transform.position, b);
    }
}
