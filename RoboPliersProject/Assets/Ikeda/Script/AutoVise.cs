using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoVise : MonoBehaviour
{

    private Vector3 m_StartPosition;

    private float m_Ratio;
    [SerializeField, Tooltip("速さを設定")]
    private float m_Speed;

    private bool m_Repeat = false;
    private float m_Timer;
    private float m_StopTime;
    private float m_GoalPosition;

    [SerializeField, Tooltip("クールタイム時間の設定(秒)")]
    private float m_CoolTimer;

    [SerializeField, Tooltip("y軸のポジションをどれくらい下げるか設定")]
    private float m_Position_Y;

    [SerializeField, Tooltip("スイッチに連動させたかったらチェックを外す")]
    private bool m_AutoMode = true;


    private bool m_IsMove = true;

    private bool m_IsFirst;
    // Use this for initialization
    void Start()
    {
        m_IsMove = true;
        m_IsFirst = false;
        m_StartPosition = transform.localPosition;
        m_GoalPosition = transform.localPosition.y - m_Position_Y;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_AutoMode)
        {
            //自動の場合
            m_Timer++;
            if (m_Timer >= (m_CoolTimer * 60))
            {
                m_Ratio += m_Speed;
                transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, m_GoalPosition, transform.localPosition.z), m_Ratio);

                if (m_Ratio >= 1.0f && !m_Repeat)
                {
                    m_Repeat = true;
                    m_Speed *= -1;
                }
                else if (m_Ratio <= 0.0f && m_Repeat)
                {
                    m_Repeat = false;
                    m_Speed *= -1;
                    m_Timer = 0.0f;
                }
            }
        }

        //スイッチに連動して止まる場合
        else
        {            
            m_Timer++;
            if (m_Timer >= (m_CoolTimer * 60))
            {
                m_Ratio += m_Speed;
                transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, m_GoalPosition, transform.localPosition.z), m_Ratio);

                //プレイヤーがスイッチに触れたら
                if (GameObject.Find("switch3").GetComponent<AutoViceSwitch>().GetPlayerIsCollide())
                {
                    m_IsMove = false;
                    m_StopTime++;
                    if (m_StopTime >= GameObject.Find("switch3").GetComponent<AutoViceSwitch>().GetStopTime())
                    {
                        m_Timer = m_CoolTimer * 60;
                        m_StopTime = 0;
                        m_Ratio = 0;
                        m_IsFirst = true;
                        m_IsMove = true;
                    }
                }


                if (m_Ratio >= 1.0f && !m_Repeat)
                {
                    m_Repeat = true;
                    m_Speed *= -1;
                }
                if (m_IsMove)
                {

                    if (m_Ratio <= 0.0f && m_Repeat)
                    {
                        m_Repeat = false;
                        m_Speed *= -1;
                        m_Timer = 0.0f;
                    }
                }
            }
        }




    }

    public bool IsAutoViceMove()
    {
        return m_IsMove;
    }

    public bool IsFirst()
    {
        return m_IsFirst;
    }

    public void SetFirst(bool l_bool)
    {
        m_IsFirst = l_bool;
    }
}
