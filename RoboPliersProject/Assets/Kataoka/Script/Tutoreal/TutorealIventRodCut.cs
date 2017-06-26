using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventRodCut : MonoBehaviour
{

    private PlayerTutorialControl mTutorialPlayer;
    private TutorealText mTutorialText;

    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;
    [SerializeField, Tooltip("Rodたち")]
    public GameObject[] m_Rod;

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
    // Use this for initialization
    void Start()
    {
        mTutorialPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorialText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
        mTutorialText.GetDrawTextFlag()) return;
        mTutorialPlayer.SetIsArmMove(!m_PlayerArmMove);
        mTutorialPlayer.SetIsPlayerMove(!m_PlayerMove);
        mTutorialPlayer.SetIsCamerMove(!m_PlayerCameraMove);
        mTutorialPlayer.SetIsArmCatchAble(!m_PlayerArmCath);
        mTutorialPlayer.SetIsArmRelease(!m_PlayerArmNoCath);

        foreach (var i in m_Rod)
        {
            //Cutされたら
            if (i.GetComponent<CutRod>().GetCutFlag())
            {
                //次のイベントテキスト有効化
                if (m_IventCollisions.Length != 0)
                    for (int j = 0; m_IventCollisions.Length > j; j++)
                    {
                        m_IventCollisions[j].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                    }
                mTutorialPlayer.SetIsArmMove(!m_PlayerClerArmMove);
                mTutorialPlayer.SetIsPlayerMove(!m_PlayerClerMove);
                mTutorialPlayer.SetIsCamerMove(!m_PlayerClerCameraMove);
                mTutorialPlayer.SetIsArmCatchAble(!m_PlayerClerArmCath);
                mTutorialPlayer.SetIsArmRelease(!m_PlayerClerArmNoCath);

                Destroy(gameObject);
            }
        }
    }
}
