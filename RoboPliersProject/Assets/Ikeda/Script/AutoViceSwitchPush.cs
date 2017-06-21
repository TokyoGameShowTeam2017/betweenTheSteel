using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoViceSwitchPush : MonoBehaviour
{

    private enum SwitchState
    {
        PushWaitState,
        PushSwitch,
        PushSwitchBack,

        None
    }

    private SwitchState m_State;
    private Vector3 m_StartPosition;
    private Vector3 m_GoalPosition;

    private float m_Rate = 0.0f;

    private float m_Speed = 0.04f;

    [SerializeField]
    private GameObject m_AutoVice;
    // Use this for initialization
    void Start()
    {
        m_State = SwitchState.PushWaitState;
        m_StartPosition = transform.localPosition;
        m_GoalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.035f, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_State)
        {
            case SwitchState.PushWaitState:
                //プレイヤーがスイッチに触れたら
                if (GameObject.Find("switch3").GetComponent<AutoViceSwitch>().GetPlayerIsCollide())
                {
                    m_State = SwitchState.PushSwitch;
                }
                break;

            case SwitchState.PushSwitch:
                //スイッチを動かす
                m_Rate += m_Speed;
                transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Rate);

                //一定距離動いたら、止まる
                if (!m_AutoVice.GetComponent<AutoVise>().IsAutoViceMove())
                {
                    if (m_Rate >= 1.0f)
                    {
                        m_Rate = 1.0f;
                    }
                }
                //動き始めたら戻る
                else if (m_AutoVice.GetComponent<AutoVise>().IsAutoViceMove() && m_AutoVice.GetComponent<AutoVise>().IsFirst())
                {
                    m_Speed *= -1;
                    GameObject.Find("switch3").GetComponent<AutoViceSwitch>().SetCollide(false);
                    m_AutoVice.GetComponent<AutoVise>().SetFirst(false);
                    m_State = SwitchState.PushSwitchBack;
                }
                break;

            case SwitchState.PushSwitchBack:
                if (m_Rate >= 0) m_Rate += m_Speed;
                else
                {
                    m_Speed *= -1;
                    m_State = SwitchState.PushWaitState;
                }
                transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Rate);
                break;
        }
    }
}
