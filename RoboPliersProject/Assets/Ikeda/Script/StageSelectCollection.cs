using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectCollection : MonoBehaviour
{

    [SerializeField]
    private int m_StageNum;

    private bool m_IsLoad = false;
    private int m_BeforStageNum;
    private RectTransform m_StageSelect;
    private bool m_BackMenu = false;

    private bool m_Once;
    private StickState m_StickState;
    // Use this for initialization
    void Start()
    {
        m_Once = false;
        m_IsLoad = false;
        m_BackMenu = false;
        m_StageNum = 1;
        m_StickState = StickState.None;
        GameObject.Find("sideFrame").GetComponent<MenuFrame>().InitializeSpreadRate();
    }

    // Update is called once per frame
    void Update()
    {
        //選択中のインプット
        StageSelectInput();

        //ステージのマップをロード
        StageMapLoad();

        //ステージを始める
        StartStaeMap();

        //ステージ選択中
        StageSelect();
    }

    private void StageMapLoad()
    {
        if (!m_IsLoad)
        {
            GameObject.Find("RotationOrigin").GetComponent<StageSelectMap>().LoadScene(m_StageNum);
            m_IsLoad = true;
        }
    }

    private void StageMapUnLoad()
    {
        GameObject.Find("RotationOrigin").GetComponent<StageSelectMap>().LoadScene(-1);
    }

    //選択中のインプット関係
    private void StageSelectInput()
    {
        m_StickState = InputManager.GetStick();

        switch (m_StickState)
        {
            case StickState.Up:
                if (!m_Once)
                {
                    m_Once = true;
                    m_StageNum = m_BeforStageNum;
                }
                break;

            case StickState.Down:
                if (!m_Once)
                {
                    m_Once = true;
                    if (m_StageNum == 20) return;
                    m_BeforStageNum = m_StageNum;
                    m_StageNum = 20;
                }
                break;

            case StickState.Right:
                if (!m_Once)
                {
                    m_Once = true;
                    if (m_StageNum == 20) return;
                    m_StageNum += 1;
                    m_IsLoad = false;
                    if (m_StageNum + 1 > 13)
                    {
                        m_StageNum = 1;
                    }
                }
                break;

            case StickState.Left:
                if (!m_Once)
                {
                    m_Once = true;
                    if (m_StageNum == 20) return;
                    m_StageNum -= 1;
                    m_IsLoad = false;
                    if (m_StageNum - 1 < 0)
                    {
                        m_StageNum = 12;
                    }
                }
                break;

            default:
                m_Once = false;
                break;
        }
    }

    //ステージ選択
    private void StageSelect()
    {
        if (m_StageNum != 20)
        {
            GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(1, 1, 1);
            GameObject.Find("backSelect").transform.localScale = new Vector3(1f, 1f, 1f);
        }

        switch (m_StageNum)
        {
            case 1:
                m_StageSelect = GameObject.Find("Stages").GetComponent<RectTransform>();
                m_StageSelect.localPosition = new Vector3(180, 0, 0);

                GameObject.Find("Stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("Stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                GameObject.Find("Stage03").transform.localScale = new Vector3(0f, 0f, 0f);

                //GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(1, 1, 1);
                //GameObject.Find("backSelect").transform.localScale = new Vector3(1f, 1f, 1f);
                break;

            case 2:
                m_StageSelect = GameObject.Find("Stages").GetComponent<RectTransform>();
                m_StageSelect.localPosition = new Vector3(0, 0, 0);

                GameObject.Find("Stage01").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("Stage01").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                GameObject.Find("Stage03").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("Stage03").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                //GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(1, 1, 1);
                //GameObject.Find("backSelect").transform.localScale = new Vector3(1f, 1f, 1f);
                break;

            case 3:
                m_StageSelect = GameObject.Find("Stages").GetComponent<RectTransform>();
                m_StageSelect.localPosition = new Vector3(-190, 0, 0);

                GameObject.Find("Stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("Stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                GameObject.Find("Stage01").transform.localScale = new Vector3(0f, 0f, 0f);

                //GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(1, 1, 1);
                //GameObject.Find("backSelect").transform.localScale = new Vector3(1f, 1f, 1f);
                break;

            case 20:
                BackMenu();

                GameObject.Find("backSelect").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(0, 1, 1);

                GameObject.Find("Stage0" + m_BeforStageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                GameObject.Find("Stage0" + m_BeforStageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                if (m_BeforStageNum == 1)
                {
                    GameObject.Find("Stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                    GameObject.Find("Stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    GameObject.Find("Stage03").transform.localScale = new Vector3(0f, 0f, 0f);
                }
                else if (m_BeforStageNum == 2)
                {
                    GameObject.Find("Stage01").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                    GameObject.Find("Stage01").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    GameObject.Find("Stage03").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                    GameObject.Find("Stage03").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                }
                else if (m_BeforStageNum == 3)
                {
                    GameObject.Find("Stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                    GameObject.Find("Stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    GameObject.Find("Stage01").transform.localScale = new Vector3(0f, 0f, 0f);
                }
                break;
        }

        //バックの時以外は大きくして白くする
        //if (m_StageNum == 20) return;
        GameObject.Find("Stage0" + m_StageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        GameObject.Find("Stage0" + m_StageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    //メニュー画面へ戻る
    private void BackMenu()
    {
        if (InputManager.GetSelectArm().isDown || Input.GetKeyDown(KeyCode.Space))
        {
            m_BackMenu = true;
            StageMapUnLoad();
        }
        if (m_BackMenu)
        {
            GameObject.Find("sideFrame").GetComponent<MenuFrame>().BackFrame();
        }
    }

    //ステージを始める
    private void StartStaeMap()
    {
        if (m_StageNum == 20) return;
        if (InputManager.GetSelectArm().isDown)
        {
            GameObject.Find("RotationOrigin").GetComponent<StageSelectMap>().StartOtherScene(m_StageNum);
        }
    }
}
