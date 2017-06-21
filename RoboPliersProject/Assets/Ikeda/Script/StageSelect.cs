using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour {
    [SerializeField]
    private int m_StageNum;

    private Vector3 m_BeforPosition;

    [SerializeField]
    private float m_SelectSpeed = 0.01f;
    [SerializeField]
    private float m_Rotae = 0.0f;

    RectTransform m_StageSelect;

    private bool m_IsLoad = false;

	// Use this for initialization
	void Start () {
        m_StageNum = 1;
    }

    // Update is called once per frame
    void Update () {
        ////ステージ選択中
        StageSelectInput();

        switch (m_StageNum)
        {

            case 1:
                m_StageSelect = GameObject.Find("Stages").GetComponent<RectTransform>();
                m_StageSelect.localPosition = new Vector3(180, 0, 0);
                GameObject.Find("stage01").transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                GameObject.Find("stage01").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                GameObject.Find("stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                GameObject.Find("stage03").transform.localScale = new Vector3(0f, 0f, 0f);
                break;

            case 2:
                m_StageSelect = GameObject.Find("Stages").GetComponent<RectTransform>();
                m_StageSelect.localPosition = new Vector3(0, 0, 0);
                GameObject.Find("stage02").transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                GameObject.Find("stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                GameObject.Find("stage01").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("stage01").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                GameObject.Find("stage03").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("stage03").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;

            case 3:
                m_StageSelect = GameObject.Find("Stages").GetComponent<RectTransform>();
                m_StageSelect.localPosition = new Vector3(-190, 0, 0);
                GameObject.Find("stage03").transform.localScale = new Vector3(0.5f, 0.5f, 0f);
                GameObject.Find("stage03").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                GameObject.Find("stage02").transform.localScale = new Vector3(0.3f, 0.3f, 0f);
                GameObject.Find("stage02").GetComponent<RawImage>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                GameObject.Find("stage01").transform.localScale = new Vector3(0f, 0f, 0f);
                break;
        }

    }


    private void StageSelectInput()
    {
        if (GameObject.Find("MenuManager").GetComponent<MenuManager>().GetMenuSelect() == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_StageNum -= 1;
                m_IsLoad = false;
                m_BeforPosition = transform.position;
                if (m_StageNum - 1 < 0)
                {
                    m_StageNum = 3;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_StageNum += 1;
                m_IsLoad = false;
                m_BeforPosition = transform.position;
                if (m_StageNum + 1 > 4)
                {
                    m_StageNum = 1;
                }
            }
        }
    }

    public bool GetIsLoad()
    {
        return m_IsLoad;
    }

    /// <summary>
    /// 今選択されているStage番号を返す
    /// </summary>
    /// <returns></returns>
    public int GetStageSelectNum()
    {
        return m_StageNum;
    } 

    public void SetLoad(bool l_bool)
    {
        m_IsLoad = l_bool;
    }
}
