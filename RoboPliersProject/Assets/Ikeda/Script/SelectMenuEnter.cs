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

    private float m_StartGameRate = 0.0f;
    private float m_StageSelectRate = 0.0f;
    private float m_ManualRate = 0.0f;
    private float m_ExitRate = 0.0f;
    private float m_BackRate = 0.0f;





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
        m_State = MenuState.StartGame;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MenusEnter()
    {
        if (GameObject.Find("sideFrame").GetComponent<MenuFrame>().GetFrameIsEnd())
        {
            switch (m_State)
            {
                case MenuState.StartGame:
                    if (m_StartGameRate <= 1.0f) m_StartGameRate += m_Speed;
                    else
                    {
                        m_State = MenuState.StageSelect;
                    }
                    transform.FindChild("menuselectback1").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, 132, 0), m_StartGameRate);
                    break;

                case MenuState.StageSelect:
                    if (m_StageSelectRate <= 1.0f) m_StageSelectRate += m_Speed;
                    else
                    {
                        m_State = MenuState.Manual;
                    }
                    m_StageSelectRate += m_Speed;
                    transform.FindChild("menuselectback2").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, 50, 0), m_StageSelectRate);
                    
                    break;

                case MenuState.Manual:
                    if (m_ManualRate <= 1.0f) m_ManualRate += m_Speed;
                    else
                    {
                        m_State = MenuState.Exit;
                    }
                    m_ManualRate += m_Speed;
                    transform.FindChild("menuselectback3").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, -31, 0), m_ManualRate);
                    
                    break;

                case MenuState.Exit:
                    if (m_ExitRate <= 1.0f) m_ExitRate += m_Speed;
                    else
                    {
                        m_State = MenuState.Back;
                    }
                    m_ExitRate += m_Speed;
                    transform.FindChild("menuselectback4").localPosition = Vector3.Lerp(m_MenusStartPosition, new Vector3(0, -114, 0), m_ExitRate);
                    
                    break;

                case MenuState.Back:
                    if (m_BackRate <= 1.0f) m_BackRate += m_Speed;
                    else
                    {
                        GameObject.Find("Canvas menu(Clone)").GetComponent<MenuCollection>().SetSceneState(1);
                        m_State = MenuState.None;
                    }
                    m_BackRate += m_Speed;
                    GameObject.Find("backbackMenu").transform.localPosition = Vector3.Lerp(new Vector3(-350, -260, 0), new Vector3(-350, -196, 0), m_BackRate);
                    
                    break;
            }
        }
    }
}
