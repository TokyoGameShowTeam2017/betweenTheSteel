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
    public GameObject m_ResetParticle;
    //プレイヤー
    private GameObject mPlayer;
    //時間
    private float mCollisionTime;
    // Use this for initialization
    void Start()
    {
        SetChild();

        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        mCollisionTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //スタティックだったらリターン
        if (GetComponent<Rod>().GetCatchType() != CatchObject.CatchType.Static)
        {
            foreach (var i in mCollisions)
            {
                if (i == null) continue;
                if (mArm.GetPliersCatchRod(0) == gameObject ||
                    mArm.GetPliersCatchRod(1) == gameObject ||
                    mArm.GetPliersCatchRod(2) == gameObject ||
                    mArm.GetPliersCatchRod(3) == gameObject) break;


                bool aaa = i.GetComponent<ObjectCollision>().GetCollisionFlag();
                if ((mArm.GetEnablArmCatchingObject() == null &&
                    i.GetComponent<ObjectCollision>().GetCollisionFlag()) ||
                    i.transform.position.y <= -50.0f)
                {
                    Instantiate(m_ResetParticle, transform.position, Quaternion.Euler(0, 0, 0));

                    Vector3 pos = transform.position;
                    if (!GetComponent<CutRod>().m_StartRodFlag)
                        pos = mPlayer.transform.position + new Vector3(0, 5, 0);

                    transform.position = pos;
                    transform.rotation = Quaternion.Euler(0,0,0);
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    break;
                }
            }
        }
        //初期化
        foreach (var i in mCollisions)
        {
            if (i == null) continue;
            i.GetComponent<ObjectCollision>().IsFlagFalse();
        }
    }

    public void SetChild()
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
    }
}
