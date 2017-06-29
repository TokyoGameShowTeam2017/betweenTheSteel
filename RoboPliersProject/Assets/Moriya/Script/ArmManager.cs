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
    [SerializeField, Tooltip("Static掴み時のアームの角度限界")]
    private float m_StaticCatchArmAngleMax = 85.0f;

    [SerializeField, Tooltip("現在選択中のアームID(0～4)")]
    private int m_EnableArmID = 0;
    [SerializeField, Tooltip("ペンチのZ軸回転速度")]
    private float m_PliersRollSpeed = 120.0f;
    [SerializeField, Tooltip("アームの伸び補完値")]
    private float m_StretchLerpValue = 0.2f;

    [SerializeField, Tooltip("ペンチが動かせるオブジェクトを掴むときの、掴んだと見なす値\n(オブジェクトの動きの量がこの値以下なら掴んだとする)")]
    private float m_CatchingThreshold = 1.5f;
    [SerializeField, Tooltip("アーム回転補完値")]
    private float m_RotationLerpValue = 0.2f;
    [SerializeField, Tooltip("アームリセット時、リセットを完了させるまでの秒数")]
    private float m_ResetEndTime = 0.5f;

    public GameObject CutParticlePrefab;
    public GameObject WarpPartielcPrefab;

    //UI移動終了にかける時間
    public float m_UIStartToEndTime = 1.0f;
    //ＵＩの移動始点
    public Vector3 m_LeftUIStart;
    public Vector3 m_RightUIStart;
    public Vector3 m_UpUIStart;
    public Vector3 m_DownUIStart;

    //ＵＩの移動終点
    public Vector3 m_LeftUIEnd;
    public Vector3 m_RightUIEnd;
    public Vector3 m_UpUIEnd;
    public Vector3 m_DownUIEnd;

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
    private RectTransform m_UI;
    private RectTransform m_LeftUI;
    private RectTransform m_RightUI;
    private RectTransform m_DownUI;
    private RectTransform m_UpUI;
    private RectTransform m_GaugeUI;
    //アームのUI
    private RectTransform[] m_GaugeUIs;
    private RectTransform[] m_ButtonUIs;


    private TutorialSetting m_TutorialSetting;

    private CameraMove m_CameraMove;

    //シーン切り替え時に消すオブジェクト用のゴミ箱
    private Transform m_DustBox;

    /// <summary>
    /// 動かせるか？
    /// trueで動かせるようになり、エイムアシスト等が働く。カメラも動かせる状態ならば、カメラの向いたほうを向く。
    /// falseで動かなくなる。
    /// </summary>
    public bool IsMove { get; set; }
    //掴めるか？
    public bool IsCatchAble { get; set; }
    //物を離せるか？
    public bool IsRelease { get; set; }
    //伸び縮みできるか？
    public bool IsStretch { get; set; }
    //アームを選択できるか？(アームのidごとに指定)(0～3)
    public bool[] IsArmSelectAble { get; set; }
    //アームリセットできるか？
    public bool IsResetAble { get; set; }

    //Static掴み時のアームの角度限界
    public float StaticCatchingArmAngleMax { get; set; }


    /*==外部参照変数==*/

    void Awake()
    {
        //UI用のオブジェクト取得
        UIGet();

        tr = GetComponent<Transform>();
        m_PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        m_CameraMove = GameObject.Find("CameraMove").GetComponent<CameraMove>();
        m_DustBox = GameObject.FindGameObjectWithTag("DustBox").transform;

        m_TutorialSetting = m_PlayerManager.GetComponent<TutorialSetting>();
        SwitchEnableArm(m_EnableArmID);

        GetComponent<LineRenderer>().startColor = new Color(1, 0, 0, 0);
        GetComponent<LineRenderer>().endColor = new Color(1, 0, 0, 0);

        for (int i = 0; i < m_Arms.Length; i++)
        {
            m_Arms[i].SetID(i);
            m_Pliers[i].SetID(i);
        }
        m_InitRot = m_Base.rotation;

        //エラー回避のため設定
        m_EnableArmID = 0;
        m_EnableArm = m_Arms[0];
        m_EnablePliers = m_Pliers[0];

        IsMove = true;
        IsCatchAble = true;
        IsRelease = true;
        IsStretch = true;
        IsArmSelectAble = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            IsArmSelectAble[i] = true;
        }
        IsResetAble = true;
        StaticCatchingArmAngleMax = m_StaticCatchArmAngleMax;
    }

    void Start()
    {
        //UI移動開始
        StartUIMove();

        //アクティブではないアームのＵＩを変更
        for (int i = 0; i < 4; i++)
        {
            if (!m_TutorialSetting.GetIsActiveArm(i))
            {
                SetGaugeUINoActive(i);
            }
        }
    }

    void Update()
    {
        if (!IsMove)
        {
            GetEnablPliersMove().RollValueReset();
            return;
        }

        //あとで直す
        SceneChange();

        //アーム切り替え
        if (InputManager.GetSelectArm().isDown)
        {
            int prev = m_EnableArmID;
            int armid = InputManager.GetSelectArm().id - 1;
            if (IsArmSelectAble[armid])
            {
                SwitchEnableArm(armid);
                SoundManager.Instance.PlaySe("xg-2armmove");

                ////何も掴んでなければ正面を向く
                //if (GetCountCatchingDynamicObjects() <= 0 && GetCountCatchingObjects() <= 0)
                //    m_RotateY = GameObject.Find("PlayerCamera").transform.eulerAngles.y;
                //else if (GetCountCatchingObjects() >= 1)
                //{
                //    StartCoroutine(CameraRotationCalc(armid, prev));
                //}
            }
        }

        //選択中のアームとペンチ更新処理
        m_EnableArm.ArmUpdate();
        m_EnablePliers.PliersUpdate();


        //何らかのアームが動かないオブジェクトを掴んでいる時
        if (GetCountCatchingObjects() > 0)
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
            //if (m_EnableArmID == 3)
            //{
            //    m_RotateY -= m_PlayerManager.GetArmAngleOver();

            //}
            //else
            m_RotateY += m_PlayerManager.GetArmAngleOver();

            Quaternion target = Quaternion.Euler(new Vector3(0.0f, -90.0f * m_EnableArmID + m_RotateY, 0.0f));
            //m_Base.rotation = target;
            m_Base.rotation = Quaternion.Slerp(r, target, m_RotationLerpValue);
        }



        //選択中のアーム以外の向き、伸び、掴み状態等をリセット
        if (InputManager.GetDash() && IsResetAble)
        {
            //アームとペンチのリセット
            ResetOther(m_EnableArmID);
            //ベースを正面に向ける
            m_RotateY = GameObject.Find("PlayerCamera").transform.eulerAngles.y;
        }
    }


    void LateUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    ResetAll();
        //    DustBoxClear();
        //}


        //if (InputManager.GetSelectArm().isDown)
        //{
        //    print("ok");

        //}

        //print(GetCountCatchingDynamicObjects());
    }

    /// <summary>
    /// 選択アーム、ペンチを変更する
    /// </summary>
    public void SwitchEnableArm(int id)
    {
        if (m_TutorialSetting.GetIsActiveArm(id))
        {
            m_EnableArmID = id;
            m_EnableArm = m_Arms[id];
            m_EnablePliers = m_Pliers[id];

            //ボタンＵＩの色変え
            SelectingUICalc();
        }
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
            if (i != id && m_Pliers[i].GetIsCatch())
                if (m_Pliers[i].GetCatchObject().GetCatchType() == CatchObject.CatchType.Static)
                    return m_Pliers[i];
        }
        return null;
    }

    /// <summary>
    /// 動かないオブジェクトを掴んでいるアームのIDを返す
    /// どのアームも掴んでない場合は-1を返す
    /// </summary>
    public int GetCatchingArmID()
    {
        for (int i = 0; i < m_Pliers.Length; i++)
        {
            if (m_Pliers[i].GetIsCatch())
                if (m_Pliers[i].GetCatchObject().GetCatchType() == CatchObject.CatchType.Static)
                    return i;
        }
        return -1;
    }

    /// <summary>
    /// 何らかのオブジェクトを掴んでいるアームのIDを返す
    /// どのアームも掴んでない場合は-1を返す
    /// </summary>
    public int GetIsCatchArmID()
    {
        for (int i = 0; i < m_Pliers.Length; i++)
        {
            if (m_Pliers[i].GetIsCatch())
                return i;
        }
        return -1;
    }

    /// <summary>
    /// すべてのペンチが掴んでいる、動かないオブジェクトの数を返す
    /// </summary>
    public int GetCountCatchingObjects()
    {
        int count = 0;

        for (int i = 0; i < m_Pliers.Length; i++)
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
        if (m_UI != null) 
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


    private string NameByID(int id)
    {
        string result = "";
        switch (id)
        {
            case 0: result = "Y"; break;
            case 1: result = "B"; break;
            case 2: result = "A"; break;
            case 3: result = "X"; break;
            default: break;
        }
        return result;
    }



    /// <summary>
    /// ゲージのＵＩを非アクティブに設定する
    /// </summary>
    public void SetGaugeUINoActive(int id)
    {
        string name = NameByID(id);
        m_GaugeUIs[id].GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
        m_GaugeUIs[id].FindChild(name + "gaugearrow").GetComponent<RawImage>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        m_GaugeUIs[id].FindChild(name + "gauge2").GetComponent<RawImage>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        m_ButtonUIs[id].GetComponent<RawImage>().color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
    }
    /// <summary>
    /// ゲージのＵＩをアクティブに設定する
    /// </summary>
    public void SetGaugeUIActive(int id)
    {
        string name = NameByID(id);
        Color armcolor = Color.white;
        switch (id)
        {
            //後から色変更できるように直打ち
            case 0: armcolor = new Color(1.0f, 1.0f, 0.0f, 1.0f); break;
            case 1: armcolor = new Color(1.0f, 0.0f, 0.0f, 1.0f); break;
            case 2: armcolor = new Color(0.0f, 1.0f, 0.0f, 1.0f); break;
            case 3: armcolor = new Color(0.0f, 0.0f, 1.0f, 1.0f); break;
            default: break;
        }
        m_GaugeUIs[id].GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
        m_GaugeUIs[id].FindChild(name + "gaugearrow").GetComponent<RawImage>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        m_ButtonUIs[id].GetComponent<RawImage>().color = armcolor;
        if(id != m_EnableArmID)
            armcolor = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        m_GaugeUIs[id].FindChild(name + "gauge2").GetComponent<RawImage>().color = armcolor;
    }
        
    /// <summary>
    /// 選択中アームのみを色付けするＵＩ計算処理
    /// </summary>
    public void SelectingUICalc()
    {
        Color color = Color.white;
        for (int i = 0; i < m_GaugeUIs.Length; i++)
        {
            string name = NameByID(i);
            if (i != m_EnableArmID)
                color = new Color(0, 0, 0, 0.2f);
            else
            {
                switch (i)
                {
                    case 0: color = new Color(1.0f, 1.0f, 0.0f, 1.0f); break;
                    case 1: color = new Color(1.0f, 0.0f, 0.0f, 1.0f); break;
                    case 2: color = new Color(0.0f, 1.0f, 0.0f, 1.0f); break;
                    case 3: color = new Color(0.0f, 0.0f, 1.0f, 1.0f); break;
                    default: break;
                }
            }

            m_GaugeUIs[i].FindChild(name + "gauge2").GetComponent<RawImage>().color = color;
        }            
    }

    public struct HookState
    {
        public float armInputY;
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
        result.armInputY = p.ArmInputY;
        result.playerIsGround = p.IsGround;
        return result;
    }


    //UI取得処理
    public void UIGet()
    {
        if (GameObject.Find("Canvas ingame") == null) return;
        m_UI = GameObject.Find("Canvas ingame").GetComponent<RectTransform>();

        m_GaugeUIs = new RectTransform[4];
        m_ButtonUIs = new RectTransform[4];

        m_LeftUI = m_UI.FindChild("left").GetComponent<RectTransform>();
        m_RightUI = m_UI.FindChild("right").GetComponent<RectTransform>();
        m_UpUI = m_UI.FindChild("up").GetComponent<RectTransform>();
        m_DownUI = m_UI.FindChild("down").GetComponent<RectTransform>();
        m_GaugeUI = m_LeftUI.FindChild("gauge").GetComponent<RectTransform>();

        string name = "";
        for (int i = 0; i < 4; i++)
        {
            name = NameByID(i);
            m_GaugeUIs[i] = m_GaugeUI.FindChild(name + "gauge1").GetComponent<RectTransform>();
            m_ButtonUIs[i] = m_GaugeUI.Find(name).GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// シーンが変わった直後に呼ぶ処理
    /// </summary>
    public void SceneChange()
    {
        //UI用のオブジェクト取得
        UIGet();


        for (int i = 0; i < 4; i++)
        {
            m_Pliers[i].SceneChange();
        }

    }


    //指定したＩＤのアーム、ペンチ以外をリセットする
    public void ResetOther(int id)
    {
        //アームとペンチのリセット
        for (int i = 0; i < m_Arms.Length; i++)
        {
            if (i != id)
            {
                m_Arms[i].Reset();
                m_Pliers[i].Reset();
            }
        }
    }

    //すべてのアームペンチリセット
    public void ResetAll()
    {
        //アームとペンチのリセット
        for (int i = 0; i < m_Arms.Length; i++)
        {
            if (i == m_EnableArmID)
            {
                m_Pliers[i].ForceCatchRelease();
            }
            else
            {
                m_Arms[i].Reset();
                m_Pliers[i].Reset();
            }
        }
    }

    /// <summary>
    /// Static掴み中のカメラ回転処理
    /// </summary>
    private IEnumerator CameraRotationCalc(int currid,int previd)
    {
        float maxtime = 0.5f;
        int i = currid - previd;    //idの差
        float maxrot = 0.0f;        //総回転量
        float rotation = 0.0f;      //回転量

        //maxrot = GetArmByID(currid).transform.eulerAngles.y - GetArmByID(previd).transform.eulerAngles.y;
        maxrot = 90.0f * currid - GetArmByID(previd).transform.localEulerAngles.y;
        //print(maxrot);

        //maxrot = GetArmByID(currid).transform.eulerAngles.y - 90.0f * previd;
        //maxrot = 90.0f * currid - 90.0f * previd;

        //rot *= 1.5f;

        //if (Mathf.Abs(i) == 2)
        //    rot = 180.0f;
        //else if ((previd == 0 && currid == 1) || (previd == 1 && currid == 2) || (previd == 2 && currid == 3) || (previd == 3 && currid == 0))
        //    rot = 90.0f;
        //else if ((previd == 0 && currid == 3) || (previd == 1 && currid == 0) || (previd == 2 && currid == 1) || (previd == 3 && currid == 2))
        //    rot = -90.0f;

        float r = maxrot * Time.deltaTime / maxtime;
        while (Mathf.Abs( rotation) < Mathf.Abs( maxrot))
        {
            //timer += Time.deltaTime;
            m_CameraMove.Rotation(r, 0);
            rotation += r;
            yield return null;
        }
        yield break;
    }

    public void DustBoxAddChild(Transform addObject)
    {
        addObject.parent = m_DustBox;
    }

    public void DustBoxAddChild(GameObject addObject)
    {
        addObject.transform.parent = m_DustBox;
    }

    public void DustBoxClear()
    {
        foreach (Transform t in m_DustBox)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    /// <summary>
    /// シーン切り替え時に呼ぶ処理
    /// 持ってるオブジェクトを削除、前までDontDestroyに入っていたものを削除
    /// </summary>
    public void SceneChangeCalc()
    {
        ResetAll();
        DustBoxClear();
    }

    /// <summary>
    /// 画面外からUIが出てくるコルーチン開始
    /// </summary>
    /// <returns></returns>
    public void StartUIMove()
    {
        StartCoroutine(StartToEndUIMove());
    }
    /// <summary>
    /// 画面内からUIが出ていくコルーチン開始
    /// </summary>
    /// <returns></returns>
    public void EndUIMove()
    {
        StartCoroutine(EndToStartUIMove());
    }


    IEnumerator StartToEndUIMove()
    {
        float timer = 0.0f;
        float max = m_UIStartToEndTime / 2;
        m_LeftUI.localPosition = m_LeftUIStart;
        m_RightUI.localPosition = m_RightUIStart;
        m_UpUI.localPosition = m_UpUIStart;
        m_DownUI.localPosition = m_DownUIStart;

        while (timer < max)
        {
            //先に左右
            m_LeftUI.localPosition = Vector3.Lerp(m_LeftUIStart, m_LeftUIEnd, timer / max);
            m_RightUI.localPosition = Vector3.Lerp(m_RightUIStart, m_RightUIEnd, timer / max);


            timer += Time.deltaTime;                
            yield return null;
        }
        timer = 0.0f;
        while (timer < max)
        {
            m_UpUI.localPosition = Vector3.Lerp(m_UpUIStart, m_UpUIEnd, timer / max);
            m_DownUI.localPosition = Vector3.Lerp(m_DownUIStart, m_DownUIEnd, timer / max);
            timer += Time.deltaTime;
            yield return null;
        }
        yield break;
    }


    IEnumerator EndToStartUIMove()
    {
        float timer = 0.0f;
        float max = m_UIStartToEndTime / 2;
        m_LeftUI.localPosition = m_LeftUIEnd;
        m_RightUI.localPosition = m_RightUIEnd;
        m_UpUI.localPosition = m_UpUIEnd;
        m_DownUI.localPosition = m_DownUIEnd;


        while (timer < max)
        {
            //先に上下
            m_UpUI.localPosition = Vector3.Lerp(m_UpUIEnd, m_UpUIStart, timer / max);
            m_DownUI.localPosition = Vector3.Lerp(m_DownUIEnd, m_DownUIStart, timer / max);

            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0.0f;
        while (timer < max)
        {
            m_LeftUI.localPosition = Vector3.Lerp(m_LeftUIEnd, m_LeftUIStart, timer / max);
            m_RightUI.localPosition = Vector3.Lerp(m_RightUIEnd, m_RightUIStart, timer / max);
            timer += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
