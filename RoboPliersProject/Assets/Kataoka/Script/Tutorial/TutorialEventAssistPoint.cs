﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventAssistPoint : MonoBehaviour {
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;
    [SerializeField, Tooltip("アシストしたいオブジェクト")]
    public GameObject m_AssistObject;


    [SerializeField, Tooltip("プレイヤー移動させるか"), Space(15), HeaderAttribute("目的を達成した時のプレイヤーの状態")]
    public bool m_PlayerClerMove;
    [SerializeField, Tooltip("プレイヤーカメラ移動させるか")]
    public bool m_PlayerClerCameraMove;
    [SerializeField, Tooltip("プレイヤーアーム動かせるか")]
    public bool m_PlayerClerArmMove;
    [SerializeField, Tooltip("プレイヤーアーム掴めるか")]
    public bool m_PlayerClerArmCath;
    [SerializeField, Tooltip("プレイヤーアーム離せるか")]
    public bool m_PlayerClerArmNoCath;
    [SerializeField, Tooltip("プレイヤーアームリセットフラグ")]
    public bool m_PlayerClerArmReset;
    [SerializeField, Tooltip("プレイヤーアームセレクトフラグ")]
    public bool m_PlayerClerArmSelect;
    [SerializeField, Tooltip("プレイヤーアーム伸びフラグ")]
    public bool m_PlayerClerArmExtend;

    [SerializeField, Tooltip("プレイヤー移動させるか"), Space(15), HeaderAttribute("テキストが終わった時のプレイヤーの状態")]
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
    //プレイヤーチュートリアル
    private PlayerTutorialControl mPlayerTutorial;
    //プレイヤーテキスト
    private TutorialText mPlayerText;
	// Use this for initialization
	void Start () {
        mPlayerTutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mPlayerText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!GetComponent<TutorialEventFlag>().GetIventFlag()||
            mPlayerText.GetDrawTextFlag()) return;
        mPlayerTutorial.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutorial.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutorial.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutorial.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutorial.SetIsArmRelease(!m_PlayerArmNoCath);

        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);

        //エイムアシストしてたら
        if (m_AssistObject != null)
        {
            if (mPlayerTutorial.GetIsAimAssistName(m_AssistObject.name.Substring(0, 4)))
            {
                GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);
                //次のイベントテキスト有効化
                if (m_IventCollisions.Length != 0)
                    for (int i = 0; m_IventCollisions.Length > i; i++)
                    {
                        m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                    }
                SoundManager.Instance.PlaySe("Answer");
                mPlayerTutorial.SetIsArmMove(!m_PlayerClerArmMove);
                mPlayerTutorial.SetIsPlayerMove(!m_PlayerClerMove);
                mPlayerTutorial.SetIsCamerMove(!m_PlayerClerCameraMove);
                mPlayerTutorial.SetIsArmCatchAble(!m_PlayerClerArmCath);
                mPlayerTutorial.SetIsArmRelease(!m_PlayerClerArmNoCath);
                mPlayerTutorial.SetIsResetAble(!m_PlayerArmReset);
                mPlayerTutorial.SetAllIsArmSelectAble(!m_PlayerArmSelect);
                mPlayerTutorial.SetIsArmStretch(!m_PlayerArmExtend);
                Destroy(gameObject);
            }
        }
        else
        {
            if (mPlayerTutorial.GetIsAimAssistAll())
            {
                GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);
                //次のイベントテキスト有効化
                if (m_IventCollisions.Length != 0)
                    for (int i = 0; m_IventCollisions.Length > i; i++)
                    {
                        m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                    }
                SoundManager.Instance.PlaySe("Answer");
                mPlayerTutorial.SetIsArmMove(!m_PlayerClerArmMove);
                mPlayerTutorial.SetIsPlayerMove(!m_PlayerClerMove);
                mPlayerTutorial.SetIsCamerMove(!m_PlayerClerCameraMove);
                mPlayerTutorial.SetIsArmCatchAble(!m_PlayerClerArmCath);
                mPlayerTutorial.SetIsArmRelease(!m_PlayerClerArmNoCath);
                mPlayerTutorial.SetIsResetAble(!m_PlayerClerArmReset);
                mPlayerTutorial.SetAllIsArmSelectAble(!m_PlayerClerArmSelect);
                mPlayerTutorial.SetIsArmStretch(!m_PlayerClerArmExtend);
                Destroy(gameObject);
            }
        }
        
	}
}
