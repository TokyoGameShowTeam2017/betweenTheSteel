using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventTextFlag : MonoBehaviour
{
    private PlayerTutorialControl mTutorialPlayer;
    private TutorialText mTutorialText;
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
    [SerializeField, Tooltip("プレイヤーアームセレクトフラグ")]
    public bool m_PlayerArmSelect;
    [SerializeField, Tooltip("プレイヤーアーム伸びフラグ")]
    public bool m_PlayerArmExtend;

    // Use this for initialization
    void Start()
    {
        mTutorialPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorialText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorialEventFlag>().GetIventFlag()) return;

        //テキストが終わったら全部の移動を解除
        if (!mTutorialText.GetDrawTextFlag())
        {
            if (GameObject.FindGameObjectWithTag("TutorialEventText") != null)
                GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);
            mTutorialPlayer.SetIsPlayerAndCameraMove(true);
            mTutorialPlayer.SetIsArmMove(true);
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }
            //プレイヤー状態登録
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
