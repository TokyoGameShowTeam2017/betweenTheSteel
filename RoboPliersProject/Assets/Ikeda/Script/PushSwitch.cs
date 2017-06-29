using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushSwitch : MonoBehaviour
{
    private enum SwitchState
    {
        PushWaitState,
        PushSwitchState,
        PlayerStayState,

        None
    }


    [SerializeField, Tooltip("スイッチの速さを設定する")]
    private float m_Speed = 0.04f;

    [SerializeField, Tooltip("戻す場合チェックを外す")]
    private bool m_KeepPush = true;

    //0～1の比率
    private float m_Ratio = 0.0f;

    //switchを押されたか
    private bool m_IsPush = false;

    //繰り返されたか
    private bool m_Repeat = false;

    private SwitchState m_SwitchState = SwitchState.None;

    [SerializeField]
    private GameObject m_Switch3;

    private Vector3 m_StartPosition;
    private Vector3 m_GoalPosition;

    private float m_FTimer;
    [SerializeField, Tooltip("何フレーム後にスイッチを戻すか")]
    private float m_FpsTime;

    // Use this for initialization
    void Start()
    {
        m_IsPush = false;
        m_SwitchState = SwitchState.PushWaitState;
        m_StartPosition = transform.localPosition;
        m_GoalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.035f, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {

        //スイッチが押されたら
        if (IsPush())
        {
            //スイッチが戻らない場合
            if (m_KeepPush)
            {
                //スイッチを動かす
                m_Ratio += m_Speed * Time.deltaTime * 60;
                transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Ratio);

                //一定距離動いたら、止まる
                if (m_Ratio >= 1.0f)
                {
                    m_Ratio = 1.0f;
                    m_IsPush = false;
                }
            }
            //スイッチが戻る場合
            else
            {
                //プレイヤーに押されるのを待機状態
                if (m_SwitchState == SwitchState.PushWaitState)
                {
                    //プレイヤーがスイッチに触れたとき
                    if (m_Switch3.GetComponent<ButtonObject>().m_IsTouch)
                        m_SwitchState = SwitchState.PushSwitchState;
                }

                //スイッチを動かす状態
                else if (m_SwitchState == SwitchState.PushSwitchState)
                {
                    m_Ratio += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, m_GoalPosition, m_Ratio);

                    if (m_Ratio >= 1.0f && !m_Repeat)
                    {
                        m_SwitchState = SwitchState.PlayerStayState;
                    }
                    else if (m_Ratio <= 0.0f && m_Repeat)
                    {
                        m_Repeat = false;
                        m_Speed *= -1;
                        m_SwitchState = SwitchState.PushWaitState;
                        m_IsPush = false;
                    }

                }

                //プレイヤーが触れ続けている状態
                else if (m_SwitchState == SwitchState.PlayerStayState)
                {
                    //プレイヤーがスイッチから離れたとき
                    if (!m_Switch3.GetComponent<ButtonObject>().m_IsTouch)
                    {
                        m_FTimer += Time.deltaTime;
                        if (m_FTimer >= m_FpsTime)
                        {
                            m_FTimer = 0;
                            m_Speed *= -1;
                            m_Repeat = true;
                            m_SwitchState = SwitchState.PushSwitchState;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// スイッチが押されているかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsPush()
    {
        if (m_Switch3.GetComponent<ButtonObject>().GetOnSwitchEnter())
        {
            m_IsPush = true;
        }
        return m_IsPush;
    }
}
