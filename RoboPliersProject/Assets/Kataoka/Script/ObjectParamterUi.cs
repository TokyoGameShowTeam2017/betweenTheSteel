using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParamterUi : MonoBehaviour
{
    //子のリスト
    private List<GameObject> mChild;
    //キャンパス
    private GameObject mCanvas;
    //ロッドUIオブジェクト
    private GameObject mObjectUi;
    //UIを表示するかどうか
    private bool mDrawUiFlag;
    //UIを表示しているか
    private bool mNowDrawUiFlag;
    //アウトラインのシェーダーたち
    private List<cakeslice.Outline> mOutLines;


    void Awake()
    {
        //パラメーターUIを生成
        mObjectUi = GameObject.FindGameObjectWithTag("ParameterUi");
    }

    void Start()
    {
        mOutLines = new List<cakeslice.Outline>();
        mCanvas = GameObject.FindGameObjectWithTag("Canvas");

        //フラグ初期化
        mDrawUiFlag = false;
        mNowDrawUiFlag = false;

        mChild = new List<GameObject>();
        if (gameObject.name == "ironplateKusari") {
            mChild.Add(gameObject);
            return;
        }

        Transform[] mTransforms;
        mTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (Transform trans in mTransforms)
        {
            if (trans.name != name &&
                trans.GetComponent<CatchObject>() != null)
            {
                mChild.Add(trans.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mObjectUi == null) return;
        //表示非表示
        if (mDrawUiFlag) mObjectUi.GetComponent<MoveUi>().DrawUi();


        //アウトラインOnOff
        if (mDrawUiFlag && mNowDrawUiFlag != mDrawUiFlag)
        {
            if (mChild.Count <= 0)
            {
                gameObject.AddComponent<cakeslice.Outline>();
                GetComponent<cakeslice.Outline>().eraseRenderer = false;
            }
            else
            {
                foreach (var i in mChild)
                {
                    i.AddComponent<cakeslice.Outline>();
                    i.GetComponent<cakeslice.Outline>().eraseRenderer = false;
                }
            }
            mNowDrawUiFlag = mDrawUiFlag;
        }
        else if (!mDrawUiFlag && mNowDrawUiFlag != mDrawUiFlag)
        {
            if (mChild.Count <= 0)
            {
                GetComponent<cakeslice.Outline>().eraseRenderer = true;
                Destroy(GetComponent<cakeslice.Outline>());
            }
            else
            {
                foreach (var i in mChild)
                {
                    i.GetComponent<cakeslice.Outline>().eraseRenderer = true;
                    Destroy(i.GetComponent<cakeslice.Outline>());
                }
            }
            mNowDrawUiFlag = mDrawUiFlag;
        }

        mDrawUiFlag = false;
    }
    //パラメーター設定
    public void ParameterSet()
    {
        ObjectParameter parameter = GetComponent<ObjectParameter>();
        //表示パラメーター設定
        ParameterSet(parameter.m_Strength, 0, "");
    }
    //パラメーター設定
    private void ParameterSet(int strength, int density, string mass)
    {
        if (mObjectUi == null) return;
        GameObject ui = mObjectUi.transform.FindChild("RodUi").gameObject;
        ui.transform.FindChild("Strength").GetComponent<RodParameterUi>().ParameterSet(strength.ToString());
        //ui.transform.FindChild("Density").GetComponent<RodParameterUi>().ParameterSet(density.ToString());
        //ui.transform.FindChild("Omosa").GetComponent<RodParameterUi>().ParameterSet(mass);
        RectTransform trans = ui.transform.FindChild("MaskGauge").GetComponent<RectTransform>();
        trans.sizeDelta = Vector2.zero;
    }

    public void DrawUiFlag(bool flag)
    {
        mDrawUiFlag = flag;

    }
}
