using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventCollision : MonoBehaviour {
    private bool mIsCollision;

	// Use this for initialization
	void Start () {
        mIsCollision = false;
	}

    public void OnTriggerStay(Collider other)
    {
        if (other.name.Substring(0,4)=="iron"||
            other.name.Substring(0,4)=="Bone")
            mIsCollision = true;
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name.Substring(0, 4) == "iron" ||
            other.name.Substring(0, 4) == "Bone")
            mIsCollision = false;
        
    }

    public bool GetIsCollision()
    {
        return mIsCollision;
    }


}
