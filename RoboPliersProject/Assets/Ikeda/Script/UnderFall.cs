using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderFall : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MoveObject;

    private bool m_IsObjectExit = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsObjectExit)
        {
            m_MoveObject.GetComponent<MoveObject>().isMotion = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        m_IsObjectExit = true;
    }
}
