using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kusari : MonoBehaviour
{
    [SerializeField, Tooltip("強度")]
    private float m_Strength = 2.0f;
    [SerializeField, Tooltip("耐久値")]
    private float m_Life = 5.0f;

    private float m_StartLife;

    public bool m_IsCollision;
    // Use this for initialization
    void Start()
    {
        m_IsCollision = false;
        GetComponent<CatchObject>().SetNoRigidBody(CatchObject.CatchType.Static);



        m_StartLife = m_Life;
    }

    public bool GetIsDead()
    {
        return m_IsCollision;
    }

    //壊す
    public void Break()
    {
        m_IsCollision = true;
    }



    public float GetStrength()
    {
        return m_Strength;
    }

    public float GetStartLife()
    {
        return m_StartLife;
    }

    public float GetLife()
    {
        return m_Life;
    }

    /// <summary>
    /// ペンチの挟む強さに応じて、耐久値にダメージを与える(耐久値が0を下回ったら壊れ龍)
    /// 計算後の耐久値を返す
    /// </summary>
    public float DamageAndGetLife(float pliersPower)
    {
        float damage = pliersPower - m_Strength;
        damage = Mathf.Clamp(damage, 0.0f, 10.0f);
        if (damage <= 0) return m_Life;

        m_Life -= damage * Time.deltaTime;
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
}
