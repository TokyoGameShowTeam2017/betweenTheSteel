using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextIvent : MonoBehaviour
{
    public enum EventController
    {
        NO_BUTTON,
        RT,
        RB,
        //LT,
        //LB,
        L_STICK,
        R_STICK,
        L_STICK_TRIGGER,
        R_STICK_TRIGGER,
        ARM_BUTTON,
        A,
        B,
        X,
        Y
    }
    [SerializeField, Tooltip("声")]
    public AudioClip[] m_Voice;
    [SerializeField, Tooltip("流すテキスト"), TextArea(1, 20)]
    public string[] m_Text;
    [SerializeField, Tooltip("表示させるコントローラーのボタン")]
    public EventController m_ControllerButton;
    [SerializeField, Tooltip("イベントプレハブ")]
    public GameObject m_IventPrefab;


    [SerializeField, Tooltip("表示させたいPointオブジェクト"), Space(15)]
    public GameObject m_DrawPointObject;
    [SerializeField, Tooltip("表示を消したいPointオブジェクト")]
    public GameObject m_NoDrawPointObject;

    [SerializeField, Tooltip("プレイヤー移動させるか"), Space(15), HeaderAttribute("プレイヤーが当たった時の状態")]
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

    [SerializeField, Tooltip("プレイヤーアーム1"), HeaderAttribute("プレイヤーが当たった時どのアームを展開するか")]
    public bool m_PlayerArmEnable1;
    [SerializeField, Tooltip("プレイヤーアーム2")]
    public bool m_PlayerArmEnable2;
    [SerializeField, Tooltip("プレイヤーアーム3")]
    public bool m_PlayerArmEnable3;
    [SerializeField, Tooltip("プレイヤーアーム3")]
    public bool m_PlayerArmEnable4;

    public GameObject mSwitch;

    private GameObject mPointObject;
    //プレイヤーテキスト
    private TutorealText mTutorealText;
    //プレイヤーのチュートリアル
    private PlayerTutorialControl mPlayerTurorial;
    //ダウンロードバー
    private TutorealArmSetGaugeUi mArmSetBar;
    [SerializeField, Tooltip("当たるかどうか"), Space(15)]
    public bool m_IsCollision;
    //声の名前
    private List<string> mVoiceName;



    // Use this for initialization
    void Start()
    {
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mPlayerTurorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mArmSetBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<TutorealArmSetGaugeUi>();
        mVoiceName = new List<string>();
        foreach (var i in m_Voice)
        {
            mVoiceName.Add(i.name);
        }

    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && m_IsCollision)
        {
            if (m_DrawPointObject != null) m_DrawPointObject.SetActive(true);
            if (m_NoDrawPointObject != null) m_NoDrawPointObject.SetActive(false);
            if (m_Text.Length > 0)
                mTutorealText.SetText(m_Text,mVoiceName);

            if (m_PlayerArmEnable1 || m_PlayerArmEnable2||
                m_PlayerArmEnable3 || m_PlayerArmEnable4)
            {
                mArmSetBar.IsLoading(m_PlayerArmEnable1,m_PlayerArmEnable2,m_PlayerArmEnable3,m_PlayerArmEnable4);
            }

            mPlayerTurorial.SetIsArmMove(!m_PlayerArmMove);
            mPlayerTurorial.SetIsPlayerMove(!m_PlayerMove);
            mPlayerTurorial.SetIsCamerMove(!m_PlayerCameraMove);
            mPlayerTurorial.SetIsArmCatchAble(!m_PlayerArmCath);
            mPlayerTurorial.SetIsArmRelease(!m_PlayerArmNoCath);

            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetController(m_ControllerButton);

            if (m_IventPrefab != null)
                m_IventPrefab.GetComponent<TutorealIventFlag>().PlayIvent();

            if (mSwitch != null) mSwitch.GetComponent<TutorealIventSwitch>().IsCollision(true);
            Destroy(gameObject);
        }
    }
    public void IsCollisionFlag(bool flag = true)
    {
        m_IsCollision = flag;
    }

    public GameObject GetIvent()
    {
        return m_IventPrefab;
    }
}
