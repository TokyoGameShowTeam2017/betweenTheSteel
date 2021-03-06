﻿using System.Collections;
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

        //アウトラインOnOff
        if (mNowDrawUiFlag != mDrawUiFlag)
        {
            GetComponent<Rod>().DrawOutLine(mDrawUiFlag);
            mNowDrawUiFlag = mDrawUiFlag;
        }

        //表示非表示
        if (mDrawUiFlag && mRodUi != null) mRodUi.GetComponent<MoveUi>().DrawUi();


        //フラグ初期化
        mDrawUiFlag = false;

    }
    //パラメーター設定
    public void ParameterSet(float boneHp, float firstHp)
    {
        ObjectParameter parameter = GetComponent<ObjectParameter>();
        //表示パラメーター設定
        ParameterSet(parameter.m_Strength, 0, "", boneHp, firstHp);
    }
    //パラメーター設定
    private void ParameterSet(int strength, int density, string mass, float boneHp, float firstHp)
    {
        if (GameObject.FindGameObjectWithTag("ParameterUi") == null) return;
        GameObject ui = GameObject.FindGameObjectWithTag("ParameterUi").transform.FindChild("RodUi").gameObject;
        ui.transform.FindChild("Strength").GetComponent<RodParameterUi>().ParameterSet(strength.ToString());
        ui.transform.FindChild("Density").GetComponent<RodParameterUi>().ParameterSet(density.ToString());
        RectTransform trans = ui.transform.FindChild("MaskGauge").GetComponent<RectTransform>();
        float lerpCount = boneHp / firstHp;
        //118がマックス
        trans.sizeDelta = new Vector2(Mathf.Lerp(118.0f, 0.0f, lerpCount), 6.0f);
        ui.transform.FindChild("MaskGauge").FindChild("RodGauge").GetComponent<Image>().color = new Color(1.0f,
            Mathf.Lerp(0.0f, 1.0f, lerpCount),
            Mathf.Lerp(0.0f, 1.0f, lerpCount));

        //ui.transform.FindChild("Omosa").GetComponent<RodParameterUi>().ParameterSet(mass);
    }
    public void DrawUiFlag(bool flag)
    {
        mDrawUiFlag = flag;

    }
}
