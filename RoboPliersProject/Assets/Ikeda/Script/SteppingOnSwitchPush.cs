using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingOnSwitchPush : MonoBehaviour {


    private Vector3 m_StartPosition;
    private Vector3 m_GoalPosition;

    private float m_Rate = 0.0f;

    private bool m_Repeat = false;

    [SerializeField]
    private GameObject m_Switch;

    // Use this for initialization
    void Start () {
        m_Rate = 0.0f;
        m_Repeat = false;

        m_StartPosition = transform.localPosition;
        m_GoalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.035f, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update () {

        if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
        {
            //スイッチを動かす
            if (m_Rate <= 1.0f) m_Rate += 0.04f * Time.deltaTime * 60;
            transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Rate);
        }
        if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
        {
            //スイッチを動かす
            if (m_Rate >= 0.0f) m_Rate -= 0.04f * Time.deltaTime * 60;
            transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Rate);
        }
    }
}
