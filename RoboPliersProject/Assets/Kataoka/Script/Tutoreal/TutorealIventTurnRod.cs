using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventTurnRod : MonoBehaviour
{

    public GameObject mMainRod1;
    public GameObject mPlayerMainRod2;

    private List<GameObject> mRotatePoint1;
    private List<GameObject> mRotatePoint2;
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

    //プレイヤーチュートリアル
    private PlayerTutorialControl mPlayerTutoreal;
    //チュートリアルテキスト
    private TutorealText mTutorealText;


    //比較するボーン番号
    private int mBoneNumber;
    // Use this for initialization
    void Start()
    {
        mRotatePoint1 = mMainRod1.GetComponent<Rod>().GetBone();
        mRotatePoint2 = mPlayerMainRod2.GetComponent<Rod>().GetBone();

        int index = 0;
        foreach (var i in mRotatePoint1)
        {
            if (i.transform.eulerAngles.x != 0.0f ||
                i.transform.eulerAngles.y != 0.0f)
                break;
            index++;
        }
        mBoneNumber = index;

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

        Vector3 angle1=mRotatePoint1[mBoneNumber].transform.eulerAngles;
        Vector3 angle2=mRotatePoint2[mBoneNumber].transform.eulerAngles;
        float disX =Mathf.Abs(angle1.x-angle2.x);
        float disZ=Mathf.Abs(angle1.z-angle2.z);
        //曲がりがほとんど一緒だったら
        if (disX <= 10.0f && disZ <= 10.0f)
        {
            //次のイベントテキスト有効化
            m_IventCollision.GetComponent<PlayerTextIvent>().IsCollisionFlag();

            //プレイヤー状態登録
            mPlayerTutoreal.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutoreal.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutoreal.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutoreal.SetIsArmRelease(!m_PlayerClerArmNoCath);

            Destroy(gameObject);
        }
    }
}
