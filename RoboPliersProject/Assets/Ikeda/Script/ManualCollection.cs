using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualCollection : MonoBehaviour {

    [SerializeField, Tooltip("α値を上げるスピードの設定")]
    private float m_HigherSpeed = 0.05f;

    private float m_HigherAlpha = 0.0f;

    [SerializeField, Tooltip("α値を下げるスピードを設定")]
    private float m_LowerSpeed = 0.04f;
    
    private float m_LowerAlpha = 1.0f;

    private StickState m_State;
    private bool m_Once;

    private float m_Rate;
    [SerializeField]
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
    void Start () {
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
    void Update () {
        switch (m_ManualState)
        {
            case ManualState.Enter:
                //バック
                BackManualEnter();

                //manual画面
                ManualEnter();
                break;

            case ManualState.InputStart:
                ManualSelect();
                ManualInputStart();
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
        GameObject.Find("controller").GetComponent<CanvasGroup>().alpha = m_HigherAlpha;
    }

    private void ManualInputStart()
    {
        if (m_ManualNum == 0)
        {
            GameObject.Find("backManual").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            GameObject.Find("backManual").GetComponent<RawImage>().color = new Color(1, 1, 1);
        }

        if (m_ManualNum == 1)
        {
            GameObject.Find("backManual").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            GameObject.Find("backManual").GetComponent<RawImage>().color = new Color(0, 1, 1);

            if (InputWrap())
            {
                m_ManualState = ManualState.FeadOutManual;
            }
        }
    }

    private void ManualSelect()
    {
        m_State = GetStick();

        switch (m_State)
        {
            case StickState.Up:
                if (!m_Once)
                {                   
                    m_Once = true;
                    m_ManualNum = 0;
                }
                break;

            case StickState.Down:
                if (!m_Once)
                {
                    m_Once = true;
                    m_ManualNum = 1;
                }
                break;

            case StickState.Left:
                if (!m_Once)
                {
                    m_Once = true;
                    m_ManualNum = 1;
                }
                break;


            default:
                m_Once = false;
                break;
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
        GameObject.Find("controller").GetComponent<CanvasGroup>().alpha = m_LowerAlpha;
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

    public static Vector2 GetMove()
    {
        float h = Input.GetAxis("XBOXLeftStickH");
        float v = Input.GetAxis("XBOXLeftStickV");

        Vector2 vec = new Vector2(h, v);
        if (vec.magnitude <= 0.0f)
        {
            h = Input.GetAxis("XBOXLeftStickH");
            v = Input.GetAxis("XBOXLeftStickV");
            vec = new Vector2(h, v);
        }

        return vec;
    }

    public static StickState GetStick()
    {
        float vecX = GetMove().x;
        float vecY = GetMove().y;

        if (vecX > 0.3f) return StickState.Right;
        if (vecX < -0.3f) return StickState.Left;
        if (vecY > 0.3f) return StickState.Up;
        if (vecY < -0.3f) return StickState.Down;

        return StickState.None;
    }
}
