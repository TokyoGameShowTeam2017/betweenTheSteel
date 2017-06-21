using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWallCollision : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //GetComponent<CatchObject>().SetNoRigidBody(CatchObject.CatchType.Dynamic);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>().GetEnablArmCatchingObject() == null) return;
        Debug.Log(GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>().GetEnablArmCatchingObject().gameObject.name);
    }


}
