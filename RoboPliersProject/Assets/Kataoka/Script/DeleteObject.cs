using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour {
    public bool mIsCollision1;
    public bool mIsCollision2;
    //ボーンかどうか
    private bool mIsBone;
    //消すオブジェクト
    private GameObject mDeleteObject;
	// Use this for initialization
	void Start () {
        mIsCollision1 = false;
        mIsCollision2 = false;
        mDeleteObject = gameObject;
        string name = gameObject.name.Substring(0, 4);
        if (name == "Bone")
        {
            mDeleteObject = gameObject.GetComponent<RodTurnBone>().GetRod();
        }
        int a = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (mIsCollision1 && mIsCollision2)
        {
            string name = gameObject.name.Substring(0, 4);
            if (name == "Bone")
            {
                mDeleteObject = gameObject.GetComponent<RodTurnBone>().GetRod();
            }
            Destroy(mDeleteObject);

        }
        mIsCollision1 = false;
        mIsCollision2 = false;
	}
}
