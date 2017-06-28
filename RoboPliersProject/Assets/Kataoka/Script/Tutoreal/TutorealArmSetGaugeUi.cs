using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorealArmSetGaugeUi : MonoBehaviour
{
    private bool mLoadingFlag;
    private RectTransform mRect;
    private float mLerpCount;
    //状態テキスト
    private Text mPlayerStateText;
    //ダウンロード完了したか
    private bool mDonloadFlag;
    //ダウンロードするアーム
    private bool mArm1;
    private bool mArm2;
    private bool mArm3;
    private bool mArm4;
    //チュートリアルプレイヤー
    private PlayerTutorialControl mPlayerTutorial;

    // Use this for initialization
    void Start()
    {
        mLoadingFlag = false;
        mDonloadFlag = false;
        mLerpCount = 0.0f;
        mRect = GetComponent<RectTransform>();
        mPlayerStateText = GameObject.FindGameObjectWithTag("PlayerStateText").GetComponent<Text>();
        mPlayerTutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //631
        if (mLoadingFlag)
        {
            mLerpCount += 0.5f*Time.deltaTime;
            mPlayerStateText.text = "DownLoading...";
        }
        if (mLerpCount >= 1.0f)
        {
            mLoadingFlag = false;
            if (mArm1) mPlayerTutorial.SetIsActiveArm(0, true);
            if (mArm2) mPlayerTutorial.SetIsActiveArm(1, true);
            if (mArm3) mPlayerTutorial.SetIsActiveArm(2, true);
            if (mArm4) mPlayerTutorial.SetIsActiveArm(3, true);

            mPlayerStateText.text="";
            mLerpCount = 0.0f;
        }
        mRect.anchoredPosition = new Vector2(Mathf.Lerp(-631.0f, 0.0f, mLerpCount), 0.0f);
    }
    public void IsLoading(bool arm1, bool arm2, bool arm3, bool arm4)
    {
        mLoadingFlag = true;
        mArm1 = arm1;
        mArm2 = arm2;
        mArm3 = arm3;
        mArm4 = arm4;
    }
}
