using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextIvent : MonoBehaviour
{
    [SerializeField, Tooltip("声")]
    public AudioClip m_Voice;
    [SerializeField, Tooltip("流すテキスト"), TextArea(1, 20)]
    public string[] m_Text;
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
    private GameObject mPointObject;
    //プレイヤーテキスト
    private TutorealText mTutorealText;
    //プレイヤーのチュートリアル
    private PlayerTutorialControl mPlayerTurorial;
    //ダウンロードバー
    private TutorealArmSetGaugeUi mArmSetBar;
    [SerializeField, Tooltip("当たるかどうか"), Space(15)]
    public bool m_IsCollision;



    // Use this for initialization
    void Start()
    {
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mPlayerTurorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mArmSetBar = GameObject.FindGameObjectWithTag("LoadingBar").GetComponent<TutorealArmSetGaugeUi>();
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && m_IsCollision)
        {
            if (m_DrawPointObject != null) m_DrawPointObject.SetActive(true);
            if (m_NoDrawPointObject != null) m_NoDrawPointObject.SetActive(false);
            if (m_Text.Length > 0)
                mTutorealText.SetText(m_Text);
            if (m_Voice != null)
                SoundManager.Instance.PlaySe(m_Voice.name);

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

            if (m_IventPrefab != null)
                m_IventPrefab.GetComponent<TutorealIventFlag>().PlayIvent();
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
