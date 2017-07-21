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
        if (mArm.GetEnablArmCatchingObject() == null ||
            mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>() == null) return;

        Rod rod=mArm.GetEnablArmCatchingObject().
            GetComponent<RodTurnBone>().GetRod().
            GetComponent<Rod>();
        CutRod cutRod=rod.gameObject.GetComponent<CutRod>();

        bool hungflag = rod.GetHongFlag();
        bool actionFlag = cutRod.GetActionFlag();
        bool fixFlag = cutRod.GetFixBothEndsFlag();
        //スポーンされてなくて両端固定されてたら曲げれない
        if (rod.GetHongFlag()||(!cutRod.GetActionFlag()&&cutRod.GetFixBothEndsFlag())) return;

        


        //2本のアームでつかむ処理
        if (mArm.GetEnablArmCatchingObject().GetComponent<CatchObject>().catchType ==
            CatchObject.CatchType.Dynamic)
        {
            //for (int i = 0; i <= 3; i++)
            //{
            //    if (mArm.GetEnablPliers() == mArm.GetPliers(i)) continue;
            //    GameObject mainRod = mArm.GetPliersCatchRod(i);
            //    GameObject mainRod2 = mArm.GetEnablePliersCatchRod();
            //    if (mainRod == mainRod2)
            //    {
            //        mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().
            //      RotateAxisVelo(mArm.GetEnablArm().transform.forward,
            //      mArm.GetEnablePliersRollValue());
            //    }
            //}
        }
        else
        {

            mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().
                RotateAxisVelo(mArm.GetEnablArm().transform.forward,
                mArm.GetEnablePliersRollValue());
        }
    }
}
