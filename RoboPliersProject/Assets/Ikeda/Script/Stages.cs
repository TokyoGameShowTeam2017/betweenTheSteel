using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stages : MonoBehaviour
{
    [SerializeField, Tooltip("α値を上げるスピードの設定")]
    private float m_HigherSpeed = 0.05f;

    private float m_Alpha = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StagesDraw()
    {
        if (m_Alpha <= 1) m_Alpha += m_HigherSpeed;
        GameObject.Find("Stages").GetComponent<CanvasGroup>().alpha = m_Alpha;
    }
}
