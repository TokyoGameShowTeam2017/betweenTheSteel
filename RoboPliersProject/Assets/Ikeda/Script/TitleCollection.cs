﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCollection : MonoBehaviour
{
    enum TitleState
    {
        TitleStart,
        TitleWaitState,
        TitleFeadOutState,

        None
    }


    private bool m_IsSceneEnd = false;

    private bool m_RapidDraw = false;
    private bool m_PressStart = false;

    [SerializeField, Tooltip("何フレーム無操作状態を作る")]
    private float m_WaitTimer = 30.0f;
    [SerializeField, Tooltip("何フレーム無操作状態を作る")]
    private float m_WaitTimer2 = 30.0f;

    private TitleState m_TitleState = TitleState.None;
    private bool m_One = false;
    // Use this for initialization
    void Start()
    {
        m_One = false;
        m_IsSceneEnd = false;
        m_RapidDraw = false;
        m_PressStart = false;
        m_TitleState = TitleState.TitleStart;

    }

    //タイトルの更新
    void Update()
    {
        switch (m_TitleState)
        {
            case TitleState.TitleStart:
                if (m_WaitTimer / 60 >= 0)
                    m_WaitTimer -= Time.deltaTime;

                else
                {
                    //早く描画させる入力
                    RapidTitle();
                    //早く描画させる処理
                    transform.FindChild("title").GetComponent<Title>().TitleRapidFeadIn(m_RapidDraw);
                    transform.FindChild("pressstartback").GetComponent<PressStart>().PressStartRapidDraw(m_RapidDraw);


                    //タイトルの描画
                    transform.FindChild("title").GetComponent<Title>().TitleFadeIn();
                    //PressStartの描画
                    transform.FindChild("pressstartback").GetComponent<PressStart>().PressStartDraw();
                }
                break;

            case TitleState.TitleWaitState:
                if (m_WaitTimer2 / 60 >= 0)
                    m_WaitTimer2 -= Time.deltaTime;
                else
                {
                    //PressStartの入力
                    PressStart();
                    //PressStartが押されたらフェードアウトへ
                    if (m_PressStart)
                    {
                        m_TitleState = TitleState.TitleFeadOutState;
                    }
                }
                //PressStartの点滅の処理
                transform.FindChild("pressstartback").GetComponent<PressStart>().FlashingState();
                break;

            case TitleState.TitleFeadOutState:
                //タイトルのフェードアウト
                transform.FindChild("title").GetComponent<Title>().TitleFadeOut();

                //PressStartのフェードアウト
                transform.FindChild("pressstartback").GetComponent<PressStart>().PressStartFadeOut();

                //Noise
                transform.FindChild("Titlenoise").GetComponent<TitleNoise>().NoiseFeadOut();
                break;
        }
    }

    private IEnumerator IsPlayerCheck()
    {
        while (true)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                break;
            }
            yield return null;
        }
        if (!m_One)
        {
            SoundManager.Instance.PlayBgm("Monday1");
            m_One = true;
        }
    }


    public void LateUpdate()
    {
        StartCoroutine(IsPlayerCheck());
    }




    /// <summary>
    /// PRESS STARTが押されてメニュー画面に移行
    /// </summary>
    private void PressStart()
    {
        if (InputWrap())
        {
            SoundManager.Instance.PlaySe("enter");
            m_PressStart = true;
        }
    }


    /// <summary>
    /// タイトルとPRESS STARTを早く描画する
    /// </summary>
    private void RapidTitle()
    {
        if (InputWrap())
        {
            SoundManager.Instance.PlaySe("enter");
            m_RapidDraw = true;
        }
    }

    /// <summary>
    /// タイトルStateの変更
    /// </summary>
    /// <param name="state"></param>
    public void SetTitleState(int state)
    {
        m_TitleState = (TitleState)state;
    }

    public bool GetPressStart()
    {
        return m_PressStart;
    }

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
        if (Input.GetButtonDown("XBOXStart"))
            id = 5;


        if (id != 0)
            return true;

        return false;
    }
}

