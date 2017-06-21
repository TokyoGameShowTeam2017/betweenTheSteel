using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{

    enum PressStartState
    {
        StartState,
        FlashingState,
        FadeOut,

        None
    }

    [SerializeField, Tooltip("何秒後に表示するか")]
    private float m_Second = 0;

    private int m_Timer = 0;

    private PressStartState m_State = PressStartState.StartState;

    [SerializeField, Tooltip("点滅時にα値を下げるスピードの設定")]
    private float m_LowerSpeed = 0.01f;

    [SerializeField, Tooltip("フェードアウト時に何倍するか")]
    private float m_ScaleDouble = 1.0f;

    [SerializeField, Tooltip("フェードアウト時にα値を下げるスピードの設定")]
    private float m_FadeLowerSpeed = 0.01f;

    private float m_Alpha = 0.0f;

    private Vector3 m_Scale;

    private bool m_Flap = false;

    // Use this for initialization
    void Start()
    {
        m_Scale = GameObject.Find("pressstart").transform.localScale;
        m_Alpha = 0.0f;
}

// Update is called once per frame
void Update()
    {

        switch (m_State)
        {
            case PressStartState.StartState:
                if (GameObject.Find("title").GetComponent<Title>().IsTitleEnd())
                {
                    if (m_Timer >= m_Second * 60 || SpeedDraw())
                    {
                        m_Alpha += 0.03f;
                        GameObject.Find("pressstartback").GetComponent<CanvasGroup>().alpha = m_Alpha;
                        if (m_Alpha >= 1)
                        {
                            m_Alpha = 1.0f;
                            m_State = PressStartState.FlashingState;
                        }
                    }
                    m_Timer++;
                }
                break;

            case PressStartState.FlashingState:
                if (!m_Flap)
                {
                    m_Alpha -= m_LowerSpeed;
                    GameObject.Find("pressstart").GetComponent<CanvasGroup>().alpha = m_Alpha;
                    if (m_Alpha <= 0)
                    {
                        m_Flap = true;
                    }
                }
                else
                {
                    m_Alpha += m_LowerSpeed;
                    GameObject.Find("pressstart").GetComponent<CanvasGroup>().alpha = m_Alpha;
                    if (m_Alpha >= 1)
                    {
                        m_Flap = false;
                    }
                }

                /* PRESS STARTが押されたか */
                if (GameObject.Find("Stage00Manager").GetComponent<StageInputManager>().GetPressButton())
                {
                    GameObject.Find("pressstart").GetComponent<CanvasGroup>().alpha = 1.0f;
                    m_Alpha = 1.0f;
                    m_State = PressStartState.FadeOut;
                }
                break;

                //フェードアウト
            case PressStartState.FadeOut:
                if (m_Alpha >= 0) m_Scale += (m_Scale * m_ScaleDouble) / 120;

                GameObject.Find("pressstart").transform.localScale = new Vector3(m_Scale.x, m_Scale.y, m_Scale.z);

                if (m_Alpha >= 0)
                {
                    m_Alpha -= m_FadeLowerSpeed;
                }
                else
                {
                    m_State = PressStartState.None;
                }
                GameObject.Find("pressstartback").GetComponent<CanvasGroup>().alpha = m_Alpha;
                GameObject.Find("pressstart").GetComponent<CanvasGroup>().alpha = m_Alpha;
                break; 
        }
    }

    private bool SpeedDraw()
    {
        return GameObject.Find("Stage00Manager").GetComponent<StageInputManager>().GetSpeedDraw();
    }

    public int GetPressState()
    {
        return (int)m_State;
    }
}
