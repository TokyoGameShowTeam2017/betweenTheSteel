using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{

    [SerializeField, Tooltip("動かしたいObjectを入れる")]
    private GameObject m_MoveTheObject;

    [SerializeField]
    private bool m_OnSwitchEnter = false;

    public bool m_IsTouch = false;

    // Use this for initialization
    void Start()
    {
        m_IsTouch = false;
    }

    // Update is called once per frame
    void Update()
    {
        //オブジェクトが動き終わったら
        if (m_MoveTheObject.GetComponent<MoveObject>().IsMoveEnd())
        {
            m_OnSwitchEnter = false;
            m_MoveTheObject.GetComponent<MoveObject>().MoveEnd();
        }
        //プレイヤーがスイッチに触れたら
        if (m_OnSwitchEnter && m_IsTouch)
        {
            m_MoveTheObject.GetComponent<MoveObject>().isMotion = true;
        }
    }


    //プレイヤーがスイッチに触れる
    public void OnTriggerEnter(Collider other)
    {
            m_OnSwitchEnter = true;
            m_IsTouch = true;
    }

    //プレイヤーがスイッチから離れる
    public void OnTriggerExit(Collider other)
    {
            m_IsTouch = false;
    }

    /******************************************************************/
    public bool GetOnSwitchEnter()
    {
        return m_OnSwitchEnter;
    }

    public void SetOnSwitchEnter(bool logical)
    {
        m_OnSwitchEnter = logical;
    }
}