/**==========================================================================*/
/**
 * 軸回転時の座標の移動計算
 * 作成者：守屋   作成日：17/06/02
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMovePosition : MonoBehaviour 
{
	/*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/

    /*==内部設定変数==*/
    private PlayerMove m_Player;
    private PlayerManager m_PlayerManager;
    private ArmManager m_ArmManager;

    /*==外部参照変数==*/

    void Awake()
    {

    }

	void Start() 
	{
        tr = GetComponent<Transform>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        m_PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
	}
	
	void Update ()
	{
        Vector3 catchPosition = m_PlayerManager.GetAxisMoveObject().transform.position;

        Transform armtr = m_ArmManager.GetEnablArm().transform;
        Vector3 armforward = armtr.forward;
        float armlength = m_ArmManager.GetEnablArmMove().GetArmStretch();
        //print(armlength);

        //アーム根元座標からプレイヤー座標に向かうベクトル
        Vector3 arm2player = m_Player.transform.position - armtr.position;
        //掴んだ座標からペンチの根元座標に向かうベクトル
        Vector3 catch2pliers = m_ArmManager.GetEnablPliers().transform.position - catchPosition;

        Vector3 pos =
            catchPosition
            + catch2pliers
            + -armforward * armlength
            + arm2player;

        tr.position = pos;


    }
}
