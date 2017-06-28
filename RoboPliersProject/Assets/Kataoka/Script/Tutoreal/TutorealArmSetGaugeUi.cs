using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorealArmSetGaugeUi : MonoBehaviour
{
    private bool mLoadingFlag;
    private RectTransform mRect;
    private float mLerpCount;
    //ダウンロード完了したか
    private bool mDonloadFlag;
    // Use this for initialization
    void Start()
    {
        mLoadingFlag = false;
        mDonloadFlag = false;
        mLerpCount = 0.0f;
        mRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //631
        if (mLoadingFlag)
        {
            mLerpCount += Time.deltaTime;
        }
        if (mLerpCount >= 1.0f)
        {
            mLoadingFlag = false;
            mDonloadFlag = true;
            mLerpCount = 0.0f;
        }
        mRect.position = new Vector2(Mathf.Lerp(-631.0f, 0.0f, mLerpCount), 0.0f);
    }
    public void IsLoading()
    {
        mLoadingFlag = true;
        mDonloadFlag = false;
    }
}
