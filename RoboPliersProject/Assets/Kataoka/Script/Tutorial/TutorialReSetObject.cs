using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorealReSetObject : MonoBehaviour {

    private List<GameObject> m_Collision;
    private Vector3 mFirstPosition;
    private Quaternion mFirstQuaternion;

    private ArmManager mArm;

    public GameObject m_ResetObject;
	// Use this for initialization
	void Start () {
        //初期位置を取得
        mFirstPosition = m_ResetObject.transform.position;
        mFirstQuaternion = m_ResetObject.transform.rotation;

        m_Collision = new List<GameObject>();

        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();

        Transform[] transs;
        transs = transform.GetComponentsInChildren<Transform>();

        foreach (var i in transs)
        {
            if (i.name != name)
            {
                m_Collision.Add(i.gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var i in m_Collision)
        {
            if (!i.GetComponent<TutorialEventCollision>().GetIsCollision()&&
                mArm.GetEnablePliersCatchRod()==null)
            {
                m_ResetObject.transform.rotation = mFirstQuaternion;
                m_ResetObject.transform.position = mFirstPosition;
                m_ResetObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
	}
}
