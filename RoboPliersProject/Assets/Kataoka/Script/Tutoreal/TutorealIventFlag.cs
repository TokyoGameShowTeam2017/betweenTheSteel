using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventFlag : MonoBehaviour {

    private bool mIsPlayIventFlag;
	// Use this for initialization
	void Start () {
        mIsPlayIventFlag = false;
	}
    public void PlayIvent()
    {
        mIsPlayIventFlag = true;
    }
    public bool GetIventFlag()
    {
        return mIsPlayIventFlag;
    }
}
