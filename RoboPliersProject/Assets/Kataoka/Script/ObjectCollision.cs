using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//あたり判定しているかどうかフラグ
public class ObjectCollision : MonoBehaviour
{
    private bool mIsCollision = false;

    public void OnCollisionStay(Collision collision)
    {
        mIsCollision = true;
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.name == "ResetObject") 
        {
            mIsCollision = true;
            return;
        }

        if (other.name.Substring(0, 4) != "Plie" &&
            other.GetComponent<CatchObject>() == null &&
            other.tag != "Player")
        {
            if (other.GetComponent<BoxCollider>() == null)
                mIsCollision = true;
            else
            {
                if (!other.GetComponent<BoxCollider>().isTrigger)
                    mIsCollision = true;
            }
        }
    }

    public bool GetCollisionFlag()
    {
        return mIsCollision;
    }
    public void IsFlagFalse()
    {
        mIsCollision = false;
    }



}
