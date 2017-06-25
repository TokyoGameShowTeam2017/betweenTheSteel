using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealIventInitialize : MonoBehaviour {
    [SerializeField, Tooltip("プレイヤー移動させるか"), HeaderAttribute("一番最初のプレイヤーの状態")]
    public bool m_PlayerMove;
    [SerializeField, Tooltip("プレイヤーカメラ移動させるか")]
    public bool m_PlayerCameraMove;
    [SerializeField, Tooltip("プレイヤーアーム動かせるか")]
    public bool m_PlayerArmMove;
    [SerializeField, Tooltip("プレイヤーアーム掴めるか")]
    public bool m_PlayerArmCath;
    [SerializeField, Tooltip("プレイヤーアーム離せるか")]
    public bool m_PlayerArmNoCath;
    [SerializeField, Tooltip("アーム1"), HeaderAttribute("アームの起動状態")]
    public bool m_PlayerArm1;
    [SerializeField, Tooltip("アーム2")]
    public bool m_PlayerArm2;
    [SerializeField, Tooltip("アーム3")]
    public bool m_PlayerArm3;
    [SerializeField, Tooltip("アーム4")]
    public bool m_PlayerArm4;

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerTutorialControl control = other.GetComponent<PlayerTutorialControl>();

            //プレイヤー状態登録
            control.SetIsArmMove(!m_PlayerArmMove);
            control.SetIsPlayerMove(!m_PlayerMove);
            control.SetIsCamerMove(!m_PlayerCameraMove);
            control.SetIsArmCatchAble(!m_PlayerArmCath);
            control.SetIsArmRelease(!m_PlayerArmNoCath);

            control.SetIsActiveArm(0, m_PlayerArm1);
            control.SetIsActiveArm(1, m_PlayerArm2);
            control.SetIsActiveArm(2, m_PlayerArm3);
            control.SetIsActiveArm(3, m_PlayerArm4);

            Destroy(gameObject);
        }
    }



}
