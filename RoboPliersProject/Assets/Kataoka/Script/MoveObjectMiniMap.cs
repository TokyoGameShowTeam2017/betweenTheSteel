using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectMiniMap : MonoBehaviour {
    //生成するオブジェクト
    private GameObject mMiniMapObject;
    [SerializeField, Tooltip("MiniMapマテリアル")]
    public Material m_MiniMapMaterial;

	// Use this for initialization
    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("Stage").transform;

        GameObject miniMap = GameObject.FindGameObjectWithTag("Map");
        //不要なスクリプト削除
        mMiniMapObject = Instantiate(gameObject);
        Destroy(mMiniMapObject.GetComponent<BoxCollider>());
        Destroy(mMiniMapObject.GetComponent<MoveObject>());
        Destroy(mMiniMapObject.GetComponent<Rigidbody>());
        Destroy(mMiniMapObject.GetComponent<MoveObject>());
        Destroy(mMiniMapObject.GetComponent<MoveObjectMiniMap>());


        Transform[] transs;
        transs = miniMap.transform.GetComponentsInChildren<Transform>();

        foreach (var i in transs)
        {
            if (i.name != miniMap.name)
            {
                Destroy(i);
            }
        }

        mMiniMapObject.transform.localPosition = transform.localPosition;
        //mMiniMapObject.GetComponent<Renderer>().material = m_MiniMapMaterial;

        List<GameObject> mMaps=new List<GameObject>();
        Transform[] mTransforms;
        mTransforms = mMiniMapObject.GetComponentsInChildren<Transform>();
        foreach (Transform trans in mTransforms)
        {
            if (trans.name!=mMiniMapObject.name)
            {
                mMaps.Add(trans.gameObject);
            }
        }
        foreach (var i in mMaps)
        {
            if(i.GetComponent<Renderer>()!=null)
            i.GetComponent<Renderer>().material = m_MiniMapMaterial;
        }
        mMiniMapObject.transform.parent = null;
        mMiniMapObject.transform.parent = miniMap.transform;
    }
	// Update is called once per frame
	void Update () {
        if (mMiniMapObject == null) return;
        mMiniMapObject.transform.localPosition = transform.localPosition;
        mMiniMapObject.transform.localRotation = transform.localRotation;
	}
}
