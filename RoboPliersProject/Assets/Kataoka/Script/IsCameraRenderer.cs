using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCameraRenderer : MonoBehaviour {
    private bool mIsRenderer;
	// Use this for initialization
	void Start () {
        mIsRenderer = false;
	}
	
	// Update is called once per frame
	void Update () {
        mIsRenderer = false;
	}

    public void OnWillRenderObject()
    {
        if (Camera.current.tag == "RawCamera")
        {
            mIsRenderer = true;
        }
    }
    public bool GetAtFalg()
    {
        return mIsRenderer;
    }

}
