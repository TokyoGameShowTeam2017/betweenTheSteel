/**==========================================================================*/
/**
 * こめんと
 * 作成者：守屋   作成日：17/10/00
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegCollision : MonoBehaviour 
{
	/*==所持コンポーネント==*/
    Transform tr;

    /*==外部設定変数==*/

    /*==内部設定変数==*/
    public float m_RayLength = 1.0f;
    [Tooltip("Rayの開始地点をどれだけプレイヤーに近づけるか 1.0でプレイヤーと同座標、0.5で中間")]
    public float m_PlayerNearLerpValue = 0.5f;
    private Vector3 m_Dir = Vector3.down;
    private Transform m_Player;


    /*==外部参照変数==*/
    public bool IsHit { get; set; }
    public RaycastHit HitInfo { get; set; }
    public Vector3 ToPlayerVec { get;set; }

	void Start() 
	{
        tr = GetComponent<Transform>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        IsHit = true;
	}
	
	void Update ()
	{
        //レイを飛ばす
        Vector3 dir = tr.forward;
        Vector3 start = Vector3.Lerp(tr.position, m_Player.position, m_PlayerNearLerpValue);
        Ray ray = new Ray(start, m_Dir);
        int mask = LayerMask.NameToLayer("ArmAndPliers");
        RaycastHit hit;
        IsHit = Physics.Raycast(ray, out hit, m_RayLength, mask);
        HitInfo = hit;

        //if(IsHit)
        //{
        //    Vector3 v = m_Player.position - start;
        //    v.y = 0;
        //    ToPlayerVec = v.normalized;

        //}

        Debug.DrawLine(start, start + m_Dir * 1.0f, Color.red);

	}

}
