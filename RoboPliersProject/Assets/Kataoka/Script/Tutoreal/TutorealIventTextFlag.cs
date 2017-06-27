using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventTextFlag : MonoBehaviour
{
    private PlayerTutorialControl mTutorealPlayer;
    private TutorealText mTutorealText;
    private GameObject mPoint;
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;
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
    [SerializeField, Tooltip("プレイヤー移動させるか"), Space(15)]
    public bool m_NextDrawPoint;

    // Use this for initialization
    void Start()
    {
        mTutorealPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        if(m_NextDrawPoint)
        mPoint = m_IventCollisions[0].transform.FindChild("Point").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<TutorealIventFlag>().GetIventFlag() &&
            mPoint != null &&
            m_NextDrawPoint)
        {
            mPoint.SetActive(true);
        }
        if (!GetComponent<TutorealIventFlag>().GetIventFlag()) return;

        //テキストが終わったら全部の移動を解除
        if (!mTutorealText.GetDrawTextFlag())
        {
            mTutorealPlayer.SetIsPlayerAndCameraMove(true);
            mTutorealPlayer.SetIsArmMove(true);
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }
            //プレイヤー状態登録
            mTutorealPlayer.SetIsArmMove(!m_PlayerArmMove);
            mTutorealPlayer.SetIsPlayerMove(!m_PlayerMove);
            mTutorealPlayer.SetIsCamerMove(!m_PlayerCameraMove);
            mTutorealPlayer.SetIsArmCatchAble(!m_PlayerArmCath);
            mTutorealPlayer.SetIsArmRelease(!m_PlayerArmNoCath);
            mTutorealPlayer.SetIsResetAble(!m_PlayerArmReset);
            Destroy(gameObject);
        }
    }
}
