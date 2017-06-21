using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCanvas : MonoBehaviour {
    [SerializeField, Tooltip("α値を上げるスピードの設定")]
    private float m_HigherSpeed = 0.05f;

    private float m_Alpha = 0.0f;

    private bool m_IsStageSelectDraw = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("MenuManager").GetComponent<MenuManager>().GetMenuSelect() == 1)
        {
            m_Alpha = 1f;
            if (GameObject.Find("MenuManager").GetComponent<MenuManager>().GetFrameMoveEnd()) GameObject.Find("Canvas select").GetComponent<CanvasGroup>().alpha = m_Alpha;
        }
    }

    public bool GetIsStageDraw()
    {
        if (m_Alpha >= 1) return true;

        return false;
    }
}
