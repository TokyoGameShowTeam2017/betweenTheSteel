using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventSetObject : MonoBehaviour {
    //プレイヤーチュートリアル
    private PlayerTutorialControl mPlayerTutorial;
    //チュートリアルテキスト
    private TutorealText mTutorealText;
    //アームマネージャー
    private ArmManager mArmManager;
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;

    //[SerializeField, Tooltip("あたり判定のオブジェクト")]
    //public GameObject m_CollisionObject;
    //[SerializeField, Tooltip("どのぐらい近づけばいいか")]
    //public float m_Distance = 5.0f;

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
    //[SerializeField, Tooltip("プレイヤーアーム離せるか")]
    //public bool m_PlayerArmNoCath;

    private bool mIsCollision;
    // Use this for initialization
    void Start()
    {
        mPlayerTutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorealText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        mIsCollision = false;
    }

    void Update()
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
            mTutorealText.GetDrawTextFlag())
        {
            return;
        }

        mPlayerTutorial.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutorial.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutorial.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutorial.SetIsArmCatchAble(!m_PlayerArmCath);
        //絶対離せないため
        if(!mIsCollision)
        mPlayerTutorial.SetIsArmRelease(false);
        mIsCollision = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!GetComponent<TutorealIventFlag>().GetIventFlag()||
            mTutorealText.GetDrawTextFlag()||other.tag=="Player") return;

        mPlayerTutorial.SetIsArmRelease(true);
        mIsCollision = true;
        if (mArmManager.GetEnablArmCatchingObject() == null)
        {
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

            Destroy(gameObject);
        }
    }
}
