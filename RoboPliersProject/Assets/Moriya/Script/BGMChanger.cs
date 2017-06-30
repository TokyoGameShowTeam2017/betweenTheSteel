/**==========================================================================*/
/**
 * こめんと
 * 作成者：守屋   作成日：17/10/00
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChanger : MonoBehaviour 
{
	/*==所持コンポーネント==*/

    /*==外部設定変数==*/
    public string m_BGMName = "";
    public bool m_IsRandom = false;
    public string[] m_RandomBGMNames;

    [SerializeField,Tooltip("ＢＧＭ変更を待機するか？")]
    public bool m_IsChangeWait = false;

    /*==内部設定変数==*/

    /*==外部参照変数==*/

	void Start() 
	{
        if (!m_IsRandom) return;

        string prevname = SoundManager.Instance.lastPlayBGMName;
        int rand;

        do
        {
            rand = Random.Range(0, m_RandomBGMNames.Length);
            m_BGMName = m_RandomBGMNames[rand];
        } while (m_BGMName == prevname);

	}
	
	void Update ()
	{
        if (!m_IsChangeWait)
        {
            SoundManager.Instance.PlayBgm(m_BGMName);
            Destroy(this.gameObject);
        }

	}



    /// <summary>
    /// BGMの変更を実行する(IsChangeWaitがtrueのときに使う)
    /// </summary>
    public void StartBGMChange()
    {
        m_IsChangeWait = false;
    }
}
