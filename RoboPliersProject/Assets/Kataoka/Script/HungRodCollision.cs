using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungRodCollision : MonoBehaviour {
    //タイプが変わるときのカウント
	
    private float mChangeCount;
    //触れているかどうか
    private bool mIsCollision;
    //アームマネージャー
    private ArmManager mArmManager;
	// Use this for initialization
	void Start () {
        mChangeCount = 0.0f;
        mArmManager=GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
	}
	// Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerStay(Collider other)
    {
        //ある程度曲がっていないといけない
         if ("Bone" == other.name.Substring(0, 4) && mArmManager.GetEnablArmCatchingObject() != null)
         {
             if (!other.GetComponent<RodTurnBone>().GetRoatePointFlag()) return;
         }
        if ("Bone" == other.name.Substring(0, 4) && mArmManager.GetEnablArmCatchingObject() != null)
        {
            ArmManager.HookState state = mArmManager.GetArmInputYAndIsGround();

            Rod catchingrod = other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>();

            CatchObject obj = mArmManager.GetEnablArmCatchingObject();
            if (state.armInputY > 0.0f && catchingrod.GetCatchType() == CatchObject.CatchType.Dynamic)
            {
                other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().IsHong(true);
                mArmManager.GetEnablPliersMove().ForceCatchReleaseHungRod();
                catchingrod.SetCatchType(CatchObject.CatchType.Static);
                mArmManager.GetEnablPliersMove().ForceCatching(obj);
                mIsCollision = true;
                SoundManager.Instance.PlaySe("hook");
            }
            //else if (InputManager.GetMove().magnitude > 0.0f && state.playerIsGround && catchingrod.GetCatchType() == CatchObject.CatchType.Static)
            else if (state.armInputY < 0.0f && state.playerIsGround && catchingrod.GetCatchType() == CatchObject.CatchType.Static)
            {
                other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().IsHong(false);
                mArmManager.GetEnablPliersMove().ForceCatchReleaseHungRod();
                catchingrod.SetCatchType(CatchObject.CatchType.Dynamic);
                mArmManager.GetEnablPliersMove().ForceCatching(obj);
            }


            //mChangeCount += Time.deltaTime;
            //if (mChangeCount >= 1.0f)
            //{
            //    other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Static);
            //}
        }
        if ("Bone" == other.name.Substring(0, 4))
        {
            other.GetComponent<RodTurnBone>().HungFlagTrue();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if ("Bone" == other.name.Substring(0, 4))
        {
            mIsCollision = false;
        }
    }


    public bool GetHungFlag()
    {
        return mIsCollision;
    }

}
