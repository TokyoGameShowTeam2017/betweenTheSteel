using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleNoise : MonoBehaviour
{
    [SerializeField, Tooltip("α値を下げるスピードの設定")]
    private float m_LowerSpeed = 0.025f;

    private float m_Alpha = 1.0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NoiseFeadOut()
    {
        m_Alpha -= m_LowerSpeed;
        GameObject.Find("Titlenoise").GetComponent<CanvasGroup>().alpha = m_Alpha;
        if (m_Alpha <= 0)
        {
            m_Alpha = 0.0f;
        }
    }
}
