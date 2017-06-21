using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{

    enum MenuState
    {
        GameStart,
        StageSelect,
        Manual,
        Exit,
        Back,

        None
    }
    [SerializeField]
    private int m_MenuNum = 0;

    private float m_LowerAlpha = 1.0f;
    private float m_ChoiceLowerAlpha = 1.0f;

    [SerializeField, Tooltip("選択されていないα値を下げるスピードを設定")]
    private float m_LowerSpeed = 0.05f;
    [SerializeField, Tooltip("選択されたα値を下げるスピードを設定")]
    private float m_ChoiceLowerSpeed = 0.01f;

    private MenuState m_State;

    private Vector3 m_Scale;

    private RectTransform m_RectLeft;
    private RectTransform m_RectRight;
    private float m_Rate = 0f;

    private bool m_FrameMoveEnd;

    [SerializeField, Tooltip("決定した選択を何倍するか")]
    private float m_ScaleDouble = 1.0f;

    private bool m_IsStageSelect;

    // Use this for initialization
    void Start()
    {
        m_IsStageSelect = false;
        m_FrameMoveEnd = false;
        m_State = MenuState.None;
        m_RectLeft = GameObject.Find("menubackleft").GetComponent<RectTransform>();
        m_RectRight = GameObject.Find("menubackright").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("selectMenu").GetComponent<SelectMenuEnter>().GetMenuEnterIsEnd() == 5)
        {

            //メニュー選択番号の更新
            GetMenuNum();
            //メニュー決定
            MenuDecision();

            switch (m_MenuNum)
            {
                case 0:
                    if (m_State == MenuState.GameStart) return;
                    //大きさを変える
                    GameObject.Find("startgame").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                    GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                    //色を変える
                    GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(0, 1, 1);
                    GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    break;

                case 1:
                    //大きさを変える
                    if (m_State == MenuState.StageSelect) return;
                    GameObject.Find("selectstage").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                    GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                    //色を変える
                    GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(0, 1, 1);
                    GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    break;

                case 2:
                    if (m_State == MenuState.Manual) return;
                    //大きさを変える
                    GameObject.Find("manual").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                    GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                    //色を変える
                    GameObject.Find("manual").GetComponent<RawImage>().color = new Color(0, 1, 1);
                    GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    break;

                case 3:
                    if (m_State == MenuState.Exit) return;
                    //大きさを変える
                    GameObject.Find("exit").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                    GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                    //色を変える
                    GameObject.Find("exit").GetComponent<RawImage>().color = new Color(0, 1, 1);
                    GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    break;

                case 4:
                    if (m_State == MenuState.Back) return;
                    //大きさを変える
                    GameObject.Find("backMenu").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                    GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);

                    //色を変える
                    GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(0, 1, 1);
                    GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                    break;
            }
        }
    }

    private void GetMenuNum()
    {
        m_MenuNum = GameObject.Find("Stage00Manager").GetComponent<StageInputManager>().GetMenuNum();
    }

    private void MenuDecision()
    {
        switch (m_MenuNum)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.GameStart;
                }
                break;

            case 1:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.StageSelect;
                    m_Scale = GameObject.Find("selectstage").transform.localScale;
                }
                if (m_State == MenuState.StageSelect)
                {
                    if (m_LowerAlpha >= 0) m_LowerAlpha -= m_LowerSpeed;

                    if (m_ChoiceLowerAlpha >= 0)
                    {
                        m_ChoiceLowerAlpha -= m_ChoiceLowerSpeed;
                        m_Scale += (m_Scale * m_ScaleDouble) / 60;
                    }

                    GameObject.Find("selectstage").transform.localScale = new Vector3(m_Scale.x, m_Scale.y, m_Scale.z);
                    GameObject.Find("menuselectback1").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;
                    GameObject.Find("menuselectback2").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;
                    GameObject.Find("menuselectback3").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;
                    GameObject.Find("menuselectback4").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;


                    if (m_Rate <= 1.1f) m_Rate += 0.03f;
                    else if (m_Rate >= 1.1f) 
                    {
                        m_FrameMoveEnd = true;
                        m_IsStageSelect = true;
                        GameObject.Find("Canvas menu").GetComponent<CanvasGroup>().alpha = 0.0f;
                    }

                    m_RectLeft.localPosition = Vector3.Lerp(new Vector3(-200.0f, 0.0f, 0.0f), new Vector3(-255.0f, 0.0f, 0.0f), m_Rate);
                    m_RectRight.localPosition = Vector3.Lerp(new Vector3(200.0f, 0.0f, 0.0f), new Vector3(255.0f, 0.0f, 0.0f), m_Rate);
                }
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.Manual;
                }

                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.Exit;
                }
                break;
        }
    }


    public int GetMenuSelect()
    {
        return (int)m_State;
    }

    public bool GetFrameMoveEnd()
    {
        return m_FrameMoveEnd;
    }

    public bool IsStageSelect()
    {
        return m_IsStageSelect;
    }
}
