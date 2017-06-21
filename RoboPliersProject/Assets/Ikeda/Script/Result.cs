using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{

    public bool m_IsResult = false;

    public bool m_IsEnd = false;

    private int m_Count = 60;

    private int m_ResultCount = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsResult)
        {
            transform.FindChild("ResultText").gameObject.SetActive(true);

            m_Count++;

            if (m_Count > 60 * 3)
            {
                switch (m_ResultCount)
                {
                    case 0:
                        transform.FindChild("StageDamege").gameObject.SetActive(true);
                        break;

                    case 60:
                        transform.FindChild("ClearTime").gameObject.SetActive(true);
                        break;

                    case 120:
                        transform.FindChild("Walk").gameObject.SetActive(true);
                        transform.FindChild("Button").gameObject.SetActive(true);
                        break;
                }
                m_ResultCount++;            
            }
        }

        if (m_IsEnd)
        {
            NonActiveResult();
            m_IsEnd = false;
            m_IsResult = false;
        }
    }

    /// <summary>
    /// リザルトを非表示にする
    /// </summary>
    private void NonActiveResult()
    {
        transform.FindChild("ResultText").gameObject.SetActive(false);
        transform.FindChild("StageDamege").gameObject.SetActive(false);
        transform.FindChild("ClearTime").gameObject.SetActive(false);
        transform.FindChild("Walk").gameObject.SetActive(false);
        transform.FindChild("Button").gameObject.SetActive(false);

    }
}