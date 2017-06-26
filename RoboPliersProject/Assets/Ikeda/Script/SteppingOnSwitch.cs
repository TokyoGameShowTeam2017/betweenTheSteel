using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingOnSwitch : MonoBehaviour
{

    private bool m_IsEnter = false;

    private bool m_IsExit = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //プレイヤーが触れたら
    public void OnTriggerEnter(Collider other)
    {
        m_IsExit = false;
        if (!m_IsEnter)
        {
            m_IsEnter = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        m_IsEnter = false;
        if (!m_IsExit)
        {
            m_IsExit = true;
        }
    }

    public bool GetIsEnter()
    {
        return m_IsEnter;
    }
    public bool GetIsExit()
    {
        return m_IsExit;
    }

}
