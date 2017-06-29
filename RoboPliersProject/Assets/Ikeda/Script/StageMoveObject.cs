using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMoveObject : MonoBehaviour {

    enum DroneState
    {
        PatrolState,
        SurveillanceState,
        GoalMoveState,

        None
    }

    [SerializeField, Tooltip("速さを設定")]
    private float m_Speed = 5.0f;

    [SerializeField]
    private GameObject[] TargetObjects;


    private DroneState m_DroneState;

    private int m_TargetObjCount = 0;
    private int m_Times = 0;
    private int m_TargetTime = 0;

    private float m_FirstAngle = 0;
    private float m_SecondAngle = 0;
    private float m_BeforeAngle_Y;
    private float m_Ratio = 0;

    private bool m_IsAngleEnd = true;
    private bool m_IsOnce = false;

    private Vector3 m_StartPos;
    private Quaternion m_StartAngle;


    // Use this for initialization
    void Start()
    {
        m_Ratio = 0;
        m_IsAngleEnd = true;
        m_IsOnce = false;
        m_DroneState = DroneState.PatrolState;

    }

    // Update is called once per frame
    void Update()
    {
        switch (m_DroneState)
        {
            //巡回中
            case DroneState.PatrolState:
                Vector3 relativePos = TargetObjects[m_TargetObjCount].transform.position - this.transform.position;
                transform.Translate(relativePos.normalized * m_Speed * Time.deltaTime, Space.World);
                relativePos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1.0f);
                //目標に近づいたら
                if (Vector3.Distance(transform.position, TargetObjects[m_TargetObjCount].transform.position) <= 0.15f)
                {
                    m_FirstAngle = GetFirstAngle();
                    m_SecondAngle = GetSecondAngle();
                    m_TargetTime = GetTimes();
                    m_BeforeAngle_Y = transform.localEulerAngles.y;
                    ArrivedProcessing();
                }
                break;

            //監視中(きょろきょろ見渡す)
            case DroneState.SurveillanceState:
                if (m_Times + 1 <= m_TargetTime)
                {
                    if (m_IsAngleEnd && !m_IsOnce)
                    {
                        m_IsOnce = true;
                        LeanTween.rotateY(gameObject, m_BeforeAngle_Y + m_FirstAngle, 2f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(() =>
                        {
                            m_Times++;
                            m_IsAngleEnd = false;
                        });
                    }
                    else if (!m_IsAngleEnd && m_IsOnce)
                    {
                        m_IsOnce = false;
                        LeanTween.rotateY(gameObject, m_BeforeAngle_Y + m_SecondAngle, 2f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(() =>
                        {
                            m_Times++;
                            m_IsAngleEnd = true;
                        });
                    }
                }
                //全てが終わったら巡回中へ
                else
                {
                    m_Times = 0;
                    m_IsAngleEnd = true;
                    m_IsOnce = false;
                    m_DroneState = DroneState.PatrolState;
                }
                break;
        }
    }


    /// <summary>
    /// 目標に近づいたときの処理
    /// </summary>
    private void ArrivedProcessing()
    {
        if (TargetObjects[m_TargetObjCount].GetComponent<TargetObject>().GetTargetState() == 0)
        {
            m_DroneState = DroneState.PatrolState;
        }
        else
        {
            m_DroneState = DroneState.SurveillanceState;
        }

        AddTargetObjCount();
    }

    /// <summary>
    /// TargetObjCountに加える
    /// </summary>
    private void AddTargetObjCount()
    {
        if (m_TargetObjCount + 1 < TargetObjects.Length)
        {
            m_TargetObjCount++;
            /* 何も入ってなかったときに0に戻す */
            if (TargetObjects[m_TargetObjCount] == null) m_TargetObjCount = 0;
        }
        else
        {
            m_TargetObjCount = 0;
        }
    }

    /// <summary>
    /// ターゲットで決めた最初の角度を返す
    /// </summary>
    /// <returns></returns>
    private float GetFirstAngle()
    {
        return TargetObjects[m_TargetObjCount].GetComponent<TargetObject>().GetFirstAngle();
    }

    /// <summary>
    /// ターゲットで決めた二番目の角度を返す
    /// </summary>
    /// <returns></returns>
    private float GetSecondAngle()
    {
        return TargetObjects[m_TargetObjCount].GetComponent<TargetObject>().GetSecondAngle();
    }

    /// <summary>
    /// ターゲットで決めた回数を返す
    /// </summary>
    /// <returns></returns>
    private int GetTimes()
    {
        return TargetObjects[m_TargetObjCount].GetComponent<TargetObject>().GetTimes();
    }


    /// <summary>
    /// 巡回ルートの描画
    /// </summary>
    public void OnDrawGizmos()
    {
        if (TargetObjects != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f);

            for (int i = 0; i < TargetObjects.Length; i++)
            {
                int startIndex = i;
                int endIndex = i + 1;

                if (endIndex == TargetObjects.Length) endIndex = 0;

                Gizmos.DrawLine(TargetObjects[startIndex].transform.position, TargetObjects[endIndex].transform.position);
            }
        }
    }
}
