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
        SoundManager.Instance.PlayBgm(m_BGMName);
        Destroy(this.gameObject);
	}
}
