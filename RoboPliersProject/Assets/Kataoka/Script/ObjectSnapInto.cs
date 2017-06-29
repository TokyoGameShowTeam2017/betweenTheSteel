using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSnapInto : MonoBehaviour
{

    public GameObject[] m_Collision;

    private Vector3 mPosition;
    private Quaternion mQuaternion;

    private ArmManager mArm;
    // Use this for initialization
    void Start()
    {
        mPosition = transform.position;
        mQuaternion = transform.rotation;

        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var i in m_Collision)
        {
            if (i.GetComponent<ObjectCollision>().GetCollisionFlag()&&
                mArm.GetEnablArmCatchingObject()==null||
                i.transform.position.y<=-50.0f)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = mPosition;
                transform.rotation = mQuaternion;

                break;
            }
        }

        //初期化
        foreach (var i in m_Collision)
        {
            if (i == null) continue;
            i.GetComponent<ObjectCollision>().IsFlagFalse();
        }
    }
}
