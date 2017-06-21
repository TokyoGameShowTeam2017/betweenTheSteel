using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectCollision : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<DeleteObject>() == null) return;
        if (name == "BreakCollision1")
            other.GetComponent<DeleteObject>().mIsCollision1 = true;
        if (name == "BreakCollision2")
            other.GetComponent<DeleteObject>().mIsCollision2 = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        int a = 0;
        a++;
    }

}
