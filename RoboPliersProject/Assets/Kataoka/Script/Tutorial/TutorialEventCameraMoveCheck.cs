using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventCameraMoveCheck : MonoBehaviour {
    private PlayerTutorialControl mPlayerTutorial;
    private TutorialText mText;
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
    [SerializeField, Tooltip("プレイヤーアームセレクトフラグ")]
    public bool m_PlayerClerArmSelect;
    [SerializeField, Tooltip("プレイヤーアーム伸びフラグ")]
    public bool m_PlayerClerArmExtend;


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

    enum InputDir
    {
        INPUT_LEFT,
        INPUT_RIGHT,
        INPUT_FRONT,
        INPUT_BACK,
        INPUT_NO
    }
    //どのInputが押されているか
    private InputDir mInputDir;
    private InputDir mNowInputDir;
    //押されている時間
    private float mInputTime;
    // Use this for initialization
    void Start()
    {
        mText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();
        mPlayerTutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
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
        if (!GetComponent<TutorialEventFlag>().GetIventFlag() ||
        mText.GetDrawTextFlag()) return;
        m_UiPrefab.SetActive(true);
        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);
        mPlayerTutorial.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutorial.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutorial.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutorial.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutorial.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutorial.SetIsResetAble(!m_PlayerArmReset);
        mPlayerTutorial.SetAllIsArmSelectAble(!m_PlayerArmSelect);
        mPlayerTutorial.SetIsArmStretch(!m_PlayerArmExtend);
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
        //真横に押されていたら
        if (inputVec.x > 0.0f) mInputDir = InputDir.INPUT_RIGHT;
        if (inputVec.x < 0.0f) mInputDir = InputDir.INPUT_LEFT;
        if (inputVec.y > 0.0f) mInputDir = InputDir.INPUT_FRONT;
        if (inputVec.y < 0.0f) mInputDir = InputDir.INPUT_BACK;

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
        //全てのInputが押されていたら
        if (mInputFlags[InputDir.INPUT_BACK] &&
            mInputFlags[InputDir.INPUT_FRONT] &&
            mInputFlags[InputDir.INPUT_LEFT] &&
            mInputFlags[InputDir.INPUT_RIGHT])
        {
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }
            mPlayerTutorial.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutorial.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutorial.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutorial.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutorial.SetIsArmRelease(!m_PlayerClerArmNoCath);
            mPlayerTutorial.SetIsResetAble(!m_PlayerClerArmReset);
            mPlayerTutorial.SetAllIsArmSelectAble(!m_PlayerClerArmSelect);
            mPlayerTutorial.SetIsArmStretch(!m_PlayerClerArmExtend);

            Destroy(gameObject);
        }

    }
}
