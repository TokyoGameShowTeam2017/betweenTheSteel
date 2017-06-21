using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerCollide : MonoBehaviour
{
    private bool m_PlayerStay = false;

    private Vector3 m_DefaultScale = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        m_DefaultScale = GameObject.FindGameObjectWithTag("Player").transform.lossyScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 l_LossyScale = GameObject.FindGameObjectWithTag("Player").transform.lossyScale;
        //Vector3 l_LocalScale = GameObject.FindGameObjectWithTag("Player").transform.localScale;

        GameObject.FindGameObjectWithTag("Player").transform.localScale = m_DefaultScale;

        if (m_PlayerStay)
        {

            GameObject.FindGameObjectWithTag("Player").transform.parent = transform.parent.transform;
        }
    
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.parent = null;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_PlayerStay = true;
    }

    public void OnTriggerExit(Collider other)
    {
        m_PlayerStay = false;
    }
}
