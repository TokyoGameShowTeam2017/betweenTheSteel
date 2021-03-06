﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventCamera : MonoBehaviour
{
    //プレイヤーカメラ
    private Transform m_PlayerCamera;
    //線形補間系
    private float mLertTime;
    private float mLertNum;

    //カメラのポジション行くべき位置
    private Vector3 mCameraEndPos;
    private Vector3 mCameraStartPos;
    //カメラの注視点の位置
    private Vector3 mTargetStartPos;
    private Vector3 mTargetEndPos;


    //カメラ初期位置
    private Vector3 mFirstCameraPos;
    private Vector3 mFirstTargetPos;
    //行くかどうか
    private bool m_IsGoCamera;

    //ズームする時間
    private float mZoomTime;
    //プレイヤーチュートリアル
    private PlayerTutorialControl mTutorialPlayer;
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;
    [SerializeField, Tooltip("ターゲットポイントたち")]
    public GameObject[] m_TargetPoints;
    [SerializeField, Tooltip("カメラポジションたち")]
    public GameObject[] m_CameraPoints;
    //テキスト
    private TutorialText mTutorialText;

    //一回しか入らないために
    private bool mFlag = true;

    [SerializeField, Tooltip("ブロックをを先にDrawさせるか")]
    public bool m_BeforeDrawBlock;
    [SerializeField, Tooltip("ズームするかどうか"), Space(15)]
    public bool m_IsNoZoom;
    [SerializeField, Tooltip("コントローラーをカメラ移動中に表示させるか")]
    public bool m_CameraMoveDrawController;

    //[SerializeField, Tooltip("2回目以降ズームするかどうか"), Space(15)]
    //public bool m_IsSecondCameraZoom;
    [SerializeField, Tooltip("プレイヤー移動させるか"), Space(15), HeaderAttribute("カメラ補間が終わった時のプレイヤー状態")]
    public bool m_PlayerMove;
    [SerializeField, Tooltip("プレイヤーカメラ移動させるか")]
    public bool m_PlayerCameraMove;
    [SerializeField, Tooltip("プレイヤーアーム動かせるか")]
    public bool m_PlayerArmMove;
    [SerializeField, Tooltip("プレイヤーアーム掴めるか")]
    public bool m_PlayerArmCath;
    [SerializeField, Tooltip("プレイヤーアーム離せるか")]
    public bool m_PlayerArmNoCath;
    [SerializeField, Tooltip("プレイヤーアームリセットフラグ")]
    public bool m_PlayerArmReset;
    [SerializeField, Tooltip("プレイヤーアームセレクトフラグ")]
    public bool m_PlayerArmSelect;
    [SerializeField, Tooltip("プレイヤーアーム伸びフラグ")]
    public bool m_PlayerArmExtend;
    //座標index
    private int mPointIndex = 1;
    private int mCurPointIndex = 0;
    // Use this for initialization

    private bool mIsEnd;
    //サイズ1の時のフラグ
    private bool mIsOneSize;
    void Start()
    {
        mPointIndex = 0;
        mCurPointIndex = 0;
        mIsEnd = false;
        mIsOneSize = false;
    }
    private void CameraStart()
    {
        mLertTime = 0.0f;
        mZoomTime = 0.0f;
        mLertNum = 0.0f;
        m_IsGoCamera = true;
        m_PlayerCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>().GetPlayerCameraTr();

        mTargetStartPos = m_PlayerCamera.position + (m_PlayerCamera.transform.forward * 2.0f);
        mTargetEndPos = m_TargetPoints[mPointIndex].transform.position;

        mCameraStartPos = m_PlayerCamera.position;
        mCameraEndPos = m_CameraPoints[mPointIndex].transform.position;

        mTutorialText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();

        mTutorialPlayer = GameObject.FindGameObjectWithTag("Player").
            GetComponent<PlayerTutorialControl>();

        if (m_BeforeDrawBlock)
            m_IventCollisions[0].GetComponent<PlayerTextIvent>().GetIvent().
                GetComponent<TutorialEventSetObject>().
                transform.FindChild("Block").gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorialEventFlag>().GetIventFlag()) return;

        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(m_CameraMoveDrawController);

        if (mFlag)
        {
            CameraStart();
            mFirstCameraPos = mCameraStartPos;
            mFirstTargetPos = mTargetStartPos;
            mFlag = false;
        }

        //スタート処理みたいな
        if (mCurPointIndex != mPointIndex && !mIsEnd)
        {
            CameraStart();
            mCurPointIndex = mPointIndex;
        }


        //もし最後の移動だったらプレイヤーに戻す
        if (!mTutorialText.GetDrawTextFlag() && !mIsEnd)
        {
            CameraStart();
            mCameraEndPos = mFirstCameraPos;
            mTargetEndPos = mFirstTargetPos;
            mIsEnd = true;
        }

        mPointIndex = mTutorialText.GetCreenCount();
        mLertNum += 50.0f * Time.deltaTime;
        mLertNum = Mathf.Clamp(mLertNum, 0.0f, 90.0f);

        mLertTime = Mathf.Sin(mLertNum * Mathf.Deg2Rad);

        //頭が働かない
        //ズーム機能
        if (mLertTime >= 1.0f && !mIsEnd && !m_IsNoZoom)
        {
            mZoomTime += Time.deltaTime;
            if (mZoomTime <= 3.0f)
            {
                Vector3 vec = (mTargetEndPos - mCameraEndPos).normalized;
                mCameraEndPos += vec * Time.deltaTime;
            }
        }

        m_PlayerCamera.position = Vector3.Lerp(mCameraStartPos, mCameraEndPos, mLertTime);
        m_PlayerCamera.LookAt(Vector3.Lerp(mTargetStartPos, mTargetEndPos, mLertTime));

        //役目が終わったら消す
        if (mLertTime >= 1.0f && mIsEnd)
        {
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(!m_CameraMoveDrawController);
            //次のイベントテキスト有効化
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }
            mTutorialPlayer.SetIsArmMove(!m_PlayerArmMove);
            mTutorialPlayer.SetIsPlayerMove(!m_PlayerMove);
            mTutorialPlayer.SetIsCamerMove(!m_PlayerCameraMove);
            mTutorialPlayer.SetIsArmCatchAble(!m_PlayerArmCath);
            mTutorialPlayer.SetIsArmRelease(!m_PlayerArmNoCath);
            mTutorialPlayer.SetIsResetAble(!m_PlayerArmReset);
            mTutorialPlayer.SetAllIsArmSelectAble(!m_PlayerArmSelect);
            mTutorialPlayer.SetIsArmStretch(!m_PlayerArmExtend);
            Destroy(gameObject);
        }
    }
}
