using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventPlayerMove : MonoBehaviour {
    private PlayerTutorialControl mPlayerTutoreal;
    private TutorealText mText;

    private bool mInputLeft;
    private bool mInputRight;
    private bool mInputFront;
    private bool mInputBack;

    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject m_IventCollision;

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
        mText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mPlayerTutoreal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();

        mInputBack = false;
        mInputFront = false;
        mInputLeft = false;
        mInputRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
        mText.GetDrawTextFlag()) return;
        mPlayerTutoreal.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutoreal.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutoreal.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutoreal.SetIsArmRelease(!m_PlayerArmNoCath);

        if (InputManager.GetMove().x > 0.0f) mInputLeft = true;
        if (InputManager.GetMove().x < 0.0f) mInputRight = true;
        if (InputManager.GetMove().y > 0.0f) mInputFront = true;
        if (InputManager.GetMove().y < 0.0f) mInputBack = true;


        if (mInputLeft && mInputRight && mInputFront && mInputBack)
        {
            //次のイベントテキスト有効化
            m_IventCollision.GetComponent<PlayerTextIvent>().IsCollisionFlag();

            mPlayerTutoreal.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutoreal.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutoreal.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutoreal.SetIsArmRelease(!m_PlayerClerArmNoCath);
            Destroy(gameObject);
        }
        
    }
}
