﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventLookAtFlag : MonoBehaviour {
    private bool mIsCameraView;
	// Use this for initialization
	void Start () {
        mIsCameraView = false;
	}
    void Update()
    {
    }
    void OnBecameInvisible()
    {
        mIsCameraView = true;
    }

    public bool GetFlag()
    {
        return true;
    }
}
