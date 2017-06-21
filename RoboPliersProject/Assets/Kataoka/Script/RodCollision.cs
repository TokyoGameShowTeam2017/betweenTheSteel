using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodCollision : MonoBehaviour
{
     [SerializeField, Tooltip("動的に棒を壊す")]
    public bool m_IsBreakFlag;

     [SerializeField, Tooltip("強度")]
     private float m_Strength = 2.0f;
     [SerializeField, Tooltip("耐久値")]
     private float m_Life = 5.0f;

    // Use this for initialization
    void Start()
    {
        m_IsBreakFlag = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!m_IsBreakFlag)
        {
            //m_IsBreakFlag = true;
        }
        //
    }

    public bool GetIsBreakFlag()
    {
        return m_IsBreakFlag;
    }
    public void SetIsBreakFlag(bool flag)
    {
        m_IsBreakFlag = flag;
    }

    
    /// <summary>
    /// 強度を取得する
    /// </summary>
    public float GetStrength()
    {
        return m_Strength;
    }

    /// <summary>
    /// ペンチの挟む強さに応じて、耐久値にダメージを与える
    /// 挟む強さが強度より大きい場合にダメージが入る
    /// </summary>
    public void Damage(float pliersPower)
    {
        float damage = pliersPower - m_Strength;
        damage = Mathf.Clamp(damage, 0.0f, 10.0f);
        m_Life -= damage * Time.deltaTime;

        if (m_Life <= 0.0f)
            m_IsBreakFlag = true;

        print("Life:" + m_Life);
    }
}
