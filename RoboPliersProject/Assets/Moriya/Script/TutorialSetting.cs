/**==========================================================================*/
/**
 * チュートリアルの設定はここを通して行う
 * 作成者：守屋   作成日：17/06/16
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSetting : MonoBehaviour 
{
	/*==所持コンポーネント==*/

    /*==外部設定変数==*/
    [SerializeField, Tooltip("プレイヤーをチュートリアルモードで実行するか？")]
    private bool m_IsTutorial = true;
    [SerializeField, Tooltip("アームの制限(false)／解除(true)状況")]
    private bool[] m_IsActiveArms = { true, false, false, false };

    [SerializeField, Tooltip("アーム、ペンチを展開し終える秒数")]
    private float m_ArmActivateTime = 4.0f;


    /*==内部設定変数==*/
    private ArmManager m_ArmManager;

    /*==外部参照変数==*/

	void Start() 
	{
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        Initialize();
	}
	
	void Update ()
	{
		
	}


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        if (!m_IsTutorial) return;

        
        for (int i = 0; i < 4; i++)
        {
            //アームをたたむ
            Transform pliers = m_ArmManager.GetPliersByID(i).transform;
            Transform arm3 = pliers.parent;
            arm3.localEulerAngles = new Vector3(-180, 0, 0);
            //ペンチを閉じる
            Transform j = pliers.FindChild("5.!joint_bone7");
            Vector3 pos = j.FindChild("PlierRight").localPosition;
            pos.x = 0;
            j.FindChild("PlierRight").localPosition = pos;
            pos = j.FindChild("PlierLeft").localPosition;
            pos.x = 0;
            j.FindChild("PlierLeft").localPosition = pos;


        }
        for (int i = 0; i < 4; i++)
        {
            if(GetIsActiveArm(i))
                StartArmActivateCor(i);
        }

         
        //最初のアームだけ展開 
        //StartFirstArmActivateCor();
    }

    public bool GetIsTutorial()
    {
        return m_IsTutorial;
    }

    /// <summary>
    /// アームの使用を制限(false)／解除(true)状況を取得
    /// </summary>
    public bool GetIsActiveArm(int armID)
    {
        return m_IsActiveArms[armID];
    }


    /// <summary>
    /// アームの使用を制限(false)／解除(true)する
    /// </summary>
    public void SetIsActiveArm(int armID, bool value)
    {
        if (!m_IsActiveArms[armID] && value)
            StartArmActivateCor(armID);

        m_IsActiveArms[armID] = value;
    }

    /// <summary>
    /// 最初に使うアームの展開処理
    /// </summary>
    public void StartFirstArmActivateCor()
    {
        StartCoroutine(ArmActivate(0));
    }



    /// <summary>
    /// アームとペンチを展開するコルーチンを開始
    /// </summary>
    public void StartArmActivateCor(int armID)
    {
        StartCoroutine(ArmActivate(armID));
    }

    //アームとペンチを展開する
    IEnumerator ArmActivate(int armID)
    {
        //ペンチベース
        Transform pliers = m_ArmManager.GetPliersByID(armID).transform;
        //回転させるアーム
        Transform arm3 = pliers.parent;

        Transform j = pliers.FindChild("5.!joint_bone7");
        Transform pliersR = j.FindChild("PlierRight");
        Transform pliersL = j.FindChild("PlierLeft");

        Vector3 posR = pliersR.localPosition;
        Vector3 posL = pliersL.localPosition;

        Vector3 armangle = arm3.localEulerAngles;

        float timer = 0.0f;
        float movespeed = 0.0f;

        while (timer < m_ArmActivateTime)
        {
            timer += Time.deltaTime;
            //アーム、ペンチを展開
            armangle.x -= 180.0f / m_ArmActivateTime * Time.deltaTime;
            arm3.localEulerAngles = armangle;

            movespeed = 0.55f / m_ArmActivateTime * Time.deltaTime;
            posR.x += movespeed;
            posL.x -= movespeed;
            pliersR.localPosition = posR;
            pliersL.localPosition = posL;
            yield return null;
        }
        
        //値を調整
        armangle.x = 180.0f;
        posR.x = 0.55f;
        posL.x = -0.55f;
        arm3.localEulerAngles = armangle;
        pliersR.localPosition = posR;
        pliersL.localPosition = posL;

        //制限を解除
        m_IsActiveArms[armID] = true;
        yield break;
    }

    [ContextMenu("Activate Arm 1")]
    private void Activate1()
    {
        StartArmActivateCor(1);
    }

    [ContextMenu("Activate Arm 2")]
    private void Activate2()
    {
        StartArmActivateCor(2);
    }

    [ContextMenu("Activate Arm 3")]
    private void Activate3()
    {
        StartArmActivateCor(3);
    }
}
