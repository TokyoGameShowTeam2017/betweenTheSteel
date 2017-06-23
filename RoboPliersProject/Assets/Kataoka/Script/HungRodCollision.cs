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
        if ("Bone" == other.name.Substring(0, 4) && mArmManager.GetEnablArmCatchingObject() != null)
        {
            //ArmManager.HookState state = mArmManager.GetArmInputYAndIsGround();
            //print(state.armInputY + ":" + state.playerIsGround);

            //if(state.armInputY > 0.0f)
            //{
            //    other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Static);
            //}
            //else if (state.armInputY < 0.0f && state.playerIsGround)
            //{
            //    other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
            //}


            mChangeCount += Time.deltaTime;
            if (mChangeCount >= 1.0f)
            {
                other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Static);
            }
        }
        if ("Bone" == other.name.Substring(0, 4))
        {
            other.GetComponent<RodTurnBone>().HungFlagTrue();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //if ("Bone" == other.name.Substring(0, 4))
        //{
        //    mChangeCount = 0.0f;
        //    other.GetComponent<RodTurnBone>().GetRod().GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
        //    mIsCollision = false;
        //}
    }




}
