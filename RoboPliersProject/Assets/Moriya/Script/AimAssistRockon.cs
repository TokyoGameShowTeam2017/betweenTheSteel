/**==========================================================================*/
/**
 * こめんと
 * 作成者：守屋   作成日：17/05/30
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistRockon : MonoBehaviour 
{
	/*==所持コンポーネント==*/
    Transform tr;

    /*==外部設定変数==*/

    /*==内部設定変数==*/
    Transform m_Camera;

    /*==外部参照変数==*/

	void Start() 
	{
        tr = GetComponent<Transform>();
        m_Camera = GameObject.Find("PlayerCamera").transform;
	}
	
	void LateUpdate ()
	{
        tr.LookAt(m_Camera.position);
	}
}
