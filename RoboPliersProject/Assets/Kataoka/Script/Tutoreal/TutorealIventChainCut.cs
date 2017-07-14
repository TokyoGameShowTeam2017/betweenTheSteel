using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventChainCut : MonoBehaviour {
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;

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
    [SerializeField, Tooltip("プレイヤーアームリセットフラグ")]
    public bool m_PlayerClerArmReset;

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

    //アームマネージャー
    private ArmManager mArm;
    //プレイヤーチュートリアル
    private PlayerTutorialControl mPlayerTutoreal;
    //チュートリアルテキスト
    private TutorealText mTutorealText;
    // Use this for initialization
    void Start()
    {
        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        mPlayerTutoreal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
            mTutorealText.GetDrawTextFlag()) return;

        //プレイヤー状態登録
        mPlayerTutoreal.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutoreal.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutoreal.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutoreal.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutoreal.SetIsResetAble(!m_PlayerArmReset);
        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);
        if (mArm.GetEnablPliersMove().GetChainCut())
        {
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);

            mPlayerTutoreal.SetIsArmMove(true);
            mPlayerTutoreal.SetIsPlayerAndCameraMove(true);
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }
            //プレイヤー状態登録
            mPlayerTutoreal.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutoreal.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutoreal.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutoreal.SetIsArmRelease(!m_PlayerClerArmNoCath);
            mPlayerTutoreal.SetIsResetAble(!m_PlayerClerArmReset);

            SoundManager.Instance.PlaySe("Answer");
            Destroy(gameObject);
        }
    }
}
