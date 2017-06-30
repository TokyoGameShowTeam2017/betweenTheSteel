using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventSwitch : MonoBehaviour
{
    private bool mFlag;
    private Vector3 mStartPos;
    // Use this for initialization
    void Start()
    {
        mStartPos = transform.position;
        mFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //苦肉の策
        if (!mFlag)
            transform.position = new Vector3(0.0f, 10000.0f);
        else
            transform.position = mStartPos;
    }

    public void IsCollision(bool flag)
    {
        mFlag = flag;
    }
}
