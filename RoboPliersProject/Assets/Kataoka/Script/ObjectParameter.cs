using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameter : MonoBehaviour
{
    [SerializeField, Tooltip("強度")]
    public int m_Strength;
    [SerializeField, Tooltip("耐久値")]
    public int m_Life;

    void Start()
    {
        SetRodParameter();
    }
    public void SetRodParameter()
    {
        if (GetComponent<Rod>() != null)
        {
            foreach (var i in GetComponent<Rod>().GetBone())
            {
                i.GetComponent<CutRodCollision>().SetStartLife(m_Life);
                i.GetComponent<CutRodCollision>().SetStartStrength(m_Strength);
            }
        }
    }
    public void SetChainParameter()
    {
        if (GetComponent<Kusari>() != null)
        {
            GetComponent<Kusari>().SetStartLife(m_Life);
            GetComponent<Kusari>().SetStartStrength(m_Strength);
        }
    }
}
