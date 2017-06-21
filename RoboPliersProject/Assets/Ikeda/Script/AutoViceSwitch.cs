using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoViceSwitch : MonoBehaviour
{

    private bool m_IsCollide = false;

    [SerializeField, Tooltip("何秒間止めるか")]
    private float m_StopTime;
    // Use this for initialization
    void Start()
    {
        m_IsCollide = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //プレイヤーが触れたら
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !m_IsCollide)
        {
            m_IsCollide = true;
        }
    }

    public bool GetPlayerIsCollide()
    {
        return m_IsCollide;
    }

    public float GetStopTime()
    {
        return m_StopTime * 60;
    }

    public void SetCollide(bool l_bool)
    {
        m_IsCollide = l_bool;
    }
}
