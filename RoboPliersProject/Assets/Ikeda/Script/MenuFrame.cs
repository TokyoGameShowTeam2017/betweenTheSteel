using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFrame : MonoBehaviour {

    private RectTransform m_RectLeft;
    private RectTransform m_RectRight;

    private Vector3 m_StartPositionLeft;
    private Vector3 m_StartPositionRight;

    [SerializeField, Tooltip("左右の枠の速さの設定")]
    private float m_FrameSpeed = 0.01f;

    private float m_Rate = 0.0f;


    // Use this for initialization
    void Start () {
        m_RectLeft = transform.FindChild("menubackleft").GetComponent<RectTransform>();
        m_RectRight = transform.FindChild("menubackright").GetComponent<RectTransform>();
        m_StartPositionLeft = transform.FindChild("menubackleft").GetComponent<RectTransform>().localPosition;
        m_StartPositionRight = transform.FindChild("menubackright").GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update () {
		if (GameObject.Find("Canvas menu").GetComponent<MenuCanvas>().GetIsMenuDraw())
        {
            if (m_Rate < 1)
            m_Rate += m_FrameSpeed;

            m_RectLeft.localPosition = Vector3.Lerp(m_StartPositionLeft, new Vector3(-200.0f, 0.0f, 0.0f), m_Rate);
            m_RectRight.localPosition = Vector3.Lerp(m_StartPositionRight, new Vector3(200.0f, 0.0f, 0.0f), m_Rate);
        }
    }

    public bool GetFrameIsEnd()
    {
        if (m_Rate >= 1) return true;

        return false;
    }
}
