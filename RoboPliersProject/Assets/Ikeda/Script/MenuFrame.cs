using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFrame : MonoBehaviour
{

    private RectTransform m_RectLeft;
    private RectTransform m_RectRight;

    private Vector3 m_StartPositionLeft;
    private Vector3 m_StartPositionRight;

    [SerializeField, Tooltip("左右の枠の速さの設定")]
    private float m_FrameSpeed = 0.08f;

    private float m_EnterRate;
    private float m_SpreadRate;
    private float m_BackRate;

    // Use this for initialization
    void Start()
    {
        m_EnterRate = 0.0f;
        m_SpreadRate = 0.0f;
        m_BackRate = 1.0f;

        m_RectLeft = transform.FindChild("sidebackleft").GetComponent<RectTransform>();
        m_RectRight = transform.FindChild("sidebackright").GetComponent<RectTransform>();
        m_StartPositionLeft = transform.FindChild("sidebackleft").GetComponent<RectTransform>().localPosition;
        m_StartPositionRight = transform.FindChild("sidebackright").GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("SceneCollection").GetComponent<SceneCollection>().GetSceneState() == 0)
        {
            m_EnterRate = 0.0f;
            m_RectLeft.localPosition = Vector3.Lerp(m_StartPositionLeft, new Vector3(-230.0f, 0.0f, 0.0f), m_EnterRate);
            m_RectRight.localPosition = Vector3.Lerp(m_StartPositionRight, new Vector3(230.0f, 0.0f, 0.0f), m_EnterRate);
        }

        //GameStartが押された時
        if (GameObject.Find("SceneCollection").GetComponent<SceneCollection>().GetSceneState() == 1)
        {
            if (GameObject.Find("Canvas menu(Clone)").GetComponent<MenuCollection>().GetMenuState() == 0 ||
                GameObject.Find("Canvas menu(Clone)").GetComponent<MenuCollection>().GetMenuState() == 3)
            {
                if (m_EnterRate > 0)
                    m_EnterRate -= 0.1f;

                m_RectLeft.localPosition = Vector3.Lerp(m_StartPositionLeft, new Vector3(-230.0f, 0.0f, 0.0f), m_EnterRate);
                m_RectRight.localPosition = Vector3.Lerp(m_StartPositionRight, new Vector3(230.0f, 0.0f, 0.0f), m_EnterRate);
            }
            //メニューからタイトルへ戻るとき
            else if (GameObject.Find("Canvas menu(Clone)").GetComponent<MenuCollection>().GetMenuState() == 4)
            {
                if (GameObject.Find("selectMenu").GetComponent<SelectMenuEnter>().GetIsEndOut())
                {
                    if (m_EnterRate > 0) m_EnterRate -= 0.1f;
                    else
                    {
                        GameObject.Find("SceneCollection").GetComponent<SceneCollection>().SetNextScene(0);
                        GameObject.Find("SceneCollection").GetComponent<SceneCollection>().IsEndScene(true);
                    }
                    m_RectLeft.localPosition = Vector3.Lerp(m_StartPositionLeft, new Vector3(-230.0f, 0.0f, 0.0f), m_EnterRate);
                    m_RectRight.localPosition = Vector3.Lerp(m_StartPositionRight, new Vector3(230.0f, 0.0f, 0.0f), m_EnterRate);
                }
            }
        }
    }

    public void FrameEnter()
    {
        if (GameObject.Find("CommonCanvas").GetComponent<MenuCanvas>().GetIsMenuDraw())
        {
            if (m_EnterRate < 1)
                m_EnterRate += m_FrameSpeed;

            m_RectLeft.localPosition = Vector3.Lerp(m_StartPositionLeft, new Vector3(-230.0f, 0.0f, 0.0f), m_EnterRate);
            m_RectRight.localPosition = Vector3.Lerp(m_StartPositionRight, new Vector3(230.0f, 0.0f, 0.0f), m_EnterRate);
        }
    }

    public void SpreadFrame()
    {
        if (m_SpreadRate <= 1.1f) m_SpreadRate += 0.03f;
        else if (m_SpreadRate >= 1.0f)
        {
            if (GameObject.Find("Canvas menu(Clone)").GetComponent<MenuCollection>().GetMenuState() == 1)
            {
                GameObject.Find("SceneCollection").GetComponent<SceneCollection>().SetNextScene(2);
                GameObject.Find("SceneCollection").GetComponent<SceneCollection>().IsEndScene(true);
            }
            else
            {
                GameObject.Find("SceneCollection").GetComponent<SceneCollection>().SetNextScene(3);
                GameObject.Find("SceneCollection").GetComponent<SceneCollection>().IsEndScene(true);
            }
        }

        m_RectLeft.localPosition = Vector3.Lerp(new Vector3(-230.0f, 0.0f, 0.0f), new Vector3(-270.0f, 0.0f, 0.0f), m_SpreadRate);
        m_RectRight.localPosition = Vector3.Lerp(new Vector3(230.0f, 0.0f, 0.0f), new Vector3(270.0f, 0.0f, 0.0f), m_SpreadRate);
    }

    public void BackFrame()
    {
        if (m_BackRate >= 0) m_BackRate -= 0.03f;
        else if (m_BackRate <= 0)
        {
            GameObject.Find("SceneCollection").GetComponent<SceneCollection>().SetNextScene(1);
            GameObject.Find("SceneCollection").GetComponent<SceneCollection>().IsEndScene(true);
        }

        m_RectLeft.localPosition = Vector3.Lerp(new Vector3(-230.0f, 0.0f, 0.0f), new Vector3(-270.0f, 0.0f, 0.0f), m_BackRate);
        m_RectRight.localPosition = Vector3.Lerp(new Vector3(230.0f, 0.0f, 0.0f), new Vector3(270.0f, 0.0f, 0.0f), m_BackRate);
    }

    public bool GetFrameIsEnd()
    {
        if (m_EnterRate >= 1) return true;

        return false;
    }




    public void InitializeSpreadRate()
    {
        m_SpreadRate = 0.0f;
    }

    public void InitializeBackRate()
    {
        m_BackRate = 1.0f;
    }
}
