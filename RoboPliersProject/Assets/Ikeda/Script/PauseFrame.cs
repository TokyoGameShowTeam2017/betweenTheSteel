using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFrame : MonoBehaviour {

    private RectTransform m_RectRight;
    private RectTransform m_RectLeft;

    private Vector3 m_RightStartPosition;
    private Vector3 m_LeftStartPosition;

    private float m_EnterRate;
    private float m_SpreadRate;
    private float m_BackRate;

    // Use this for initialization
    void Start () {
        m_RectRight = transform.FindChild("sidebackright").GetComponent<RectTransform>();
        m_RectLeft = transform.FindChild("sidebackleft").GetComponent<RectTransform>();
        m_RightStartPosition = transform.FindChild("sidebackright").transform.localPosition;
        m_LeftStartPosition = transform.FindChild("sidebackleft").transform.localPosition;

        m_EnterRate = 0.0f;
        m_SpreadRate = 0.0f;
        m_BackRate = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void FrameEnter()
    {
        if (m_EnterRate <= 1.0f) m_EnterRate += 3.5f * Time.deltaTime;

        m_RectLeft.localPosition = Vector3.Lerp(m_LeftStartPosition, new Vector3(-230, 0, 0), m_EnterRate);
        m_RectRight.localPosition = Vector3.Lerp(m_RightStartPosition, new Vector3(230, 0, 0), m_EnterRate);
    }

    public void FrameSpread()
    {
        if (m_SpreadRate <= 1.0f) m_SpreadRate += 2.0f * Time.deltaTime;

        m_RectLeft.localPosition = Vector3.Lerp(new Vector3(-230, 0, 0), new Vector3(-330, 0, 0), m_SpreadRate);
        m_RectRight.localPosition = Vector3.Lerp(new Vector3(230, 0, 0), new Vector3(330, 0, 0), m_SpreadRate);
    }

    public void FrameBack()
    {
        if (m_BackRate >= 0.0f) m_BackRate -= 2.0f * Time.deltaTime;

        m_RectLeft.localPosition = Vector3.Lerp(new Vector3(-230, 0, 0), new Vector3(-330, 0, 0), m_BackRate);
        m_RectRight.localPosition = Vector3.Lerp(new Vector3(230, 0, 0), new Vector3(330, 0, 0), m_BackRate);
    }

    public bool GetFrameIsEnd()
    {
        if (m_EnterRate >= 1) return true;

        return false;
    }

    public bool GetSpreadIsEnd()
    {
        if (m_SpreadRate >= 1) return true;

        return false;
    }

    public bool GetBackIsEnd()
    {
        if (m_BackRate <= 0) return true;

        return false;
    }


    public void SpreadInitialize()
    {
        m_SpreadRate = 0.0f;
    }

    public void BackInitialize()
    {
        m_BackRate = 1.0f;
    }
}
