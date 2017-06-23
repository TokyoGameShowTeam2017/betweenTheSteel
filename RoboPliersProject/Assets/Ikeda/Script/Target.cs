using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {


    private const string MAIN_CAMERA = "MainCamera";

    private bool m_IsRendered = false;

    // Use this for initialization
    void Start () {
        m_IsRendered = true;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void OnWillRenderObject()
    {
        if (Camera.current.tag == MAIN_CAMERA)
        {
            m_IsRendered = true;
        }
        else
        {
            m_IsRendered = false;
        }
    }

    public bool GetIsRenderer()
    {
        return m_IsRendered;
    }

    public void SetIsRenderer(bool l_bool)
    {
        m_IsRendered = l_bool;
    }
}
