using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualCollection : MonoBehaviour
{

    [SerializeField, Tooltip("α値を上げるスピードの設定")]
    private float m_HigherSpeed = 0.05f;

    private float m_HigherAlpha = 0.0f;

    [SerializeField, Tooltip("α値を下げるスピードを設定")]
    private float m_LowerSpeed = 0.04f;

    private float m_LowerAlpha = 1.0f;

    private StickState m_State;
    private bool m_Once;

    private float m_Rate;

    private int m_ManualNum;

    private float m_FeadOutRate;

    private enum ManualState
    {
        Enter,
        InputStart,
        FeadOutManual,

        None
    }

    private ManualState m_ManualState;
    // Use this for initialization
    void Start()
    {
        m_Rate = 0.0f;
        m_Once = false;
        m_ManualNum = 0;
        m_HigherAlpha = 0.0f;
        m_FeadOutRate = 1.0f;
        m_LowerAlpha = 1.0f;
        m_ManualState = ManualState.Enter;

        GameObject.Find("sideFrame").GetComponent<MenuFrame>().InitializeSpreadRate();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_ManualState)
        {
            case ManualState.Enter:
                //バック
                BackManualEnter();

                //manual画面
                ManualEnter();
                break;

            case ManualState.InputStart:
                ManualBackStart();
                break;

            case ManualState.FeadOutManual:
                ManualFeadOut();
                BackManualFeadOut();
                break;
        }
    }

    private void BackManualEnter()
    {
        if (m_Rate <= 1) m_Rate += 0.03f;
        else m_ManualState = ManualState.InputStart;
        GameObject.Find("backbackManual").transform.localPosition = Vector3.Lerp(new Vector3(-350, -260, 0), new Vector3(-350, -196, 0), m_Rate);
    }

    private void ManualEnter()
    {
        m_HigherAlpha += 0.04f;
        GameObject.Find("manualback").GetComponent<CanvasGroup>().alpha = m_HigherAlpha;
    }

    private void ManualBackStart()
    {
        GameObject.Find("backManual").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        GameObject.Find("backManual").GetComponent<RawImage>().color = new Color(0, 1, 1);

        if (InputWrap())
        {
            SoundManager.Instance.PlaySe("back");
            m_ManualState = ManualState.FeadOutManual;
        }
    }

    private void BackManualFeadOut()
    {
        if (m_FeadOutRate >= 0) m_FeadOutRate -= 0.03f;
        else GameObject.Find("sideFrame").GetComponent<MenuFrame>().BackFrame();

        GameObject.Find("backbackManual").transform.localPosition = Vector3.Lerp(new Vector3(-350, -260, 0), new Vector3(-350, -196, 0), m_FeadOutRate);
    }

    private void ManualFeadOut()
    {
        m_LowerAlpha -= m_LowerSpeed;
        GameObject.Find("manualback").GetComponent<CanvasGroup>().alpha = m_LowerAlpha;
    }

    //ボタンの押されたとき
    private bool InputWrap()
    {
        int id = 0;

        if (Input.GetButtonDown("XBOXArm1"))
            id = 1;
        if (Input.GetButtonDown("XBOXArm2"))
            id = 2;
        if (Input.GetButtonDown("XBOXArm3"))
            id = 3;
        if (Input.GetButtonDown("XBOXArm4"))
            id = 4;

        if (id != 0)
            return true;

        return false;
    }
}
