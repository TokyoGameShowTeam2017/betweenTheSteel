using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventPlayerMovePointObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnWillRenderObject()
    {
        if (Camera.current.tag == "RawCamera")
        {
            //mCameraViewObject = true;
        }
    }
}
