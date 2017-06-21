/**==========================================================================*/
/**
 * UAVの移動
 * 作成者：守屋   作成日：17/10/00
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAVMove : MonoBehaviour 
{
	/*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField, Tooltip("回転速度")]
    private float m_RotationSpeed = 360.0f;


    /*==内部設定変数==*/
    private Transform m_CameraTr;


    /*==外部参照変数==*/

    void Awake()
    {
        tr = GetComponent<Transform>();
    }

	void Start() 
	{
        m_CameraTr = Camera.main.transform;
	}
	
	void Update ()
	{
        tr.position = Vector3.Lerp(tr.position, m_CameraTr.position - m_CameraTr.forward, 0.1f);
        tr.Rotate(Vector3.up, m_RotationSpeed * Time.deltaTime);

	}
}
