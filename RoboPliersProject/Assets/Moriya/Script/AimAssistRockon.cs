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
    SpriteRenderer sr;

    /*==外部設定変数==*/
    public bool m_IsMoveAble = true;
    public float m_LerpValue = 0.6f;

    /*==内部設定変数==*/
    Transform m_Camera;

    Vector3 m_TargetPosition;


    /*==外部参照変数==*/

	void Start() 
	{
        tr = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        m_Camera = GameObject.Find("PlayerCamera").transform;
	}
	
	void LateUpdate ()
	{
        if (m_IsMoveAble)
        {
            tr.LookAt(m_Camera.position);
            tr.position = Vector3.Lerp(tr.position, m_TargetPosition, m_LerpValue);
        }
        else
            sr.enabled = false;
	}

    public void SetTargetPosition(Vector3 pos)
    {
        if (m_TargetPosition.magnitude - pos.magnitude > 1.0f)
            tr.position = pos;

        m_TargetPosition = pos;
    }

    public void SetSpriteDraw(bool value)
    {
        sr.enabled = value;
    }
}
