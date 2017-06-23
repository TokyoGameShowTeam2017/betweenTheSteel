using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUiObject : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TargetObject;

    private const string MAIN_CAMERA = "MainCamera";

    private bool m_IsRendered = true;

    // Use this for initialization
    void Start()
    {
        m_IsRendered = true;
    }

    // Update is called once per frame
    void Update()
    {

        IsRenderer();

        if (!m_IsRendered)
        {
            //ターゲット方向のベクトル
            Vector3 l_ToTarget = m_TargetObject.transform.position - GameObject.FindGameObjectWithTag(MAIN_CAMERA).transform.position;

            //前方向とターゲットの外積
            Vector3 l_ForwardCrossTarget = Vector3.Cross(GameObject.FindGameObjectWithTag(MAIN_CAMERA).transform.forward, l_ToTarget);

            //前方向とターゲットの内積
            float l_ForwardDotTarget = Vector3.Dot(GameObject.FindGameObjectWithTag(MAIN_CAMERA).transform.forward, l_ToTarget);

            //上方向と外積の内積
            float l_UpDotCross = Vector3.Dot(GameObject.FindGameObjectWithTag(MAIN_CAMERA).transform.up, l_ForwardCrossTarget);

            if (l_UpDotCross >= 0.0f)
            {
                print("右");
            }
            else
            {
                print("左");
            }
        }
    }

    private void IsRenderer()
    {
        m_IsRendered = m_TargetObject.GetComponent<Target>().GetIsRenderer();
    }
}
