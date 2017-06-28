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
    private float m_FeadOutRate;
    private StickState m_StickState;
    // Use this for initialization
    void Start()
    {
        m_FeadOutRate = 1.0f;
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
        m_StickState = GetStick();

        switch (m_StickState)
        {
            case StickState.Up:
                if (!m_Once)
                {
                    m_Once = true;
                    m_StageNum = m_BeforStageNum;
                    SoundManager.Instance.PlaySe("select");
                }
                break;

            case StickState.Down:
                if (!m_Once)
                {
                    m_Once = true;
                    if (m_StageNum == 20) return;
                    m_BeforStageNum = m_StageNum;
                    m_StageNum = 20;
                    SoundManager.Instance.PlaySe("select");
                }
                break;

            case StickState.Right:
                if (!m_Once)
                {
                    m_Once = true;
                    if (m_StageNum == 20) return;
                    m_StageNum += 1;
                    SoundManager.Instance.PlaySe("select");
                    m_IsLoad = false;
                    if (m_StageNum + 1 > 16)
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
                    SoundManager.Instance.PlaySe("select");
                    m_IsLoad = false;
                    if (m_StageNum - 1 < 0)
                    {
                        m_StageNum = 15;
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
        if (m_StageNum == 1)
        {
            GameObject.Find("Stage015").GetComponent<RectTransform>().localPosition = new Vector3(-180, 145, 0);
            GameObject.Find("Stage015").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage015").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage02").GetComponent<RectTransform>().localPosition = new Vector3(180, 145, 0);
            GameObject.Find("Stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage014").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);
            GameObject.Find("Stage03").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);

            GameObject.Find("Stage0" + m_StageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RectTransform>().localPosition = new Vector3(0, 145, 0);
        }

        else if (m_StageNum == 2)
        {
            GameObject.Find("Stage01").GetComponent<RectTransform>().localPosition = new Vector3(-180, 145, 0);
            GameObject.Find("Stage01").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage01").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage03").GetComponent<RectTransform>().localPosition = new Vector3(180, 145, 0);
            GameObject.Find("Stage03").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage03").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage015").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);
            GameObject.Find("Stage04").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);

            GameObject.Find("Stage0" + m_StageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RectTransform>().localPosition = new Vector3(0, 145, 0);
        }
        else if (m_StageNum == 14)
        {
            GameObject.Find("Stage013").GetComponent<RectTransform>().localPosition = new Vector3(-180, 145, 0);
            GameObject.Find("Stage013").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage013").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage015").GetComponent<RectTransform>().localPosition = new Vector3(180, 145, 0);
            GameObject.Find("Stage015").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage015").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage01").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);
            GameObject.Find("Stage012").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);

            GameObject.Find("Stage0" + m_StageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RectTransform>().localPosition = new Vector3(0, 145, 0);

        }
        else if (m_StageNum == 15)
        {
            GameObject.Find("Stage014").GetComponent<RectTransform>().localPosition = new Vector3(-180, 145, 0);
            GameObject.Find("Stage014").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage014").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage01").GetComponent<RectTransform>().localPosition = new Vector3(180, 145, 0);
            GameObject.Find("Stage01").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage01").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage013").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);
            GameObject.Find("Stage02").GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);

            GameObject.Find("Stage0" + m_StageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RectTransform>().localPosition = new Vector3(0, 145, 0);
        }
        else if (m_StageNum != 20)
        {
            GameObject.Find("Stage0" + (m_StageNum - 1)).GetComponent<RectTransform>().localPosition = new Vector3(-180, 145, 0);
            GameObject.Find("Stage0" + (m_StageNum - 1)).transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage0" + (m_StageNum - 1)).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage0" + (m_StageNum + 1)).GetComponent<RectTransform>().localPosition = new Vector3(180, 145, 0);
            GameObject.Find("Stage0" + (m_StageNum + 1)).transform.localScale = new Vector3(0.3f, 0.3f, 0f);
            GameObject.Find("Stage0" + (m_StageNum + 1)).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            GameObject.Find("Stage0" + (m_StageNum - 2)).GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);
            GameObject.Find("Stage0" + (m_StageNum + 2)).GetComponent<RectTransform>().localPosition = new Vector3(0, 250, 0);

            GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(1, 1, 1);
            GameObject.Find("backSelect").transform.localScale = new Vector3(1f, 1f, 1f);
            GameObject.Find("Stage0" + m_StageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GameObject.Find("Stage0" + m_StageNum).GetComponent<RectTransform>().localPosition = new Vector3(0, 145, 0);
        }

        if (m_StageNum == 20)
        {
            BackMenu();
            GameObject.Find("backSelect").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            GameObject.Find("backSelect").GetComponent<RawImage>().color = new Color(0, 1, 1);

            GameObject.Find("Stage0" + m_BeforStageNum).transform.localScale = new Vector3(0.5f, 0.5f, 0f);
            GameObject.Find("Stage0" + m_BeforStageNum).GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    //メニュー画面へ戻る
    private void BackMenu()
    {
        if (InputWrap() || Input.GetKeyDown(KeyCode.Space))
        {
            m_BackMenu = true;
            SoundManager.Instance.PlaySe("back");
            StageMapUnLoad();
        }
        if (m_BackMenu)
        {
            if (m_FeadOutRate >= 0) m_FeadOutRate -= 0.03f;
            else GameObject.Find("sideFrame").GetComponent<MenuFrame>().BackFrame();

            GameObject.Find("backback").transform.localPosition = Vector3.Lerp(new Vector3(-350, -260, 0), new Vector3(-350, -196, 0), m_FeadOutRate);
        }
    }

    //ステージを始める
    private void StartStaeMap()
    {
        if (m_StageNum == 20) return;
        if (InputWrap())
        {
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlaySe("enter");
            GameObject.Find("RotationOrigin").GetComponent<StageSelectMap>().StartOtherScene(m_StageNum);
        }
    }

    //ボタンの押されたとき
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

        if (id != 0)
            return true;

        return false;
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
