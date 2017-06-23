/**==========================================================================*/
/**
 * チュートリアル操作用　プレイヤー、カメラを動かす、止める。
 * 
 * 作成者：守屋   作成日：17/06/21
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorialControl : MonoBehaviour 
{
	/*==所持コンポーネント==*/

    /*==外部設定変数==*/

    /*==内部設定変数==*/
    private PlayerManager m_PlayerManager;
    private CameraMove m_CameraMove;
    private ArmManager m_ArmManager;
    private TutorialSetting m_TutorialSetting;

    //private bool iscam = true;

    /*==外部参照変数==*/

	void Start() 
	{
        m_PlayerManager = GetComponent<PlayerManager>();
        m_CameraMove = GameObject.Find("CameraMove").GetComponent<CameraMove>();
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        m_TutorialSetting = GetComponent<TutorialSetting>();

	}
	
	void Update ()
	{
        //if (Input.GetKeyDown(KeyCode.Z))
        //    SetIsPlayerMove(false);
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    SetIsPlayerMove(iscam);
        //    SetIsCamerMove(iscam);
        //    iscam = !iscam;
        //}
           
        //if (Input.GetKeyDown(KeyCode.C))
        //    SetIsArmUIVisible(false);
        //if (Input.GetKeyDown(KeyCode.V))
        //    SetIsPlayerAndCameraMove(false);
	}

    /// <summary>
    /// プレイヤーの操作制限(false)／解除(true)を行う
    /// </summary>
    public void SetIsPlayerMove(bool value)
    {
        m_PlayerManager.IsMove = value;
    }
    /// <summary>
    /// カメラの操作制限(false)／解除(true)を行う
    /// </summary>
    public void SetIsCamerMove(bool value)
    {
        m_CameraMove.IsMove = value;
    }
    /// <summary>
    /// プレイヤーとカメラの操作制限(false)／解除(true)を行う
    /// </summary>
    public void SetIsPlayerAndCameraMove(bool value)
    {
        SetIsPlayerMove(value);
        SetIsCamerMove(value);
    }
    /// <summary>
    /// アームUIの非表示(false)／表示(true)を行う
    /// </summary>
    public void SetIsArmUIVisible(bool value)
    {
        m_ArmManager.SetUIVisible(value);
    }

    public void SetAll(bool value)
    {
        SetIsPlayerMove(value);
        SetIsCamerMove(value);
        SetIsArmUIVisible(value);
    }



    /// <summary>
    /// アームの使用を制限(false)／解除(true)状況を取得
    /// </summary>
    public bool GetIsActiveArm(int armID)
    {
        return m_TutorialSetting.GetIsActiveArm(armID);
    }


    /// <summary>
    /// アームの使用を制限(false)／解除(true)する
    /// trueを入れると畳まれていたアームを展開します
    /// </summary>
    public void SetIsActiveArm(int armID, bool value)
    {
        m_TutorialSetting.SetIsActiveArm(armID, value);
    }

    /// <summary>
    /// PlayerCameraオブジェクトのトランスフォーム取得
    /// カメラを動かして何かしたい場合はここからお願いします
    /// </summary>
    public Transform GetPlayerCameraTr()
    {
        return m_CameraMove.GetPlayerCamera();
    }



    /// <summary>
    /// アームの操作制限(false)／解除(true)を行う
    /// </summary>
    public void SetIsArmMove(bool value)
    {
        m_ArmManager.IsMove = value;
    }
    /// <summary>
    /// アームのキャッチ操作の操作制限(false)／解除(true)を行う
    /// 大元であるアームの操作制限がかかっているとtrueにしても意味がありません
    /// </summary>
    public void SetIsArmCatchAble(bool value)
    {
        m_ArmManager.IsCatchAble = value;
    }
    /// <summary>
    /// アームの離し操作の操作制限(false)／解除(true)を行う
    /// 大元であるアームの操作制限がかかっているとtrueにしても意味がありません
    /// </summary>
    public void SetIsArmRelease(bool value)
    {
        m_ArmManager.IsRelease = value;
    }
    /// <summary>
    /// 特定のオブジェクトをエイムアシストの対象にしているかを名前で調べる
    /// </summary>
    public bool GetIsAimAssistName(string aimAssistPosGameObjectName)
    {
        return m_ArmManager.GetEnablArmMove().GetAimAssistName() == aimAssistPosGameObjectName;
    }

    /// <summary>
    /// 何らかのオブジェクトをエイムアシストの対象にしているかを取得する
    /// </summary>
    public bool GetIsAimAssistAll()
    {
        return m_ArmManager.GetEnablArmMove().GetIsSearched();
    }
}
