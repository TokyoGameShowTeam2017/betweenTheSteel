/**==========================================================================*/
/**
 * こめんと
 * 作成者：守屋   作成日：17/06/23
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPadType : SingletonMonoBehaviour<InputPadType> 
{
    public enum INPUT_TYPE
    {
        PS4,
        XBOX,
    }

	/*==所持コンポーネント==*/

    /*==外部設定変数==*/
    public INPUT_TYPE m_Type;

    public string TypeName { get; set; }

    /*==内部設定変数==*/

    /*==外部参照変数==*/

	void Start() 
	{
		switch(m_Type)
        {
            case INPUT_TYPE.PS4: TypeName = "PS4"; break;
            case INPUT_TYPE.XBOX: TypeName = "XBOX"; break;
            default: TypeName = ""; break;
        }
	}
	
	void Update ()
	{

	}
}
