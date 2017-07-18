using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseManual : MonoBehaviour
{

    private StickState m_State;
    private bool m_Once;
    private int m_ManualNum;
    private enum ManualState
    {
        Enter,
        InputStart,
        FeadOutManual,

        None
    }

    private ManualState m_ManualState;

    private float m_Rate;
    private float m_HigherAlpha;
    private float m_LowerAlpha;
    private float m_FeadOutRate;

    private bool m_IsEnd;

    [SerializeField]
    private GameObject m_ManualCanvas;
    private GameObject m_ManualCanvasInstace;

    // Use this for initialization
    void Start()
    {
        m_ManualState = ManualState.Enter;
        m_Rate = 0.0f;
        m_FeadOutRate = 1.0f;
        m_IsEnd = false;

        //GameObject.Find("sideFrame").GetComponent<PauseFrame>().SpreadInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        StartDestroy();

        switch (m_ManualState)
        {
            case ManualState.Enter:
                if (GameObject.Find("sideFrame").GetComponent<PauseFrame>().GetSpreadIsEnd())
                {
                    //バック
                    BackManualEnter();

                    //manual画面
                    ManualEnter();
                }
                break;

            case ManualState.InputStart:
                ManualInput();
                ManualUpDown();
                break;

            case ManualState.FeadOutManual:
                BackManualFeadOut();
                ManualFeadOut();
                break;
        }
    }

    private void BackManualEnter()
    {
        if (m_Rate <= 1) m_Rate += 3.5f * Time.deltaTime;
        else m_ManualState = ManualState.InputStart;
        GameObject.Find("backbackManual").transform.localPosition = Vector3.Lerp(new Vector3(-540, -420, 0), new Vector3(-540, -315, 0), m_Rate);
    }

    private void ManualEnter()
    {
        m_HigherAlpha += 0.04f * Time.deltaTime * 60;
        GameObject.Find("manualback").GetComponent<CanvasGroup>().alpha = m_HigherAlpha;

        if (m_ManualCanvasInstace == null)
        {
            m_ManualCanvasInstace = Instantiate(m_ManualCanvas);
        }
    }

    private void ManualUpDown()
    {
        //Aボタンで戻る
        if (Input.GetButtonDown("XBOXArm3") && !m_IsEnd)
        {
            m_IsEnd = true;
            m_ManualNum = 1;
        }

        if (m_ManualNum == 0)
        {
            GameObject.Find("backManual").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            GameObject.Find("backManual").GetComponent<RawImage>().color = new Color(1, 1, 1);
        }
        else if (m_ManualNum == 1)
        {
            GameObject.Find("backManual").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            GameObject.Find("backManual").GetComponent<RawImage>().color = new Color(0, 1, 1);
            if (InputManager.GetSelectArm().isDown || m_IsEnd)
            {
                SoundManager.Instance.PlaySe("back");
                if (m_ManualCanvasInstace != null)
                {
                    m_ManualCanvasInstace.GetComponent<ManualController>().CanvasDestroy();
                    m_ManualCanvasInstace = null;
                }
                m_ManualState = ManualState.FeadOutManual;
            }
        }
    }

    private void BackManualFeadOut()
    {
        if (m_FeadOutRate >= 0) m_FeadOutRate -= 3.5f * Time.deltaTime;
        else
        {
            GameObject.Find("PausePerformanceCanvas(Clone)").GetComponent<PausePerformance>().SetPauseState(PausePerformance.PauseState.BackPauseMenu);
            Destroy(gameObject);
        }
        GameObject.Find("backbackManual").transform.localPosition = Vector3.Lerp(new Vector3(-540, -420, 0), new Vector3(-540, -315, 0), m_FeadOutRate);
    }

    private void ManualFeadOut()
    {
        m_LowerAlpha -= 0.05f * Time.deltaTime * 60;
        GameObject.Find("manualback").GetComponent<CanvasGroup>().alpha = m_LowerAlpha;
    }


    private void StartDestroy()
    {
        //スタートボタンを押したら消える
        if (Input.GetButtonDown("XBOXStart"))
        {
            Destroy(gameObject);
            Destroy(m_ManualCanvasInstace);
        }
    }

    private void ManualInput()
    {
        m_State = GetStick();

        switch (m_State)
        {
            case StickState.Up:
                if (!m_Once)
                {
                    m_Once = true;
                    SoundManager.Instance.PlaySe("select");
                    if (m_ManualNum == 0)
                    {
                        m_ManualNum = 1;
                    }
                    else
                        m_ManualNum = 0;
                }
                break;
            case StickState.Down:
                if (!m_Once)
                {
                    m_Once = true;
                    SoundManager.Instance.PlaySe("select");
                    if (m_ManualNum == 1)
                    {
                        m_ManualNum = 0;
                    }
                    else
                        m_ManualNum = 1;
                }
                break;

            default:
                m_Once = false;
                break;
        }
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
