using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingOnSwitch : MonoBehaviour
{

    private bool m_IsCollide = false;

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
        if (!m_IsCollide)
        {
            m_IsCollide = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (m_IsCollide)
        {
            m_IsCollide = false;
        }
    }

    public bool GetIsCollide()
    {
        return m_IsCollide;
    }

}
