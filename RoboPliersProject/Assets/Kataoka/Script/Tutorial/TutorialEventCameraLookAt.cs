﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventCameraLookAt : MonoBehaviour
{

    private PlayerTutorialControl mPlayerTutorial;
    private TutorialText mText;
    private Transform[] mTransforms;
    private GameObject mPlayer;
    //プレイヤーカメラ
    private GameObject mPlayerCamera;
    [SerializeField, Tooltip("生成するTextIventのプレハブ")]
    public GameObject[] m_IventCollisions;
    [SerializeField, Tooltip("当たる範囲")]
    public float m_CollisionSize;
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

    // Use this for initialization
    void Start()
    {
        mText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorialText>();
        mPlayerTutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mPlayerCamera = GameObject.FindGameObjectWithTag("RawCamera");
        mTransforms = transform.GetComponentsInChildren<Transform>();
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        LookAtActiveObject(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<TutorialEventFlag>().GetIventFlag())
            LookAtActiveObject(true);
        if (!GetComponent<TutorialEventFlag>().GetIventFlag() ||
        mText.GetDrawTextFlag()) return;


        mPlayerTutorial.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutorial.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutorial.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutorial.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutorial.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutorial.SetIsResetAble(!m_PlayerArmReset);
        mPlayerTutorial.SetAllIsArmSelectAble(!m_PlayerArmSelect);
        mPlayerTutorial.SetIsArmStretch(!m_PlayerArmExtend);

        if (mPlayerCamera == null) return;
        Ray ray = new Ray(mPlayerCamera.transform.position, mPlayerCamera.transform.forward * 200.0f);
        RaycastHit hit;
        int layer = 1 << 16;
        if (Physics.SphereCast(ray, m_CollisionSize, out hit, 200.0f, layer))
        {
            if (hit.collider.name == "LookAtObject")
            {
                float cameraToPoint = Vector3.Distance(GameObject.FindGameObjectWithTag("RawCamera").transform.position,hit.collider.transform.position);
                float playerToPoint = Vector3.Distance(mPlayer.transform.position, hit.collider.transform.position);
                if (cameraToPoint >= playerToPoint)
                {
                    SoundManager.Instance.PlaySe("Answer");
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        GameObject.FindGameObjectWithTag("TutorialEventText").GetComponent<TutorialEventImageSet>().SetFlag(true);

        //子を全部消したら
        if (transform.childCount <= 0)
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

    public void LookAtActiveObject(bool flag)
    {
        foreach (var i in mTransforms)
        {
            if (i == null) return;
            if (i.name != name)
            {
                i.gameObject.SetActive(flag);
            }
        }
    }

}
