using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RodTurnCollision : MonoBehaviour
{
    //曲げ方の種類
    public bool m_TestFlag = false;
    //アームマネージャー
    private ArmManager mArm;
    //重さで曲げるインデックス
    private int mBoneRotateIndex;
    //プレイヤーマネ＾－じ
    private PlayerManager mPlayerManager;
    //曲げ限界カウント
    private int mRotateCount;
    //曲げフラグ
    private bool mRotateFlag;
    //曲げのindex
    private int mRotateIndex;
    enum RotateArmDire
    {
        LEFT,
        RIGHT,
        NO
    }
    private RotateArmDire mDire;
    private RotateArmDire mNowDire;
    void Start()
    {
        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        mPlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        mBoneRotateIndex = 1;
        mRotateCount = 0;
        mRotateIndex = 0;
        mRotateFlag = false;
        mDire = RotateArmDire.NO;
        mNowDire = RotateArmDire.LEFT;
    }

    // Update is called once per frame
    void Update()
    {
        //if (mArm.GetEnablArmCatchingObject() == null ||
        //    mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>() == null)
        //{
        //    mRotateFlag = true;
        //}
        //else
        //{
        //    RodTurnBone rodBone = mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>();

        //    if (mArm.GetEnablArmCatchingObject().GetComponent<CatchObject>().catchType == CatchObject.CatchType.Dynamic)
        //    {
        //        for (int i = 0; i <= 3; i++)
        //        {
        //            if (mArm.GetEnablPliers() == mArm.GetPliers(i)) continue;
        //            GameObject mainRod = mArm.GetPliersCatchRod(i);
        //            GameObject mainRod2 = mArm.GetEnablePliersCatchRod();
        //            if (mainRod == mainRod2)
        //            {
        //                float velo = mArm.GetEnablePliersRollValue();
        //                if (velo < 0.0f) mDire = RotateArmDire.LEFT;
        //                else if (velo > 0.0f) mDire = RotateArmDire.RIGHT;

        //                if (mDire != mNowDire)
        //                {
        //                    mRotateIndex = rodBone.GetBoneNumber();
        //                    mNowDire = mDire;
        //                    mRotateCount = 0;
        //                }


        //                if (mDire == RotateArmDire.NO || mRotateCount > 3) return;

        //                if (rodBone.GetIsMaxRotateIndex(mRotateIndex))
        //                {
        //                    //rodBone.Flag(mRotateIndex);
        //                    if (rodBone.GetRod().GetComponent<Rod>().m_point == Rod.StartPoint.LEFT_POINT)
        //                        mRotateIndex++;
        //                    else
        //                        mRotateIndex--;

        //                    mRotateCount++;
        //                }

        //                //軸回転
        //                mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().
        //                    RotateAxisVelo(GameObject.FindGameObjectWithTag("RawCamera").
        //                    transform.forward,
        //                    mArm.GetEnablePliersRollValue(),
        //                    mRotateIndex);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        float velo = mArm.GetEnablePliersRollValue();
        //        if (velo < 0.0f) mDire = RotateArmDire.LEFT;
        //        else if (velo > 0.0f) mDire = RotateArmDire.RIGHT;

        //        if (mDire != mNowDire)
        //        {
        //            mRotateIndex = rodBone.GetBoneNumber();
        //            mNowDire = mDire;
        //            mRotateCount = 0;
        //        }

        //        if (mDire == RotateArmDire.NO || mRotateCount > 3) return;

        //        if (rodBone.GetIsMaxRotateIndex(mRotateIndex))
        //        {
        //            //rodBone.Flag(mRotateIndex);
        //            if (rodBone.GetRod().GetComponent<Rod>().m_point == Rod.StartPoint.LEFT_POINT)
        //                mRotateIndex++;
        //            else
        //                mRotateIndex--;

        //            mRotateCount++;
        //        }

        //        //軸回転
        //        mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().
        //            RotateAxisVelo(GameObject.FindGameObjectWithTag("RawCamera").
        //            transform.forward,
        //            mArm.GetEnablePliersRollValue(),
        //            mRotateIndex);
        //        return;
        //    }
        //}
        //    //重さで曲がる処理
        //else
        //{
        //    if (mArm.GetEnablArmCatchingObject() != null)
        //    {
        //        int index = 1;
        //        RodTurnBone rod=mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>();
        //        while (true)
        //        {
        //            if (!rod.GetIsMaxRotateIndex(index)) break;
        //            index++;
        //        }
        //        mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().RotateBone2(index);
        //        mPlayerManager.MoveAxisMoveObject(mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().GetVelocity());
        //    }
        //}
        if (mArm.GetEnablArmCatchingObject() == null ||
            mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>() == null) return;

        if (mArm.GetEnablArmCatchingObject().
            GetComponent<RodTurnBone>().GetRod().
            GetComponent<Rod>().GetHongFlag()) return;

        


        //2本のアームでつかむ処理
        if (mArm.GetEnablArmCatchingObject().GetComponent<CatchObject>().catchType ==
            CatchObject.CatchType.Dynamic)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (mArm.GetEnablPliers() == mArm.GetPliers(i)) continue;
                GameObject mainRod = mArm.GetPliersCatchRod(i);
                GameObject mainRod2 = mArm.GetEnablePliersCatchRod();
                if (mainRod == mainRod2)
                {
                    mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().
                  RotateAxisVelo(mArm.GetEnablArm().transform.forward,
                  mArm.GetEnablePliersRollValue());
                }
            }
        }
        else
        {

            mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().
                RotateAxisVelo(mArm.GetEnablArm().transform.forward,
                mArm.GetEnablePliersRollValue());

            //RodTurnBone rodBone = mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>();

            //float velo = mArm.GetEnablePliersRollValue();
            //if (velo < 0.0f) mDire = RotateArmDire.LEFT;
            //else if (velo > 0.0f) mDire = RotateArmDire.RIGHT;

            //if (mDire != mNowDire)
            //{
            //    mBoneRotateIndex = rodBone.GetBoneNumber();
            //    mRotateCount = 0;
            //}

            //if (rodBone.GetIsMaxRotateIndex(mRotateIndex))
            //{
            //    if (rodBone.GetRod().GetComponent<Rod>().m_point == Rod.StartPoint.LEFT_POINT)
            //        mRotateIndex++;
            //    else
            //        mRotateIndex--;
            //    mRotateCount++;
            //}

        }
    }
}
