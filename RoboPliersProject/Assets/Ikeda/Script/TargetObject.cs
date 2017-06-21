using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour {
    enum RouteState
    {
        DoNothing,     //何もしない
        LookAround     //見渡す
    }

    [SerializeField, Tooltip("何もしないか、見渡すかを設定")]
    private RouteState m_RouteState = RouteState.DoNothing;

    [SerializeField, Tooltip("最初に見渡す角度を設定")]
    private float m_FirstAngle = 0;

    [SerializeField, Tooltip("次に見渡す角度を設定")]
    private float m_SecondAngle = 0;

    [SerializeField, Tooltip("見渡す回数を設定")]
    private int m_NumberOfTimes = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetTargetState()
    {
        return (int)m_RouteState;
    }

    public float GetFirstAngle()
    {
        return m_FirstAngle;
    }

    public float GetSecondAngle()
    {
        return m_SecondAngle;
    }

    public int GetTimes()
    {
        return m_NumberOfTimes;
    }
}
