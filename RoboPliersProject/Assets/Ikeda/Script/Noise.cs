using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{

    private float m_Alpha = 0.0f;
    private float m_HigherSpeed = 0.1f;
    private float m_Timer = 0.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("SceneCollection").GetComponent<SceneCollection>().GetSceneState() == 0)
        {
            GameObject.Find("Noises").GetComponent<CanvasGroup>().alpha = 0.0f;
        }
    }

    public void NoiseEnter()
    {
        if (GameObject.Find("RouteMoveObject").GetComponent<RouteMove>().GetIsGoal())
        {
            if (m_Timer <= 30.0f / 60.0f)
            {
                m_Timer += Time.deltaTime;
            }
            else
            {
                m_Alpha += m_HigherSpeed * Time.deltaTime * 60;
                GameObject.Find("Noises").GetComponent<CanvasGroup>().alpha = m_Alpha;
            }
        }
    }
}
