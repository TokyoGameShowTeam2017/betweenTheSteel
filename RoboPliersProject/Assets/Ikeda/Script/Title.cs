using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{

    enum TitleState
    {
        TitleFadeIn,
        TitleFadeOut,

        None
    }


    private RectTransform m_Rect1;
    private RectTransform m_Rect2;

    private Vector3 m_StartPosition1;
    private Vector3 m_StartPosition2;

    private float m_Rate = 0.0f;
    private float m_Alpha = 0.0f;
    private float m_LowerAlpha = 1.0f;

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

    private TitleState m_State = TitleState.TitleFadeIn;

    // Use this for initialization
    void Start()
    {
        m_Rect1 = transform.FindChild("title1").GetComponent<RectTransform>();
        m_Rect2 = transform.FindChild("title1 (1)").GetComponent<RectTransform>();
        m_StartPosition1 = transform.FindChild("title1").GetComponent<RectTransform>().localPosition;
        m_StartPosition2 = transform.FindChild("title1 (1)").GetComponent<RectTransform>().localPosition;

        m_LowerAlpha = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_State)
        {
            case TitleState.TitleFadeIn:
                if (GameObject.Find("BlackImage").GetComponent<AlphaChanger>().GetIsEnd())
                {
                    if (m_Timer >= (m_Second * 60) || GameObject.Find("Stage00Manager").GetComponent<StageInputManager>().GetSpeedDraw())
                    {
                        if (m_Rate <= 1)
                        {
                            m_Rate += m_FrameSpeed;
                        }
                        if (m_Rate >= 1)
                        {
                            transform.FindChild("title2").GetComponent<CanvasGroup>().alpha = 1.0f;
                        }

                        m_Rect1.localPosition = Vector3.Lerp(m_StartPosition1, Vector3.zero, m_Rate);
                        m_Rect2.localPosition = Vector3.Lerp(m_StartPosition2, Vector3.zero, m_Rate);

                        if (m_Rate >= 1)
                        {
                            if (m_Alpha < 1.0f) m_Alpha += m_HigherSpeed;
                            else
                            {
                                m_TitleEnd = true;
                                m_Alpha = 1.0f;
                                m_State = TitleState.TitleFadeOut;
                            }
                            transform.FindChild("title3").GetComponent<CanvasGroup>().alpha = m_Alpha;
                        }
                    }
                    m_Timer++;
                }
                break;


            case TitleState.TitleFadeOut:
                //PRESS STARTを押されたとき
                if (GameObject.Find("pressstartback").GetComponent<PressStart>().GetPressState() == 2)
                {
                    if (m_Alpha > 0.01f) m_Alpha -= m_LowerSpeed;
                    else
                    {
                        m_State = TitleState.None;
                    }
                    transform.FindChild("title1").GetComponent<CanvasGroup>().alpha = m_Alpha;
                    transform.FindChild("title1 (1)").GetComponent<CanvasGroup>().alpha = m_Alpha;
                    transform.FindChild("title2").GetComponent<CanvasGroup>().alpha = m_Alpha;
                    transform.FindChild("title3").GetComponent<CanvasGroup>().alpha = m_Alpha;
                }
                break;


            default:
                GameObject.Find("Canvas title").GetComponent<CanvasGroup>().alpha = 0.0f;
                break;
        }
    }


    public bool IsTitleEnd()
    {
        return m_TitleEnd;
    }
}
