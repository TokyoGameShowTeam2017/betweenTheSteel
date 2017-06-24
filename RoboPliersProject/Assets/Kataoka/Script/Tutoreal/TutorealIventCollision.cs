using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventCollision : MonoBehaviour {
    private bool mIsCollision;

	// Use this for initialization
	void Start () {
        mIsCollision = false;
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            mIsCollision = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            mIsCollision = false;
        }
    }

    public bool GetIsCollision()
    {
        return mIsCollision;
    }


}
