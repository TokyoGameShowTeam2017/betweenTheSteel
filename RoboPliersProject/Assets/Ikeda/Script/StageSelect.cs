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
