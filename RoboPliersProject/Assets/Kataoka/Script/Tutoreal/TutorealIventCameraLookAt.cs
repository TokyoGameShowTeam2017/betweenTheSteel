﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventCameraLookAt : MonoBehaviour
{

    private PlayerTutorialControl mPlayerTutoreal;
    private TutorealText mText;
    private Transform[] mTransforms;

    //プレイヤーカメラ
    private GameObject mPlayerCamera;
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

    // Use this for initialization
    void Start()
    {
        mText = GameObject.FindGameObjectWithTag("PlayerText").GetComponent<TutorealText>();
        mPlayerTutoreal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTutorialControl>();
        mPlayerCamera = GameObject.FindGameObjectWithTag("RawCamera");
        mTransforms = transform.GetComponentsInChildren<Transform>();
        LookAtActiveObject(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<TutorealIventFlag>().GetIventFlag())
            LookAtActiveObject(true);
        if (!GetComponent<TutorealIventFlag>().GetIventFlag() ||
        mText.GetDrawTextFlag()) return;


        mPlayerTutoreal.SetIsArmMove(!m_PlayerArmMove);
        mPlayerTutoreal.SetIsPlayerMove(!m_PlayerMove);
        mPlayerTutoreal.SetIsCamerMove(!m_PlayerCameraMove);
        mPlayerTutoreal.SetIsArmCatchAble(!m_PlayerArmCath);
        mPlayerTutoreal.SetIsArmRelease(!m_PlayerArmNoCath);
        mPlayerTutoreal.SetIsResetAble(!m_PlayerArmReset);


        Ray ray = new Ray(mPlayerCamera.transform.position, mPlayerCamera.transform.forward * 50.0f);
        RaycastHit hit;
        int layer = ~(1 << 15 | 1 << 17);
        if (Physics.SphereCast(ray, 1.5f, out hit, 200.0f, layer))
        {
            if (hit.collider.name == "LookAtObject")
            {
                Destroy(hit.collider.gameObject);
            }
        }
        //子を全部消したら
        if (transform.childCount <= 0)
        {
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
