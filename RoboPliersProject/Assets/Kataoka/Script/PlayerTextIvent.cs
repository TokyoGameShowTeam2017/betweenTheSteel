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
    [SerializeField, Tooltip("ポイントをを表示するかどうか"), Space(15)]
    public bool m_IsDrawPoint;
    [SerializeField, Tooltip("最初からポイントを表示させるか")]
    public bool m_IsDrawFirstPoint;
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

    private GameObject mPointObject;
    //プレイヤーテキスト
    private TutorealText mTutorealText;
    //プレイヤーのチュートリアル
    private PlayerTutorialControl mPlayerTurorial;
    [SerializeField, Tooltip("当たるかどうか"), Space(15)]
    public bool m_IsCollision;



    // Use this for initialization
    void Start()
    {
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mPlayerTurorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        if (transform.FindChild("Point") != null)
            mPointObject = transform.FindChild("Point").gameObject;

    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && m_IsCollision)
        {
            if (mPointObject != null)
                mPointObject.SetActive(m_IsCollision && m_IsDrawPoint);

            if (m_Text.Length > 0)
                mTutorealText.SetText(m_Text);
            if (m_Voice != null)
                SoundManager.Instance.PlaySe(m_Voice.name);


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
    public void IsCollisionFlag()
    {
        m_IsCollision = true;
    }

    public GameObject GetIvent()
    {
        return m_IventPrefab;
    }
}
