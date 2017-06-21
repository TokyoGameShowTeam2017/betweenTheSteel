using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//あたり判定しているかどうかフラグ
public class ObjectCollision : MonoBehaviour {
    private bool mIsCollision = false;
    void Update()
    {
        mIsCollision = false;
    }

    public void OnCollisionStay(Collision collision)
    {
        mIsCollision = true;
    }


    public void OnTriggerStay(Collider other)
    {
        mIsCollision = true;
    }

    public bool GetCollisionFlag()
    {
        return mIsCollision;
    }




}
