using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodTurn : MonoBehaviour
{

    //ボーンたち
    private List<GameObject> mBones;
    //アームマネージャー
    private ArmManager mArm;
    [SerializeField, Tooltip("角度制限設定")]
    public Vector3 m_RotateClamp = new Vector3(45, 45, 45);
    // Use this for initialization
    void Start()
    {
        //初期化
        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        mBones = GetComponent<Rod>().GetBone();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //当たった位置から一番近いところのボーンを取得
    public GameObject GetNearBone(GameObject obj)
    {
        GameObject result;
        result = mBones[0];
        foreach (GameObject i in mBones)
        {
            if (Vector3.Distance(result.transform.position, i.transform.position) >=
                Vector3.Distance(obj.transform.position, i.transform.position))
            {
                result = i;
            }
        }
        return result;
    }
}
