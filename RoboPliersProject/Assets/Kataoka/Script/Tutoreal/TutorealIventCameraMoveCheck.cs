using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventCameraMoveCheck : MonoBehaviour {
    private PlayerTutorialControl mPlayerTutoreal;
    private TutorealText mText;
    private GameObject mPlayerCamera;

    private Dictionary<InputDir, bool> mInputFlags;
    private Dictionary<InputDir, GameObject> mInputPlates;

    [SerializeField, Tooltip("何秒間でINPUTをOKにするか")]
    private float m_InputTime = 0.5f;
    [SerializeField, Tooltip("カメラ移動のUIプレハブ")]
    private GameObject m_UiPrefab;
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
    struct PlateState
    {
        GameObject plate;
        float colorRed;
    }

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

        mPlayerCamera = GameObject.FindGameObjectWithTag("RawCamera").gameObject;

        mInputTime = 0.0f;
        mInputDir = InputDir.INPUT_NO;
        mNowInputDir = InputDir.INPUT_NO;

        mInputPlates = new Dictionary<InputDir, GameObject>();
        mInputFlags = new Dictionary<InputDir, bool>();
        //フラグ初期化
        mInputFlags[InputDir.INPUT_LEFT] = false;
        mInputFlags[InputDir.INPUT_RIGHT] = false;
        mInputFlags[InputDir.INPUT_BACK] = false;
        mInputFlags[InputDir.INPUT_FRONT] = false;

        mInputPlates[InputDir.INPUT_LEFT] = m_UiPrefab.transform.FindChild("Left").gameObject;
        mInputPlates[InputDir.INPUT_RIGHT] = m_UiPrefab.transform.FindChild("Right").gameObject;
        mInputPlates[InputDir.INPUT_FRONT] = m_UiPrefab.transform.FindChild("Down").gameObject;
        mInputPlates[InputDir.INPUT_BACK] = m_UiPrefab.transform.FindChild("Up").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
        mText.GetDrawTextFlag()) return;

        m_UiPrefab.SetActive(true);
        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventTextSet>().SetDrawFlag(true);
        mPlayerTutoreal.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutoreal.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutoreal.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutoreal.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutoreal.SetIsResetAble(!m_PlayerArmReset);

        Vector2 inputVec = InputManager.GetCameraMove();
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

        if (mInputDir == InputDir.INPUT_NO)
        {
            mInputPlates[InputDir.INPUT_BACK].GetComponent<PlayerCameraMoveCheckUi>().SetColor(0.0f);
            mInputPlates[InputDir.INPUT_FRONT].GetComponent<PlayerCameraMoveCheckUi>().SetColor(0.0f);
            mInputPlates[InputDir.INPUT_LEFT].GetComponent<PlayerCameraMoveCheckUi>().SetColor(0.0f);
            mInputPlates[InputDir.INPUT_RIGHT].GetComponent<PlayerCameraMoveCheckUi>().SetColor(0.0f);
            mInputTime = 0.0f;
            return;
        }

        if (!mInputFlags[mInputDir])
        {
            if (mInputDir != mNowInputDir)
            {
                mInputTime = 0.0f;
                if (mNowInputDir != InputDir.INPUT_NO)
                    mInputPlates[mNowInputDir].GetComponent<PlayerCameraMoveCheckUi>().SetColor(0.0f);

            }
            else
            {
                mInputTime += Time.deltaTime;
                mInputPlates[mInputDir].GetComponent<PlayerCameraMoveCheckUi>().SetColor(mInputTime);
            }

            if (mInputTime >= m_InputTime)
            {
                mInputFlags[mInputDir] = true;
                SoundManager.Instance.PlaySe("Answer");
                mInputPlates[mInputDir].GetComponent<PlayerCameraMoveCheckUi>().IsDead();
            }

            mNowInputDir = mInputDir;
        }

        if (mInputFlags[InputDir.INPUT_BACK] &&
            mInputFlags[InputDir.INPUT_FRONT] &&
            mInputFlags[InputDir.INPUT_LEFT] &&
            mInputFlags[InputDir.INPUT_RIGHT])
        {
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventTextSet>().SetDrawFlag(false);
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
            mPlayerTutoreal.SetIsResetAble(!m_PlayerClerArmReset);

            Destroy(gameObject);
        }

    }
}
