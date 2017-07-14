using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventPlayerMove : MonoBehaviour
{
    private PlayerTutorialControl mPlayerTutoreal;
    private TutorealText mText;
    private GameObject mPlayerCamera;

    private Dictionary<InputDir, bool> mInputFlags;
    private Dictionary<InputDir, GameObject> mInputPlates;

    private Transform mMoveCheckTrans;
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

        mMoveCheckTrans = transform.FindChild("PlayerMoveCheck");
        mInputPlates[InputDir.INPUT_LEFT] = mMoveCheckTrans.FindChild("Left").gameObject;
        mInputPlates[InputDir.INPUT_RIGHT] = mMoveCheckTrans.FindChild("Right").gameObject;
        mInputPlates[InputDir.INPUT_FRONT] = mMoveCheckTrans.FindChild("Front").gameObject;
        mInputPlates[InputDir.INPUT_BACK] = mMoveCheckTrans.FindChild("Back").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
        mText.GetDrawTextFlag()) return;

        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);


        mMoveCheckTrans.gameObject.SetActive(true);
        mMoveCheckTrans.transform.position = mPlayerTutoreal.gameObject.transform.position;
        mMoveCheckTrans.rotation =
            Quaternion.Euler(mMoveCheckTrans.rotation.eulerAngles.x,
            mPlayerCamera.transform.rotation.eulerAngles.y,
            mMoveCheckTrans.rotation.eulerAngles.z);

        mPlayerTutoreal.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutoreal.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutoreal.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutoreal.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutoreal.SetIsResetAble(!m_PlayerArmReset);



        Vector2 inputVec = InputManager.GetMove();
        //Debug.Log(inputVec);
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
        if (inputVec.x > 0.0f) mInputDir = InputDir.INPUT_RIGHT;
        if (inputVec.x < 0.0f) mInputDir = InputDir.INPUT_LEFT;
        if (inputVec.y > 0.0f) mInputDir = InputDir.INPUT_FRONT;
        if (inputVec.y < 0.0f) mInputDir = InputDir.INPUT_BACK;

        if (mInputDir == InputDir.INPUT_NO)
        {
            mInputPlates[InputDir.INPUT_BACK].GetComponent<PlayerMoveCheckPlate>().SetColor(0.0f);
            mInputPlates[InputDir.INPUT_FRONT].GetComponent<PlayerMoveCheckPlate>().SetColor(0.0f);
            mInputPlates[InputDir.INPUT_LEFT].GetComponent<PlayerMoveCheckPlate>().SetColor(0.0f);
            mInputPlates[InputDir.INPUT_RIGHT].GetComponent<PlayerMoveCheckPlate>().SetColor(0.0f);
            mInputTime = 0.0f;
            return;
        }

        if (!mInputFlags[mInputDir])
        {
            if (mInputDir != mNowInputDir)
            {
                mInputTime = 0.0f;
                if (mNowInputDir != InputDir.INPUT_NO)
                    mInputPlates[mNowInputDir].GetComponent<PlayerMoveCheckPlate>().SetColor(0.0f);

            }
            else
            {
                mInputTime += Time.deltaTime;
                mInputPlates[mInputDir].GetComponent<PlayerMoveCheckPlate>().SetColor(mInputTime);
            }

            if (mInputTime >= m_InputTime)
            {
                mInputFlags[mInputDir] = true;
                SoundManager.Instance.PlaySe("Answer");
                mInputPlates[mInputDir].GetComponent<PlayerMoveCheckPlate>().IsDead();
            }

            mNowInputDir = mInputDir;
        }

        
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
            mPlayerTutoreal.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutoreal.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutoreal.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutoreal.SetIsArmRelease(!m_PlayerClerArmNoCath);
            mPlayerTutoreal.SetIsResetAble(!m_PlayerClerArmReset);
            Destroy(gameObject);
        }
    }
    public void SetText(string text)
    {

    }
}
