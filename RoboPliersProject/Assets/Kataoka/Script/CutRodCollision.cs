using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutRodCollision : MonoBehaviour
{
    [SerializeField, Tooltip("動的に壊す")]
    public bool m_isBreak=false;
    [SerializeField, Tooltip("強度")]
    private float m_Strength = 2.0f;
    [SerializeField, Tooltip("耐久値")]
    private float m_Life = 5.0f;

    private GameObject m_Rod;
    private float m_StartLife;

    // Use this for initialization
    void Start()
    {
        m_StartLife = m_Life;
        if (GetComponent<RodTurnBone>()!=null)
        m_Rod = GetComponent<RodTurnBone>().GetRod();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //壊す
    public void IsBreak()
    {
        m_isBreak = true;
    }


    /// <summary>
    /// 強度を取得する
    /// </summary>
    public float GetStrength()
    {
        return m_Strength;
    }

    /// <summary>
    /// ペンチの挟む強さに応じて、耐久値にダメージを与える 破壊した場合はtrueを返す
    /// 挟む強さが強度より大きい場合にダメージが入る
    /// </summary>
    public bool Damage(float pliersPower)
    {
        if (m_Rod.GetComponent<Rod>() != null)
        {
            if (m_Rod.GetComponent<Rod>().GetHongFlag()) return false;
        }
        float damage = pliersPower - m_Strength;
        damage = Mathf.Clamp(damage, 0.0f, 10.0f);
        if (damage <= 0) return false;

        m_Life -= damage * Time.deltaTime;

        if (m_Life <= 0.0f)
        {
            m_isBreak = true;
            SoundManager.Instance.PlaySe("break1");
            return true;
        }

        //print("Life:" + m_Life);
        return false;
    }

    /// <summary>
    /// ペンチの挟む強さに応じて、耐久値にダメージを与える(耐久値が0を下回ったら壊れ龍)
    /// 計算後の耐久値を返す
    /// </summary>
    public float DamageAndGetLife(float pliersPower)
    {
        if (m_Rod.GetComponent<Rod>() != null)
        {
            if (m_Rod.GetComponent<Rod>().GetHongFlag()) return m_StartLife;
        }
        float damage = pliersPower - m_Strength;
        damage = Mathf.Clamp(damage, 0.0f, 10.0f);
        if (damage <= 0) return m_Life;

        m_Life -= damage * Time.deltaTime;

        if (m_Life <= 0.0f)
        {
            m_isBreak = true;
            SoundManager.Instance.PlaySe("break1");
        }

        return m_Life;
    }

    public void SetStartLife(float life)
    {
        m_StartLife = life;
        m_Life = m_StartLife;
    }
    public void SetStartStrength(float strength)
    {
        m_Strength = strength;
    }
    public float GetStartLife()
    {
        return m_StartLife;
    }

    public float GetLife()
    {
        return m_Life;
    }
}
