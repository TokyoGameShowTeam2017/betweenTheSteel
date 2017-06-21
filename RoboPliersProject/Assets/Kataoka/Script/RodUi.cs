using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodUi : MonoBehaviour
{
    //キャンパス
    private GameObject mCanvas;
    //ロッドUIオブジェクト
    private GameObject mRodUi;
    //UIを表示するかどうか
    private bool mDrawUiFlag;
    //UIを表示しているか
    private bool mNowDrawUiFlag;
    //アウトラインのシェーダーたち
    private List<cakeslice.Outline> mOutLines;
    //生成するUIプレハブ
    public GameObject m_RoduiPrefab;
    
    ////材質情報
    //[SerializeField, Tooltip("材質名"),Space(15)]
    //public string m_Material;
    //[SerializeField, Tooltip("強度")]
    //public string m_Strength;
    //[SerializeField, Tooltip("重さ")]
    //public string m_Mass;
    // Use this for initialization

    void Awake()
    {
        //パラメーターUIを生成
        mRodUi = GameObject.FindGameObjectWithTag("ParameterUi");
    }

    void Start()
    {
        mOutLines = new List<cakeslice.Outline>();
        mCanvas = GameObject.FindGameObjectWithTag("Canvas");
        //フラグ初期化
        mDrawUiFlag = false;
        mNowDrawUiFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //表示非表示
        if (mDrawUiFlag) mRodUi.GetComponent<MoveUi>().DrawUi();

        //アウトラインOnOff
        if (mNowDrawUiFlag != mDrawUiFlag)
        {
            GetComponent<Rod>().DrawOutLine(mDrawUiFlag);
            mNowDrawUiFlag = mDrawUiFlag;
        }
        //フラグ初期化
        mDrawUiFlag = false;

    }
    //パラメーター設定
    public void ParameterSet()
    {
        ObjectParameter parameter = GetComponent<ObjectParameter>();
        //表示パラメーター設定
        ParameterSet(parameter.m_Strength, parameter.m_Density, parameter.GetObjectMass().ToString());
    }
    //パラメーター設定
    private void ParameterSet(int strength, int density, string mass)
    {
        GameObject ui = GameObject.FindGameObjectWithTag("ParameterUi").transform.FindChild("RodUi").gameObject;
        ui.transform.FindChild("Strength").GetComponent<RodParameterUi>().ParameterSet(strength.ToString());
        ui.transform.FindChild("Density").GetComponent<RodParameterUi>().ParameterSet(density.ToString());
        //ui.transform.FindChild("Omosa").GetComponent<RodParameterUi>().ParameterSet(mass);
    }
    public void DrawUiFlag(bool flag)
    {
        mDrawUiFlag = flag;

    }
}
