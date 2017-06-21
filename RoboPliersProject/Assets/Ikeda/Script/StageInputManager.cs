using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInputManager : MonoBehaviour {

    //タイトルとPRESS STARTを早く描画
    private bool m_SpeedDraw = false;

    //ボタンが押されたかどうか
    private bool m_IsPressButton = false;

    //選択されている番号
    private int m_MenuNum = 0;

    private int m_BeflorNum;

	// Use this for initialization
	void Start () {
        m_SpeedDraw = false;
	}
	
	// Update is called once per frame
	void Update () {

        //タイトルとPRESS STARTを早く描画する
        RapidTitle();

        //PRESS STARTが押されてメニュー画面に移行
        PressStart();

        //メニュー画面選択中
        MenuSelect();
    }


    /// <summary>
    /// PRESS STARTが押されてメニュー画面に移行
    /// </summary>
    private void PressStart()
    {
        if (GameObject.Find("pressstartback").GetComponent<PressStart>().GetPressState() == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_IsPressButton = true;
            }
        }
    }


    /// <summary>
    /// タイトルとPRESS STARTを早く描画する
    /// </summary>
    private void RapidTitle()
    {
        if (GameObject.Find("BlackImage").GetComponent<AlphaChanger>().GetIsEnd())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_SpeedDraw = true;
            }
        }
    }

    /// <summary>
    /// メニュー画面選択中
    /// </summary>
    private void MenuSelect()
    {
        if (GameObject.Find("Canvas menu").GetComponent<MenuCanvas>().GetIsMenuDraw() &&
            !GameObject.Find("MenuManager").GetComponent<MenuManager>().IsStageSelect())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_MenuNum = m_MenuNum - 1;
                if (m_MenuNum - 1 < -1)
                {
                    m_MenuNum = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (m_MenuNum == 4) return;
                m_MenuNum = m_MenuNum + 1;
                if (m_MenuNum + 1 > 4)
                {
                    m_MenuNum = 3;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_BeflorNum = m_MenuNum;
                m_MenuNum = 4;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_MenuNum =m_BeflorNum;
            }


        }
    }

    /// <summary>
    /// タイトルとPRESS STARTを早く描画するかを返す
    /// </summary>
    /// <returns></returns>
    public bool GetSpeedDraw()
    {
        return m_SpeedDraw;
    }

    /// <summary>
    /// ボタンが押されたかどうかを返す
    /// </summary>
    /// <returns></returns>
    public bool GetPressButton()
    {
        return m_IsPressButton;
    }

    /// <summary>
    /// メニューで選択されている番号を返す
    /// </summary>
    /// <returns></returns>
    public int GetMenuNum()
    {
        return m_MenuNum;
    }
}
