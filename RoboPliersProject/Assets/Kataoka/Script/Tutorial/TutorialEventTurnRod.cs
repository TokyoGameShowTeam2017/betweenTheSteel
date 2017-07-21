using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventTurnRod : MonoBehaviour
{
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;
    [SerializeField, Tooltip("消したいオブジェクト")]
    public GameObject m_DeleteObject;

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
    [SerializeField, Tooltip("プレイヤーアームセレクトフラグ")]
    public bool m_PlayerArmSelect;
    [SerializeField, Tooltip("プレイヤーアーム伸びフラグ")]
    public bool m_PlayerArmExtend;

    //プレイヤーチュートリアル
    private PlayerTutorialControl mPlayerTutoreal;
    //チュートリアルテキスト
    private TutorialText mTutorialText;


    //比較するボーン番号
    private int mBoneNumber;
    // Use this for initialization
    void Start()
    {
        mPlayerTutoreal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mTutorialText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<TutorialEventFlag>().GetIventFlag() ||
            mTutorialText.GetDrawTextFlag()) return;

        //プレイヤー状態登録
        mPlayerTutoreal.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutoreal.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutoreal.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutoreal.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutoreal.SetAllIsArmSelectAble(!m_PlayerArmSelect);
        mPlayerTutoreal.SetIsArmStretch(!m_PlayerArmExtend);


        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);
        //曲がって当たったら
        if (transform.FindChild("Collision").GetComponent<TutorialEventCollision>().GetIsCollision())
        {
            GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(false);
            //次のイベントテキスト有効化
            if (m_IventCollisions.Length != 0)
                for (int i = 0; m_IventCollisions.Length > i; i++)
                {
                    m_IventCollisions[i].GetComponent<PlayerTextIvent>().IsCollisionFlag();
                }

            SoundManager.Instance.PlaySe("Answer");
            //プレイヤー状態登録
            mPlayerTutoreal.SetIsArmMove(!m_PlayerClerArmMove);
            mPlayerTutoreal.SetIsPlayerMove(!m_PlayerClerMove);
            mPlayerTutoreal.SetIsCamerMove(!m_PlayerClerCameraMove);
            mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerClerArmCath);
            mPlayerTutoreal.SetIsArmRelease(!m_PlayerClerArmNoCath);
            mPlayerTutoreal.SetAllIsArmSelectAble(!m_PlayerClerArmSelect);
            mPlayerTutoreal.SetIsArmStretch(!m_PlayerClerArmExtend);
            Destroy(gameObject);

            Destroy(m_DeleteObject);
        }
    }
}
