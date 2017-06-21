using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldChainManager : MonoBehaviour {
    //列挙中にコレクションを変更したため警告でてしまう

    //鎖たち
    private List<GameObject> mKusaris;
    //鎖壊れたフラグ
    private bool mBreakFlag;
    // Use this for initialization
    void Start()
    {
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

        mBreakFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //くさり削除処理
        int count = 0;

        for (int i = 0; i <= mKusaris.Count - 1; i++)
        {
            if (mKusaris[i] == null)
            {
                mKusaris.Remove(mKusaris[i]);
                continue;
            }
            //鎖が切れたら下の部分は全部消す処理
            if (mKusaris[i].GetComponent<Kusari>().GetIsDead())
            {
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
                mBreakFlag = true;
            }
            count++;
        }
    }
    public bool GetBreakFlag()
    {
        return mBreakFlag;
    }
}
