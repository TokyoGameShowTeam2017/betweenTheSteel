using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodReSet : MonoBehaviour
{
    //子たち
    private List<GameObject> mCollisions;
    //初期位置
    private Vector3 mFirstPosition;
    //初期回転
    private Quaternion mFirstQuaternion;
    //アームマネージャー
    private ArmManager mArm;
    // Use this for initialization
    void Start()
    {
        Transform[] trans;
        trans = transform.GetComponentsInChildren<Transform>();
        mCollisions = new List<GameObject>();
        foreach (var i in trans)
        {
            if (i.name == "SnapIntoCollision")
            {
                mCollisions.Add(i.gameObject);
            }
        }
        mFirstPosition = transform.position;
        mFirstQuaternion = transform.rotation;

        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //スタティックだったらリターン
        if (GetComponent<Rod>().GetCatchType() == CatchObject.CatchType.Static) return;
        foreach (var i in mCollisions)
        {
            if (i == null) continue;
            if ((mArm.GetEnablArmCatchingObject() == null &&
                i.GetComponent<ObjectCollision>().GetCollisionFlag()) ||
                i.transform.position.y <= -50.0f)
            {
                transform.position = mFirstPosition;
                transform.rotation = mFirstQuaternion;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;
            }
        }
        //初期化
        foreach (var i in mCollisions)
        {
            if (i == null) continue;
            i.GetComponent<ObjectCollision>().IsFlagFalse();
        }
    }
}
