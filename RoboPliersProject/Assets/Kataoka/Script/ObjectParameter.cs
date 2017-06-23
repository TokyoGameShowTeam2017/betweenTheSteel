using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameter : MonoBehaviour {
    [SerializeField, Tooltip("密度")]
    private int m_Density;
    [SerializeField, Tooltip("強度")]
    public int m_Strength;
    [SerializeField, Tooltip("太さ")]
    public int m_Thickness;

    //重さ没
    [SerializeField, Tooltip("オブジェクトの重さ(kg)"), Space(15)]
    private float m_Mass;
    [SerializeField, Tooltip("ボーン一つの重さ(kg)※ロッド専用")]
    private float m_BoneMass;

    //ロッドの全体の重さを返す
    public float GetRodMass()
    {
        return m_BoneMass * GetComponent<Rod>().GetBone().Count;
    }
    public float GetObjectMass()
    {
        return m_Mass;
    }
}
