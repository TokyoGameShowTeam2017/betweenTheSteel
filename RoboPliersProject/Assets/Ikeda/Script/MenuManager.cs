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
