using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private TitleState m_TitleState = TitleState.None;

    // Use this for initialization
    void Start()
    {
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
                //黒い画像をフェードアウトさせる
                transform.FindChild("BlackImage").GetComponent<AlphaChanger>().BlackImageUpdate();

                //黒い画像がフェードアウトし終わったら
                if (transform.FindChild("BlackImage").GetComponent<AlphaChanger>().GetIsBlackImageEnd())
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
                //PressStartの入力
                PressStart();

                //PressStartの点滅の処理
                transform.FindChild("pressstartback").GetComponent<PressStart>().FlashingState();

                //PressStartが押されたらフェードアウトへ
                if (m_PressStart)
                {
                    m_TitleState = TitleState.TitleFeadOutState;
                }
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



    /// <summary>
    /// PRESS STARTが押されてメニュー画面に移行
    /// </summary>
    private void PressStart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_PressStart = true;
        }
    }


    /// <summary>
    /// タイトルとPRESS STARTを早く描画する
    /// </summary>
    private void RapidTitle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
}
