using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldChainBreakManager : MonoBehaviour
{
    //全部壊れたらTrue
    private bool mAllBreak;
    private bool mFlag;
    //鎖
    private List<GameObject> mKusaris;

    [SerializeField, Tooltip("足場プレハブ")]
    public GameObject m_ScaffoldPrefab;
    // Use this for initialization
    void Start()
    {
        mFlag = true;
        mKusaris = new List<GameObject>();
        //子のくさりを全部リストへ
        Transform[] trans;
        trans = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in trans)
        {
            //自分自身は入れない
            if (t.name != name &&
                t.name != "Chain")
            {
                if(t.name.Substring(0,5)!="Kusar")
                mKusaris.Add(t.gameObject);
            }

        }
        mAllBreak = false;
    }

    void Update()
    {
        mAllBreak = true;
        foreach (var i in mKusaris)
        {
            if(!i.GetComponent<ScaffoldChainManager>().GetBreakFlag())
            {
                mAllBreak = false;
            }
        }
        if (mAllBreak&&mFlag)
        {
            m_ScaffoldPrefab.GetComponent<ScaffoldManager>().SetType(CatchObject.CatchType.Dynamic);
            m_ScaffoldPrefab.transform.parent = null;
            mFlag = false;

        }
    }
}
