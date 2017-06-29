using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    //現在の座標
    Vector3 mCurPosition;
    //前回の座標
    Vector3 mPrePosition;
    //速度
    Vector3 mVec;
    //速度の長さ
    float mLeng;
    //一回しか入らないフラグ
    bool mFlag;


    public float m_VecY = 0.05f;
    public float m_JumpPowerY = 20.0f;
    public float m_JumpPowerZ = 30.0f;

    ArmManager mManager;
    //ジャンプしない時間
    float mNoJumpTime;
    // Use this for initialization
    void Start()
    {
        mVec = Vector3.zero;
        mCurPosition = transform.position;
        mPrePosition = transform.position;
        mLeng = 0.0f;
        mFlag = true;
        mNoJumpTime = 0.0f;
        mManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mFlag)
        {
            mNoJumpTime += Time.deltaTime;
            if (mNoJumpTime >= 1.5f)
            {
                mFlag = true;
                mNoJumpTime = 0.0f;
            }
        }
    }

    public void LateUpdate()
    {
        //速度計算
        mCurPosition = transform.position;
        mVec = mCurPosition - mPrePosition;
        mPrePosition = transform.position;
    }

    public void OnTriggerStay(Collider other)
    {
        if (mVec.y >= m_VecY && other.tag == "Player" &&
            mFlag && !mManager.GetIsEnablArmCatching())
        {
            other.GetComponent<PlayerMove>().Jump(mVec.normalized,m_JumpPowerZ , m_JumpPowerY);
            mFlag = false;
        }
    }

}
