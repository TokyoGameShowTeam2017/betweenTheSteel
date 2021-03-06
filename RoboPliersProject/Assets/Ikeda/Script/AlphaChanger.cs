﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChanger : MonoBehaviour
{

    private CanvasGroup m_Canvas;

    [SerializeField, Tooltip("α値を下げるスピードの設定")]
    private float m_LowerSpeed = 0.03f;

    private float m_Alpha = 1.0f;

    private bool m_IsBlackImageEnd = false;

    // Use this for initialization
    void Start()
    {
        m_IsBlackImageEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BlackImageUpdate()
    {
        m_Alpha -= m_LowerSpeed;
        m_Canvas = GameObject.Find("BlackImage").GetComponent<CanvasGroup>();
        m_Canvas.GetComponent<CanvasGroup>().alpha = m_Alpha;
        if (m_Alpha <= 0)
        {
            m_Alpha = 0.0f;
            m_IsBlackImageEnd = true;
        }
    }

    public bool GetIsBlackImageEnd()
    {
        return m_IsBlackImageEnd;
    }
}
