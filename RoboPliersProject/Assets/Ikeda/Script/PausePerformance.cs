using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePerformance : MonoBehaviour {

    private enum PauseEnterState
    {
        BackToGame,
        SelectStage,
        Manual,
        BackToTitle,
        BackTo,

        None
    }

    public enum PauseState
    {
        Enter,
        DecisionWait,
        FrameOut,
        BackPauseMenu,

        None
    }

    private Vector3 m_StartPosition;
    private PauseEnterState m_InOutState = PauseEnterState.BackToGame;
    private PauseState m_State = PauseState.Enter;

    private bool m_Decision;

    private float m_BackToGameRate;
    private float m_SelectStageRate;
    private float m_ManualRate;
    private float m_BackToTitleRate;
    private float m_BackToRate;

    // Use this for initialization
    void Start () {
        m_Decision = false;
        m_StartPosition = GameObject.Find("menuselectback1").GetComponent<RectTransform>().localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        //スタートボタンを押したら消える
        StartDestroy();

        switch (m_State)
        {
            case PauseState.Enter:
                //フレームの入り
                GameObject.Find("sideFrame").GetComponent<PauseFrame>().FrameEnter();
                //フレームが動き終わったら
                if (GameObject.Find("sideFrame").GetComponent<PauseFrame>().GetFrameIsEnd())
                {
                    //ポーズ画面の入り
                    PauseEnter();
                }
                break;

            case PauseState.DecisionWait:
                SelectDecision();
                if (m_Decision)
                {
                    m_InOutState = PauseEnterState.BackTo;
                    m_State = PauseState.FrameOut;
                    GameObject.Find("sideFrame").GetComponent<PauseFrame>().BackInitialize();
                    GameObject.Find("sideFrame").GetComponent<PauseFrame>().SpreadInitialize();
                }
                break;

            case PauseState.FrameOut:
                PauseOut();
                if (m_InOutState == PauseEnterState.None)
                {
                    m_Decision = false;
                    m_InOutState = PauseEnterState.BackToGame;
                    GameObject.Find("sideFrame").GetComponent<PauseFrame>().FrameSpread();
                }
                break;

            case PauseState.BackPauseMenu:
                GameObject.Find("sideFrame").GetComponent<PauseFrame>().FrameBack();
                if (GameObject.Find("sideFrame").GetComponent<PauseFrame>().GetBackIsEnd())
                PauseEnter();
                break;
        }
	}

    private void SelectDecision()
    {
        if (GameObject.Find("PauseCanvas(Clone)").GetComponent<PauseController>().GetSelectNum() == 1 ||
            GameObject.Find("PauseCanvas(Clone)").GetComponent<PauseController>().GetSelectNum() == 2)
        {
            if (InputWrap())
            {
                m_Decision = true;
            }
        }
    }

    //ポーズ画面の入り
    private void PauseEnter()
    {
        switch (m_InOutState)
        {
            case PauseEnterState.BackToGame:
                if (m_BackToGameRate < 1) m_BackToGameRate += 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.SelectStage;

                GameObject.Find("menuselectback1").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, 210.0f, 0.0f), m_BackToGameRate);
                break;

            case PauseEnterState.SelectStage:
                if (m_SelectStageRate < 1) m_SelectStageRate += 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.Manual;

                GameObject.Find("menuselectback2").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, 80.0f, 0.0f), m_SelectStageRate);
                break;
            case PauseEnterState.Manual:
                if (m_ManualRate < 1) m_ManualRate += 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.BackToTitle;

                GameObject.Find("menuselectback3").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, -50.0f, 0.0f), m_ManualRate);
                break;
            case PauseEnterState.BackToTitle:
                if (m_BackToTitleRate < 1) m_BackToTitleRate += 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.BackTo;

                GameObject.Find("menuselectback4").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, -180.0f, 0.0f), m_BackToTitleRate);
                break;
            case PauseEnterState.BackTo:
                if (m_BackToRate < 1) m_BackToRate += 4.5f * Time.deltaTime;
                else {
                    m_InOutState = PauseEnterState.None;
                    m_State = PauseState.DecisionWait;
                }
                GameObject.Find("menuselectback5").GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(-540, -450, 0), new Vector3(-540.0f, -315.0f, 0.0f), m_BackToRate);
                break;
        }
    }

    private void PauseOut()
    {
        switch (m_InOutState)
        {
            case PauseEnterState.BackTo:
                if (m_BackToRate > 0) m_BackToRate -= 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.BackToTitle;
                GameObject.Find("menuselectback5").GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(-540, -450, 0), new Vector3(-540.0f, -315.0f, 0.0f), m_BackToRate);
                break;

            case PauseEnterState.BackToTitle:
                if (m_BackToTitleRate > 0) m_BackToTitleRate -= 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.Manual;
                GameObject.Find("menuselectback4").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, -180.0f, 0.0f), m_BackToTitleRate);
                break;

            case PauseEnterState.Manual:
                if (m_ManualRate > 0) m_ManualRate -= 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.SelectStage;
                GameObject.Find("menuselectback3").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, -50.0f, 0.0f), m_ManualRate);
                break;

            case PauseEnterState.SelectStage:
                if (m_SelectStageRate > 0) m_SelectStageRate -= 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.BackToGame;
                GameObject.Find("menuselectback2").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, 80.0f, 0.0f), m_SelectStageRate);
                break;

            case PauseEnterState.BackToGame:
                if (m_BackToGameRate > 0) m_BackToGameRate -= 4.5f * Time.deltaTime;
                else m_InOutState = PauseEnterState.None;
                GameObject.Find("menuselectback1").GetComponent<RectTransform>().localPosition = Vector3.Lerp(m_StartPosition, new Vector3(3.0f, 210.0f, 0.0f), m_BackToGameRate);
                break;
        }
    }

    private void StartDestroy()
    {
        //スタートボタンを押したら消える
        if (Input.GetButtonDown("XBOXStart"))
        {
            Destroy(gameObject);
        }
    }

    public void EnterInitialize()
    {
        m_BackToGameRate  = 0.0f;
        m_SelectStageRate = 0.0f;
        m_ManualRate      = 0.0f;
        m_BackToTitleRate = 0.0f;
        m_BackToRate      = 0.0f;
    }

    public PauseState GetPauseState()
    {
        return m_State;
    }

    public void SetPauseState(PauseState state)
    {
        m_State = state;
    }

    //ボタンの押されたとき
    private bool InputWrap()
    {
        int id = 0;

        //if (Input.GetButtonDown("XBOXArm1"))
        //    id = 1;
        if (Input.GetButtonDown("XBOXArm2"))
            id = 2;
        //if (Input.GetButtonDown("XBOXArm3"))
        //    id = 3;
        //if (Input.GetButtonDown("XBOXArm4"))
        //    id = 4;

        if (id != 0)
            return true;

        return false;
    }

}
