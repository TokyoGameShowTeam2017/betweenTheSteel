using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{

    private float m_Timer = 0.0f;

    [SerializeField, Tooltip("α値を上げるスピードの設定")]
    private float m_HigherSpeed = 0.05f;

    private float m_Alpha = 0.0f;
    private float m_StartAlpha = 1.0f;

    // Use this for initialization
    void Start()
    {
        m_Alpha = 0.0f;
        m_StartAlpha = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Alpha >= 1.0f) return;
        if (GameObject.Find("RouteMoveObject").GetComponent<RouteMove>().GetIsGoal())
        {
            if (m_Timer <= 30.0f)
            {
                m_Timer++;
            }
            else
            {
                m_Alpha += m_HigherSpeed;
                GameObject.Find("CommonCanvas").GetComponent<CanvasGroup>().alpha = m_Alpha;
            }
        }
    }

    /// <summary>
    /// メニューを表示し終わったか
    /// </summary>
    /// <returns></returns>
    public bool GetIsMenuDraw()
    {
        if (m_Alpha <= 1.0f) return false;
        return true;
    }
}
