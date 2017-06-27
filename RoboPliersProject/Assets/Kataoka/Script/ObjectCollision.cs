using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//あたり判定しているかどうかフラグ
public class ObjectCollision : MonoBehaviour
{
    private bool mIsCollision = false;
    void Update()
    {
    }

    public void OnCollisionStay(Collision collision)
    {
        mIsCollision = true;
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.name.Substring(0, 4) != "Plie" &&
            other.GetComponent<CatchObject>() == null &&
            other.name != "Player")
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

    public void OnTriggerExit(Collider other)
    {
        if (other.name.Substring(0, 4) != "Plie" &&
            other.GetComponent<CatchObject>() == null&&
            other.name!="Player")
        {
            if (other.GetComponent<BoxCollider>() == null)
                mIsCollision = false;
            else
            {
                if (!other.GetComponent<BoxCollider>().isTrigger)
                    mIsCollision = false;
            }
        }
    }

    public bool GetCollisionFlag()
    {
        return mIsCollision;
    }




}
