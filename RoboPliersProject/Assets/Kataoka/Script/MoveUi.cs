using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveUi : MonoBehaviour {

    private RectTransform m_Rect;
    private ParameterUiRay m_RayUi;

    private bool mIsDraw;
	// Use this for initialization
	void Start () {
        m_Rect = GetComponent<RectTransform>();
        m_RayUi = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ParameterUiRay>();
        m_Rect.position = Vector3.zero;
        mIsDraw = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!mIsDraw) {
            GetComponent<CanvasGroup>().alpha = 0.0f;
            return; 
        }
        if(GameObject.FindGameObjectWithTag("RawCamera")!=null)
        m_Rect.position = RectTransformUtility.WorldToScreenPoint(GameObject.FindGameObjectWithTag("RawCamera").GetComponent<Camera>(), m_RayUi.GetColRayPos());
        GetComponent<CanvasGroup>().alpha = 1.0f;
        mIsDraw = false;
    }
    public void DrawUi()
    {
        mIsDraw = true;
    }
}
