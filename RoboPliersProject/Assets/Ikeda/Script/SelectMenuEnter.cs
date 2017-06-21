using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenuEnter : MonoBehaviour
{

    enum MenuState
    {
        StartGame,
        StageSelect,
        Manual,
        Exit,
        Back,

        None
    }

    private Vector3 m_MenusStartPosition;

    private float m_Rate = 0.0f;
    private float m_Timer = 0.0f;
    [SerializeField]
    private float m_Speed = 0.025f;

    [SerializeField, Tooltip("順番に上がるときの間の時間(秒)")]
    private float m_CoolTime;

    private MenuState m_State;

    // Use this for initialization
    void Start()
    {
        m_MenusStartPosition = transform.FindChild("menuselectback1").localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("menuFrame").GetComponent<MenuFrame>().GetFrameIsEnd())
        {
            switch (m_State)
            {
                case MenuState.StartGame:
                    if (m_Rate <= 1.0f) m_Rate += m_Speed;
                    else m_State = MenuState.StageSelect;

                    transform.FindChild("menuselectback1").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, 132, 0), m_Rate);
                    break;

                case MenuState.StageSelect:
                    m_Timer++;
                    if (m_Timer >= (m_CoolTime * 60))
                    {
                        if (m_Rate <= 1.0f) m_Rate += m_Speed;
                        else
                        {
                            m_Timer = 0.0f;
                            m_State = MenuState.Manual;
                        }
                        m_Rate += m_Speed;
                        transform.FindChild("menuselectback2").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, 50, 0), m_Rate);
                    }
                    else m_Rate = 0.0f;
                    break;

                case MenuState.Manual:
                    m_Timer++;
                    if (m_Timer >= (m_CoolTime * 60))
                    {
                        if (m_Rate <= 1.0f) m_Rate += m_Speed;
                        else
                        {
                            m_Timer = 0.0f;
                            m_State = MenuState.Exit;
                        }
                        m_Rate += m_Speed;
                        transform.FindChild("menuselectback3").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, -31, 0), m_Rate);
                    }
                    else m_Rate = 0.0f;
                    break;

                case MenuState.Exit:
                    m_Timer++;
                    if (m_Timer >= (m_CoolTime * 60))
                    {
                        if (m_Rate <= 1.0f) m_Rate += m_Speed;
                        else
                        {
                            m_Timer = 0.0f;
                            m_State = MenuState.Back;
                        }
                        m_Rate += m_Speed;
                        transform.FindChild("menuselectback4").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, -114, 0), m_Rate);
                    }
                    else m_Rate = 0.0f;
                    break;

                case MenuState.Back:
                    m_Timer++;
                    if (m_Timer >= (m_CoolTime * 60))
                    {
                        if (m_Rate <= 1.0f) m_Rate += m_Speed;
                        else
                        {
                            m_Timer = 0.0f;
                            m_State = MenuState.None;
                        }
                        m_Rate += m_Speed;
                        GameObject.Find("backbackMenu").transform.localPosition = Vector3.Lerp(new Vector3(-350, -260, 0), new Vector3(-350, -196, 0), m_Rate);
                    }
                    else m_Rate = 0.0f;
                    break;
            }
        }
    }

    public int GetMenuEnterIsEnd()
    {
        return (int)m_State;
    }
}
