using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour {

    private float m_Alpha = 0.0f;
    private float m_HigherSpeed = 0.1f;
    private float m_Timer = 0.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.Find("RouteMoveObject").GetComponent<RouteMove>().GetIsGoal())
        {

            if (m_Timer <= 30.0f)
            {
                m_Timer++;
            }
            else
            {
                m_Alpha += m_HigherSpeed;
                GameObject.Find("NoiseCanvas").GetComponent<CanvasGroup>().alpha = m_Alpha;
            }
        }
    }
}
