using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventSetObject : MonoBehaviour {
    //プレイヤーチュートリアル
    private PlayerTutorialControl mPlayerTutorial;
    //チュートリアルテキスト
    private TutorialText mTutorialText;
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
    [SerializeField, Tooltip("プレイヤーアーム離せるか"), HeaderAttribute("いつでも離せるのならFalse")]
    public bool m_PlayerArmNoCath;
    [SerializeField, Tooltip("プレイヤーアームリセットフラグ")]
    public bool m_PlayerArmReset;
    [SerializeField, Tooltip("プレイヤーアームセレクトフラグ")]
    public bool m_PlayerArmSelect;
    [SerializeField, Tooltip("プレイヤーアーム伸びフラグ")]
    public bool m_PlayerArmExtend;
    //子のトランスフォーム
    List<Transform> mTransorms;



    private bool mIsCollision;
    // Use this for initialization
    void Start()
    {
        mPlayerTutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorialText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();
        mArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        mIsCollision = false;

        Transform[] transs;
        transs = transform.GetComponentsInChildren<Transform>();
        mTransorms = new List<Transform>();

        foreach (var i in transs)
        {
            if (i.name != name)
            {
                mTransorms.Add(i);
            }
        }
        foreach (var i in mTransorms)
        {
            i.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!GetComponent<TutorialEventFlag>().GetIventFlag() ||
            mTutorialText.GetDrawTextFlag())
        {
            return;
        }
        foreach (var i in mTransorms) {
            i.gameObject.SetActive(true);
        }
        mPlayerTutorial.SetIsArmRelease(false);
        mPlayerTutorial.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutorial.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutorial.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutorial.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutorial.SetIsResetAble(!m_PlayerArmReset);
        mPlayerTutorial.SetAllIsArmSelectAble(!m_PlayerArmSelect);
        mPlayerTutorial.SetIsArmStretch(!m_PlayerArmExtend);
        if(!m_PlayerArmNoCath)
        mPlayerTutorial.SetIsArmRelease(true);
        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);

        int flagCount = 0;
        int childCount = 2;
        foreach (var i in mTransorms)
        {
            i.gameObject.SetActive(true);
            if (i.GetComponent<TutorialEventCollision>().GetIsCollision())
            {
                flagCount++;
            }
        }

        if (flagCount >= childCount)
        {
            mPlayerTutorial.SetIsArmRelease(true);
        }

        //クリアー処理
        if (flagCount >= childCount&&mArmManager.GetEnablArmCatchingObject()==null)
        {
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);
            
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }

            SoundManager.Instance.PlaySe("Answer");
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
