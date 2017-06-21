using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kusari : MonoBehaviour
{
    public bool m_IsCollision;
    // Use this for initialization
    void Start()
    {
        m_IsCollision = false;
    }

    public bool GetIsDead()
    {
        return m_IsCollision;
    }
}
