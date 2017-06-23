using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{

    [SerializeField, Tooltip("何秒後に表示するか")]
    private float m_Second = 0;

    private int m_Timer = 0;

    [SerializeField, Tooltip("点滅時にα値を下げるスピードの設定")]
    private float m_LowerSpeed = 0.01f;

    [SerializeField, Tooltip("フェードアウト時に何倍するか")]
    private float m_ScaleDouble = 1.0f;

    [SerializeField, Tooltip("フェードアウト時にα値を下げるスピードの設定")]
    private float m_FadeLowerSpeed = 0.01f;

    private float m_Alpha = 0.0f;

    private Vector3 m_Scale;

    private bool m_Flap = false;

    private bool m_SpeedDraw = false;

    // Use this for initialization
    void Start()
    {
        m_Scale = GameObject.Find("pressstart").transform.localScale;
        m_Alpha = 0.0f;
        m_Timer = 0;
        m_Flap = false;
        m_SpeedDraw = false;
    }
    public void PressStartRapidDraw(bool rapidDraw)
    {
        if (rapidDraw)
        {
            GameObject.Find("pressstartback").GetComponent<CanvasGroup>().alpha = 1.0f;
            transform.parent.GetComponent<TitleCollection>().SetTitleState(1);
        }
    }

    public void PressStartDraw()
    {
        if (GameObject.Find("title").GetComponent<Title>().IsTitleEnd())
        {
            if (m_Timer >= m_Second * 60)
            {
                m_Alpha += 0.05f;
                GameObject.Find("pressstartback").GetComponent<CanvasGroup>().alpha = m_Alpha;
                if (m_Alpha >= 1)
                {
                    m_Alpha = 1.0f;
                    transform.parent.GetComponent<TitleCollection>().SetTitleState(1);
                }
            }
            m_Timer++;
        }
    }

    public void FlashingState()
    {
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
    }

    public void PressStartFadeOut()
    {
        if (m_Alpha >= 0) m_Scale += (m_Scale * m_ScaleDouble) / 120;

        GameObject.Find("pressstart").transform.localScale = new Vector3(m_Scale.x, m_Scale.y, m_Scale.z);

        if (m_Alpha >= 0)
        {
            m_Alpha -= m_FadeLowerSpeed;
        }
        GameObject.Find("pressstartback").GetComponent<CanvasGroup>().alpha = m_Alpha;
        GameObject.Find("pressstart").GetComponent<CanvasGroup>().alpha = m_Alpha;
    }

}
