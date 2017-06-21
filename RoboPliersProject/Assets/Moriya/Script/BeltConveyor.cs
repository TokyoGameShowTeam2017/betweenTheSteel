/**==========================================================================*/
/**    //aaa
 * ベルトコンベアの処理
 * 作成者：守屋   作成日：17/05/19
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour 
{
	/*==所持コンポーネント==*/

    /*==外部設定変数==*/
    [SerializeField, Tooltip("ベルトの速度")]
    private float m_BeltSpeed = 7.0f;
    //[SerializeField, Tooltip("アニメーション速度")]
    //private float m_AnimSpeed = 1.0f;

    /*==内部設定変数==*/

    /*==外部参照変数==*/
    private Vector3 m_BeltVelocity;
    //Animator[] m_Anims;

	void Start() 
	{
        m_BeltVelocity = transform.forward * m_BeltSpeed;

        //m_Anims = gameObject.GetComponentsInChildren<Animator>();
        //foreach (Animator a in m_Anims)
        //{
        //    a.speed = 0.001f;
        //}
	}
	
	void Update ()
	{
		
	}

    void OnCollisionStay(Collision coll)
    {
        //プレイヤー以外のオブジェクトを運ぶ
        if(coll.transform.tag != "Player")
        {
            Rigidbody rb = coll.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //rb.AddForce(m_BeltVelocity, ForceMode.Acceleration);
                rb.velocity = Vector3.zero;
            }

            coll.transform.position += m_BeltVelocity * Time.deltaTime;
        }
    }

    void OnCollisionExit(Collision coll)
    {
        //プレイヤー以外のオブジェクトを運ぶ
        if (coll.transform.tag != "Player")
        {
            Rigidbody rb = coll.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //rb.AddForce(m_BeltVelocity, ForceMode.Force);
                rb.velocity = m_BeltVelocity / 2.0f;
            }
        }
    }

    public Vector3 GetBeltVelocity()
    {
        return m_BeltVelocity;
    }
}
