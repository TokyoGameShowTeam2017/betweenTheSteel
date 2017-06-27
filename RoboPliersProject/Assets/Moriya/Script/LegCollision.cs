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
    private Vector3 m_Dir = Vector3.down;
    private Transform m_Player;

    /*==外部参照変数==*/
    public bool IsHit { get; set; }
    public RaycastHit HitInfo { get; set; }





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
        Ray ray = new Ray(Vector3.Lerp(tr.position, m_Player.position, 0.5f), m_Dir);
        int mask = LayerMask.NameToLayer("ArmAndPliers");
        RaycastHit hit;
        IsHit = Physics.Raycast(ray, out hit, 1.0f, mask);
        HitInfo = hit;
	}

}
