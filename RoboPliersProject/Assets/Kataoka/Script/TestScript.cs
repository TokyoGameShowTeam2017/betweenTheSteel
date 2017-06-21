using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    //現在いる場所
    private Transform mTrans;
    //巡回ポイント
    public GameObject m_ZyunkaiPoint;
    //巡回ポイントのリスト
    private List<GameObject> m_ZyunkaiPoints;
    //どこを巡回しているか
    private int num;
    //移動補間用
    private float mLerpTime;
    //回転補間用
    private float mLerpRotateTime;
    // Use this for initialization
    void Start()
    {

        num = 0;
        mLerpRotateTime = 0.0f;
        mLerpRotateTime = 0.0f;
        m_ZyunkaiPoints = new List<GameObject>();
        //巡回オブジェクトの子を取得
        Transform[] mTransforms;
        mTransforms = m_ZyunkaiPoint.transform.GetComponentsInChildren<Transform>();
        foreach (Transform trans in mTransforms)
        {
            if (trans.gameObject.name != m_ZyunkaiPoint.name)
            {
                m_ZyunkaiPoints.Add(trans.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        mLerpTime += Time.deltaTime;
        transform.position = Vector3.Lerp(mTrans.position,m_ZyunkaiPoints[num].transform.position,mLerpTime);

        Debug.Log(mTrans.position);
        if (mLerpTime >= 1.0f)
        {
            mLerpTime = 0.0f;
            if (num >= m_ZyunkaiPoints.Count - 1) num = 0;
            else num++;
        }
    }


}
