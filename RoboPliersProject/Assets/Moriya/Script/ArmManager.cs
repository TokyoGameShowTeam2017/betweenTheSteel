/**==========================================================================*/
/**
 * アームとペンチの管理、制御
 * 作成者：守屋   作成日：17/04/21
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmManager : MonoBehaviour 
{
	/*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField, Tooltip("本体はここへ登録")]
    private Transform m_Base;
    [SerializeField, Tooltip("アームはここへ登録")]
    private ArmMove[] m_Arms;
    [SerializeField, Tooltip("ペンチはここへ登録")]
    private PliersMove[] m_Pliers;
    [SerializeField, Tooltip("ペンチの掴む強さをここで設定")]
    private float[] m_PliersPower;
    [SerializeField, Tooltip("アームの角度限界")]
    private float m_ArmAngleMax = 80.0f;
    [SerializeField,Tooltip("現在選択中のアームID(0～4)")]
    private int m_EnableArmID = 0;
    [SerializeField, Tooltip("ペンチのZ軸回転速度")]
    private float m_PliersRollSpeed  = 120.0f;
    [SerializeField, Tooltip("アームの伸び補完値")]
    private float m_StretchLerpValue = 0.2f;

    [SerializeField, Tooltip("ペンチが動かせるオブジェクトを掴むときの、掴んだと見なす値\n(オブジェクトの動きの量がこの値以下なら掴んだとする)")]
    private float m_CatchingThreshold = 1.5f;
    [SerializeField, Tooltip("アーム回転補完値")]
    private float m_RotationLerpValue = 0.2f;
    [SerializeField, Tooltip("アームリセット時、リセットを完了させるまでの秒数")]
    private float m_ResetEndTime = 0.5f;


    /*==内部設定変数==*/
    private PlayerManager m_PlayerManager;
    //現在選択中のアーム本体
    private ArmMove m_EnableArm;
    //現在選択中のペンチ本体
    private PliersMove m_EnablePliers;
    //初期回転量
    private Quaternion m_InitRot;
    //アームが角度制限を超えたときの自身の回転量
    private float m_RotateY = 0.0f;

    //ＵＩ(表示／非表示を行う)
    private Transform m_UI;

    bool aaa = true;

    /*==外部参照変数==*/

    void Awake()
    {
        tr = GetComponent<Transform>();

        m_PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        SwitchEnableArm(m_EnableArmID);

        for (int i = 0; i < m_Arms.Length; i++)
        {
            m_Arms[i].SetID(i);
            m_Pliers[i].SetID(i);
        }
        m_InitRot = m_Base.rotation;
    }

	void Start()
	{
        m_UI = GameObject.Find("Canvas ingame").transform;
	}
	
	void Update()
    {
        if (!m_PlayerManager.IsMove) return;

        //アーム切り替え
        if (InputManager.GetSelectArm().isDown)
        {
            SwitchEnableArm(InputManager.GetSelectArm().id - 1);
            SoundManager.Instance.PlaySe("xg-2armmove");
        }

        //選択中のアームとペンチ更新処理
        m_EnableArm.ArmUpdate();
        m_EnablePliers.PliersUpdate();


        //何らかのアームが動かないオブジェクトを掴んでいる時
        if(GetCountCatchingObjects() > 0)
        {

        }
        //通常時
        else
        {
            ////選択中のアームに合わせて自身を回転
            //Quaternion r = m_Base.localRotation;
            //m_RotateY += m_PlayerManager.GetArmAngleOver();
            //Quaternion target = Quaternion.Euler(new Vector3(0.0f, -90.0f * m_EnableArmID + m_RotateY, 0.0f));
            //m_Base.rotation = Quaternion.Slerp(r, target, 0.1f);


            //選択中のアームに合わせて自身を回転
            Quaternion r = m_Base.rotation;
            if (m_EnableArmID == 3)
                m_RotateY -= m_PlayerManager.GetArmAngleOver();
            else
                m_RotateY += m_PlayerManager.GetArmAngleOver();
            Quaternion target = Quaternion.Euler(new Vector3(0.0f, -90.0f * m_EnableArmID + m_RotateY, 0.0f));
            //m_Base.rotation = target;
            m_Base.rotation = Quaternion.Slerp(r, target, m_RotationLerpValue);
        }



        //選択中のアーム以外の向き、伸び、掴み状態等をリセット
        if (InputManager.GetDash())
        {
            for (int i = 0; i < m_Arms.Length; i++)
            {
                if (i != m_EnableArmID)
                {
                    m_Arms[i].Reset();
                    m_Pliers[i].Reset();
                }
            }
        }

	}


    void LateUpdate()
    {
        //print(GetCountCatchingDynamicObjects());
    }

    /// <summary>
    /// 選択アーム、ペンチを変更する
    /// </summary>
    public void SwitchEnableArm(int id)
    {
        m_EnableArmID = id;
        m_EnableArm = m_Arms[id];
        m_EnablePliers = m_Pliers[id];
    }

    /// <summary>
    /// 選択中のアーム（ペンチ）のIDを返す
    /// </summary>
    public int GetEnablArmID()
    {
        return m_EnableArmID;
    }

    /// <summary>
    /// 選択中のアームを返す
    /// </summary>
    public GameObject GetEnablArm()
    {
        return m_EnableArm.gameObject;
    }

    /// <summary>
    /// 選択中のペンチを返す
    /// </summary>
    public GameObject GetEnablPliers()
    {
        return m_EnablePliers.gameObject;
    }

    /// <summary>
    /// 選択中のアームを返す
    /// </summary>
    public ArmMove GetEnablArmMove()
    {
        return m_EnableArm;
    }

    /// <summary>
    /// 選択中のペンチを返す
    /// </summary>
    public PliersMove GetEnablPliersMove()
    {
        return m_EnablePliers;
    }

    /// <summary>
    /// アームを返す
    /// </summary>
    public GameObject GetArmByID(int id)
    {
        return m_Arms[id].gameObject;
    }

    /// <summary>
    /// ペンチを返す
    /// </summary>
    public GameObject GetPliersByID(int id)
    {
        return m_Pliers[id].gameObject;
    }


    /// <summary>
    /// アームを返す
    /// </summary>
    public ArmMove GetArmMoveByID(int id)
    {
        return m_Arms[id];
    }

    /// <summary>
    /// ペンチを返す
    /// </summary>
    public PliersMove GetPliersMoveByID(int id)
    {
        return m_Pliers[id];
    }


    /// <summary>
    /// 選択中のアーム（ペンチ）がオブジェクトを掴んでいるかを返す
    /// </summary>
    public bool GetIsEnablArmCatching()
    {
        return m_EnablePliers.GetIsCatch();
    }

    /// <summary>
    /// 選択中のアーム（ペンチ）が掴んでいるオブジェクトを返す
    /// 掴んでないならnull
    /// </summary>
    public CatchObject GetEnablArmCatchingObject()
    {
        return m_EnablePliers.GetCatchObject();
    }

    /// <summary>
    /// 指定したIDのアーム（ペンチ）がオブジェクトを掴んでいるかを返す
    /// </summary>
    public bool GetIsArmCatching(int id)
    {
        return m_Pliers[id].GetIsCatch();
    }

    /// <summary>
    /// 指定したIDのアーム（ペンチ）が掴んでいるオブジェクトを返す
    /// 掴んでないならnull
    /// </summary>
    public CatchObject GetArmCatchingObject(int id)
    {
        return m_Pliers[id].GetCatchObject();
    }

    /// <summary>
    /// 指定したID以外のアームのうち、動かないオブジェクトを掴んでいるアームを返す(idの若いものが優先)
    /// どのアームも掴んでない場合はnullを返す
    /// </summary>
    public PliersMove GetCatchingArm(int id)
    {
        for (int i = 0; i < m_Pliers.Length; i++)
        {
            if(i != id && m_Pliers[i].GetIsCatch())
               if( m_Pliers[i].GetCatchObject().GetCatchType() == CatchObject.CatchType.Static)
                    return m_Pliers[i];
        }
        return null;
    }

    /// <summary>
    /// すべてのペンチが掴んでいる、動かないオブジェクトの数を返す
    /// </summary>
    public int GetCountCatchingObjects()
    {
        int count = 0;

        for (int i = 0; i < m_Pliers.Length;i++ )
        {
            CatchObject obj = m_Pliers[i].GetCatchObject();
            if (obj != null)
                if (obj.GetCatchType() == CatchObject.CatchType.Static)
                    count++;
        }
        return count;
    }

    /// <summary>
    /// すべてのペンチが掴んでいる、動くオブジェクトの数を返す
    /// </summary>
    public int GetCountCatchingDynamicObjects()
    {
        int count = 0;

        for (int i = 0; i < m_Pliers.Length; i++)
        {
            CatchObject obj = m_Pliers[i].GetCatchObject();
            if (obj != null)
                if (obj.GetCatchType() == CatchObject.CatchType.Dynamic)
                    count++;
        }
        return count;
    }

    /// <summary>
    /// 選択中のアームのエイムアシストの座標を更新
    /// </summary>
    public void SetAimAssistPosition(Vector3 pos)
    {
        m_EnableArm.SetAimAssistPosition(pos);
    }

    /// <summary>
    /// アームの左右の角度限界を返す
    /// </summary>
    public float GetArmAngleMax()
    {
        return m_ArmAngleMax;
    }

    /// <summary>
    /// ペンチの掴む強さを取得する
    /// </summary>
    public float GetPliersPower(int id)
    {
        return m_PliersPower[id];
    }


    /// <summary>
    /// ペンチのZ軸回転速度を取得する
    /// </summary>
    public float GetPliersRollSpeed()
    {
        return m_PliersRollSpeed;
    }

    /// <summary>
    /// 現在選択中のアームのペンチのZ軸回転量を返す(ペンチの回転入力がない場合には0、入力がある場合には回転に使用した回転量を返す)
    /// </summary>
    public float GetEnablePliersRollValue()
    {
        return m_EnablePliers.GetRollValue();
    }

    public float GetStretchLerpValue()
    {
        return m_StretchLerpValue;
    }

    public float GetCatchingThreshold()
    {
        return m_CatchingThreshold;
    }

    /// <summary>
    /// 現在選択中のアームのペンチが掴んでいるロッドオブジェクトを取得
    /// </summary>
    public GameObject GetEnablePliersCatchRod()
    {
        if (GetEnablArmCatchingObject() == null) return null;
        if (GetEnablArmCatchingObject().gameObject.GetComponent<RodTurnBone>() == null) return null;
        return GetEnablArmCatchingObject().gameObject.GetComponent<RodTurnBone>().GetRod();
    }
    /// <summary>
    /// 指定したのアームのペンチが掴んでいるロッドオブジェクトを取得
    /// </summary>
    public GameObject GetPliersCatchRod(int id)
    {
        if (m_Pliers[id].GetCatchObject() == null) return null;
        GameObject a = m_Pliers[id].GetCatchObject().gameObject.GetComponent<RodTurnBone>().GetRod();
        return m_Pliers[id].GetCatchObject().gameObject.GetComponent<RodTurnBone>().GetRod();
    }
    /// <summary>
    /// 指定したIDのペンチを返す
    /// </summary>
    public GameObject GetPliers(int id)
    {
        return m_Pliers[id].gameObject;
    }

    /// <summary>
    /// 指定したIDのペンチが最も最近に手放したオブジェクトを返す
    /// </summary>
    public GameObject GetPliersReleasedObject(int id)
    {
        return m_Pliers[id].GetReleasedObject();
    }

    /// <summary>
    /// 現在選択中のペンチが最も最近に手放したオブジェクトを返す
    /// </summary>
    public GameObject GetEnablePliersReleasedObject()
    {
        return m_EnablePliers.GetReleasedObject();
    }
    /// <summary>
    /// 現在選択中のペンチが最も最近に手放したロッドオブジェクトを返す
    /// </summary>
    public GameObject GetEnablePliersReleasedRod()
    {
        if (GetEnablePliersReleasedObject() == null) return null;
        return GetEnablePliersReleasedObject().gameObject.GetComponent<RodTurnBone>().GetRod();
    }

    /// <summary>
    /// アームの伸びた量を返す
    /// </summary>
    public float GetArmLength(int id)
    {
        return m_Pliers[id].GetArmLength();
    }

    /// <summary>
    /// アームの伸びた量を返す
    /// </summary>
    public float GetEnableArmLength(int id)
    {
        return m_EnablePliers.GetArmLength();
    }

    public float GetResetEndTime()
    {
        return m_ResetEndTime;
    }


    /// <summary>
    /// UIの表示(true)／非表示(false)を設定する
    /// </summary>
    public void SetUIVisible(bool value)
    {
        RunChildren(m_UI, value);
    }

    //全ての子の走査用関数
    private void RunChildren(Transform trans, bool value)
    {
        //RawImageがあるならアクティブをセットする
        RawImage r = trans.GetComponent<RawImage>();
        if (r != null)
            r.enabled = value;

        //子を走査
        Transform children = trans.GetComponentInChildren<Transform>();
        if (children.childCount == 0)
            return;

        foreach (Transform t in children)
        {
            RunChildren(t, value);
        }
    }

    public struct HookState
    {
        public float armMoveY;
        public bool playerIsGround;
    }

    /// <summary>
    /// ひっかけの時に使う
    /// アームを操作する縦方向入力と、プレイヤーが地面に接しているかを同時に取得する
    /// </summary>
    public HookState GetArmInputYAndIsGround()
    {
        HookState result;
        PlayerMove p = m_PlayerManager.GetPlayerMove();
        result.armMoveY = p.ArmInputY;
        result.playerIsGround = p.IsGround;
        return result;
    }
}
