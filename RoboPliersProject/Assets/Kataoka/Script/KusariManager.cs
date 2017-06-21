using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KusariManager : MonoBehaviour
{
    //鎖たち
    private List<GameObject> mKusaris;
    [SerializeField, Tooltip("鉄球のプレハブ")]
    public GameObject m_PrefabTekkyu;
    [SerializeField, Tooltip("キューブのプレハブ")]
    public GameObject m_PrefabCube;
    [SerializeField, Tooltip("鎖のプレハブ")]
    public GameObject m_PrefabKusari;
    [SerializeField, Tooltip("ターゲットポイントプレハブ")]
    public GameObject m_PrefabTarget;
    [SerializeField, Tooltip("鎖の数"), Space(15)]
    public int m_KusariNum;
    [SerializeField, Tooltip("鎖と鎖の距離")]
    public float m_KusariDis = 1.0f;
    // Use this for initialization
    void Start()
    {
        //鎖を生成からの子に登録
        for (int i = 0; i <= m_KusariNum; i++)
        {
            GameObject kusari = Instantiate(m_PrefabKusari);
            kusari.transform.parent = transform;
            kusari.transform.localPosition = new Vector3(0, -(m_KusariDis * i + 1), 0);

            if (i % 2 == 0)
            {
                kusari.transform.FindChild("Chain").transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
        }



        mKusaris = new List<GameObject>();
        //子のくさりを全部リストへ
        Transform[] trans;
        trans = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in trans)
        {
            //自分自身は入れない
            if (t.name != name &&
                t.name != "Chain")
                mKusaris.Add(t.gameObject);
        }
        //鎖ジョイント設定
        for (int i = 0; i <= mKusaris.Count - 1; i++)
        {
            if (i == 0)
                mKusaris[0].GetComponent<HingeJoint>().connectedBody = m_PrefabCube.GetComponent<Rigidbody>();
            else
                mKusaris[i].GetComponent<HingeJoint>().connectedBody = mKusaris[i - 1].GetComponent<Rigidbody>();
        }
        //鉄球のポジション
        m_PrefabTekkyu.transform.position = mKusaris[mKusaris.Count - 1].transform.position + new Vector3(0, -(m_PrefabTekkyu.transform.localScale.x/1.4f), 0);
        m_PrefabTekkyu.GetComponent<FixedJoint >().connectedBody = mKusaris[mKusaris.Count - 1].GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //くさり削除処理
        int count = 0;

        for (int i = 0; i <= mKusaris.Count - 1; i++)
        {
            //鎖が切れたら下の部分は全部消す処理
            if (mKusaris[i].GetComponent<Kusari>().GetIsDead())
            {
                //鉄球のジョイントコンポーネントを消す
                Destroy(m_PrefabTekkyu.GetComponent<FixedJoint>());
                //鎖削除
                for (int j = count; j <= mKusaris.Count - 1; j++)
                {
                    Destroy(mKusaris[j]);
                }
                //リスト内を削除
                for (int j = 0; j <= count; j++)
                {
                    mKusaris.Remove(mKusaris[mKusaris.Count - 1]);
                }
            }
            count++;
        }
    }
}
