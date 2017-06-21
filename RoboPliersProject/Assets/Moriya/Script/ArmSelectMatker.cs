/**==========================================================================*/
/**
 * こめんと
 * 作成者：守屋   作成日：17/10/00
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSelectMatker : MonoBehaviour 
{
	/*==所持コンポーネント==*/

    /*==外部設定変数==*/
    public float length = 1.0f;

    /*==内部設定変数==*/
    private ArmManager m_AM;


    /*==外部参照変数==*/

	void Start() 
	{
        m_AM = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
	}
	
	void Update ()
	{
        transform.position = 
            m_AM.GetEnablPliers().transform.position
            + m_AM.GetEnablPliers().transform.right * length;
	}
}
