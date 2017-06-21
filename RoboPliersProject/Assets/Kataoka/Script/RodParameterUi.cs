using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodParameterUi : MonoBehaviour {
	// Use this for initialization
    private Text mText;
    //重さ　とかのテキスト設定
    public string TextString;
    //パラメーターstring型
    private string mParameterString;
	void Start () {
        mText = GetComponent<Text>();
        mText.text = TextString;
	}
	
	// Update is called once per frame
	void Update () {
        mText.text = TextString + mParameterString;
	}
    //パラメーターをセットする(float)
    public void ParameterSet(float parameter)
    {
        mParameterString = parameter.ToString();
    }
    //パラメーターをセットする(string)
    public void ParameterSet(string parameter)
    {
        mParameterString = parameter;
    }

}
