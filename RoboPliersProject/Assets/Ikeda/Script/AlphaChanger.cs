using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChanger : MonoBehaviour {

    private CanvasGroup m_Canvas;

    [SerializeField, Tooltip("α値を下げるスピードの設定")]
    private float m_LowerSpeed = 0.01f;

    private float m_Alpha = 1.0f;

    // Use this for initialization
    void Start () {
        m_Alpha = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        m_Alpha -= m_LowerSpeed;
        m_Canvas = GameObject.Find("BlackImage").GetComponent<CanvasGroup>();
        m_Canvas.GetComponent<CanvasGroup>().alpha = m_Alpha;       
    }

    public bool GetIsEnd()
    {
        if (m_Alpha <= 0)
        {
            return true;
        }

        return false;
    }
}
