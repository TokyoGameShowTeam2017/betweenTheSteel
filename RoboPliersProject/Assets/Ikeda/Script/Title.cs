using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private RectTransform m_Rect1;
    private RectTransform m_Rect2;

    private Vector3 m_StartPosition1;
    private Vector3 m_StartPosition2;

    private float m_Rate = 0.0f;
    private float m_Alpha = 0.0f;

    [SerializeField, Tooltip("何秒後にタイトルを表示させるか")]
    private int m_Second = 3;

    [SerializeField, Tooltip("タイトルの枠の速さを設定(0～1)")]
    private float m_FrameSpeed = 0.01f;

    [SerializeField, Tooltip("α値を上げる速さの設定")]
    private float m_HigherSpeed = 0.01f;

    [SerializeField, Tooltip("α値を下げる速さの設定")]
    private float m_LowerSpeed = 0.025f;

    private float m_Timer;

    private bool m_TitleEnd = false;

    private bool m_RapidDraw = false;

    private float m_FedeOutRate;
    private float m_FedeOutAlpha;
    // Use this for initialization
    void Start()
    {
        m_Rect1 = transform.FindChild("title1").GetComponent<RectTransform>();
        m_Rect2 = transform.FindChild("title1 (1)").GetComponent<RectTransform>();
        m_StartPosition1 = transform.FindChild("title1").GetComponent<RectTransform>().localPosition;
        m_StartPosition2 = transform.FindChild("title1 (1)").GetComponent<RectTransform>().localPosition;

        m_RapidDraw = false;
        m_TitleEnd = false;
        m_Rate = 0.0f;
        m_Alpha = 0.0f;
        m_FedeOutAlpha = 1.0f;
        m_FedeOutRate = 1.0f;
    }


    public void TitleRapidFeadIn(bool rapidDraw)
    {
        if (rapidDraw)
        {
            m_Rate = 1.0f;
            m_Rect1.localPosition = Vector3.Lerp(m_StartPosition1, Vector3.zero, m_Rate);
            m_Rect2.localPosition = Vector3.Lerp(m_StartPosition2, Vector3.zero, m_Rate);
            transform.FindChild("title2").GetComponent<CanvasGroup>().alpha = 1.0f;
            transform.FindChild("title3").GetComponent<CanvasGroup>().alpha = 1.0f;
            m_RapidDraw = true;
        }
    }

    /// <summary>
    /// タイトルフェードインの処理
    /// </summary>
    public void TitleFadeIn()
    {
        if (m_Timer >= (m_Second * 60))
        {
            if (m_Rate <= 1)
            {
                m_Rate += m_FrameSpeed * Time.deltaTime * 60;
            }
            if (m_Rate >= 1)
            {
                transform.FindChild("title2").GetComponent<CanvasGroup>().alpha = 1.0f;
            }

            m_Rect1.localPosition = Vector3.Lerp(m_StartPosition1, Vector3.zero, m_Rate);
            m_Rect2.localPosition = Vector3.Lerp(m_StartPosition2, Vector3.zero, m_Rate);

            if (m_Rate >= 1)
            {
                if (m_Alpha < 1.0f) m_Alpha += m_HigherSpeed * Time.deltaTime * 60;
                else
                {
                    m_TitleEnd = true;
                    m_Alpha = 1.0f;
                }
                transform.FindChild("title3").GetComponent<CanvasGroup>().alpha = m_Alpha;
            }
            if (m_RapidDraw)
            {
                transform.FindChild("title3").GetComponent<CanvasGroup>().alpha = 1.0f;
            }
        }
        m_Timer++;
    }

    /// <summary>
    /// PRESS STARTを押されたとき(フェードアウト)の処理
    /// </summary>
    public void TitleFadeOut()
    {
        if (m_FedeOutAlpha > 0.01f) m_FedeOutAlpha -= m_LowerSpeed * Time.deltaTime * 60;


        m_FedeOutRate -= 0.1f * Time.deltaTime * 60;
        m_Rect1.localPosition = Vector3.Lerp(m_StartPosition1, Vector3.zero, m_FedeOutRate);
        m_Rect2.localPosition = Vector3.Lerp(m_StartPosition2, Vector3.zero, m_FedeOutRate);
        transform.FindChild("title2").GetComponent<CanvasGroup>().alpha = m_FedeOutAlpha;
        transform.FindChild("title3").GetComponent<CanvasGroup>().alpha = m_FedeOutAlpha;
    }


    public bool IsTitleEnd()
    {
        return m_TitleEnd;
    }

}
