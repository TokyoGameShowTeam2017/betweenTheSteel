using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingOnSwitchPush : MonoBehaviour {


    private Vector3 m_StartPosition;
    private Vector3 m_GoalPosition;

    private float m_Rate = 0.0f;

    private bool m_Repeat = false;

    [SerializeField]
    private GameObject m_MoveObject;

    // Use this for initialization
    void Start () {
        m_Rate = 0.0f;
        m_Repeat = false;

        m_StartPosition = transform.localPosition;
        m_GoalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.035f, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update () {
        if (GameObject.Find("SteppingSwitch3").GetComponent<SteppingOnSwitch>().GetIsCollide() &&
            !m_Repeat)
        {
            //MoveObjectを動かす
            m_MoveObject.GetComponent<MoveObject>().isMotion = true;

            m_Repeat = true;
        }
        else if (!GameObject.Find("SteppingSwitch3").GetComponent<SteppingOnSwitch>().GetIsCollide() &&
                 m_Repeat)
        {
            //MoveObjectを動かす
            m_MoveObject.GetComponent<MoveObject>().isMotion = true;
            m_Rate = 1.0f;
            m_Repeat = false;
        }

        if (m_Repeat)
        {
            //スイッチを動かす
            if (m_Rate <= 1.0f) m_Rate += 0.04f;
            transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Rate);
        }
        else
        {
            //スイッチを動かす
            if (m_Rate >= 0.0f) m_Rate -= 0.04f;
            transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Rate);
        }
    }
}
