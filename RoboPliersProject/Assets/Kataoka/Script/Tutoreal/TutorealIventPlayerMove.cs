using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventPlayerMove : MonoBehaviour
{
    private PlayerTutorialControl mPlayerTutoreal;
    private TutorealText mText;

    private Dictionary<InputDir, bool> mInputFlags;
    [SerializeField, Tooltip("何秒間でINPUTをOKにするか")]
    private float m_InputTime = 0.5f;
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


    enum InputDir
    {
        INPUT_LEFT,
        INPUT_RIGHT,
        INPUT_FRONT,
        INPUT_BACK,
        INPUT_NO
    }
    private InputDir mInputDir;
    private InputDir mNowInputDir;
    private float mInputTime;
    // Use this for initialization
    void Start()
    {
        mText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mPlayerTutoreal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();

        mInputTime = 0.0f;
        mInputDir = InputDir.INPUT_NO;
        mNowInputDir = InputDir.INPUT_NO;

        mInputFlags = new Dictionary<InputDir, bool>();
        //フラグ初期化
        mInputFlags[InputDir.INPUT_LEFT] = false;
        mInputFlags[InputDir.INPUT_RIGHT] = false;
        mInputFlags[InputDir.INPUT_BACK] = false;
        mInputFlags[InputDir.INPUT_FRONT] = false;
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


        Vector2 inputVec = InputManager.GetMove();
        Vector2 absVec = new Vector2(Mathf.Abs(inputVec.x), Mathf.Abs(inputVec.y));
        mInputDir = InputDir.INPUT_NO;
        if (inputVec.x < 0.0f && inputVec.y < 0.0f)
        {
            if (absVec.x > absVec.y) mInputDir = InputDir.INPUT_LEFT;
            else mInputDir = InputDir.INPUT_BACK;
        }
        if (inputVec.x > 0.0f && inputVec.y < 0.0f)
        {
            if (absVec.x > absVec.y) mInputDir = InputDir.INPUT_RIGHT;
            else mInputDir = InputDir.INPUT_BACK;
        }
        if (inputVec.x < 0.0f && inputVec.y > 0.0f)
        {
            if (absVec.x > absVec.y) mInputDir = InputDir.INPUT_LEFT;
            else mInputDir = InputDir.INPUT_FRONT;

        }
        if (inputVec.x > 0.0f && inputVec.y > 0.0f)
        {
            if (absVec.x > absVec.y) mInputDir = InputDir.INPUT_RIGHT;
            else mInputDir = InputDir.INPUT_FRONT;
        }

        if (inputVec.x >= 1.0f) mInputDir = InputDir.INPUT_RIGHT;
        if (inputVec.x <= -1.0f) mInputDir = InputDir.INPUT_LEFT;
        if (inputVec.y >= 1.0f) mInputDir = InputDir.INPUT_FRONT;
        if (inputVec.y <= -1.0f) mInputDir = InputDir.INPUT_BACK;


        if (mInputDir != InputDir.INPUT_NO)
        {
            if (mInputDir == mNowInputDir)
            {
                mInputTime += Time.deltaTime;
            }
            else
                mInputTime = 0.0f;
        }
        mNowInputDir = mInputDir;

        if (mInputTime >= m_InputTime)
        {
            mInputFlags[mInputDir] = true;
            mInputTime = 0.0f;
        }


        if (mInputFlags[InputDir.INPUT_BACK] &&
            mInputFlags[InputDir.INPUT_FRONT] &&
            mInputFlags[InputDir.INPUT_LEFT] &&
            mInputFlags[InputDir.INPUT_RIGHT])
        {
            //次のイベントテキスト有効化

            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }
            mPlayerTutoreal.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutoreal.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutoreal.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutoreal.SetIsArmRelease(!m_PlayerClerArmNoCath);
            Destroy(gameObject);
        }

    }
}
