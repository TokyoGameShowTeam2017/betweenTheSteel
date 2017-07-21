using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        if (gameObject.name == "ironplateKusari")
        {
            mChild.Add(gameObject);
            return;
        }

        Transform[] mTransforms;
        mTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (Transform trans in mTransforms)
        {
            if (trans.name != name &&
                trans.GetComponent<CatchObject>() != null ||
                trans.name == "Chain")
            {
                mChild.Add(trans.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {


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


        //表示非表示
        if (mDrawUiFlag && mObjectUi != null && name.Substring(0, 6) == "Kusari") mObjectUi.GetComponent<MoveUi>().DrawUi();


        mDrawUiFlag = false;
    }
    //パラメーター設定
    public void ParameterSet()
    {
        ObjectParameter parameter = GetComponent<ObjectParameter>();
        Kusari kusari = GetComponent<Kusari>();
        float hp = 5;
        float firstHp = 5;
        if (kusari != null)
        {
            hp = kusari.GetLife();
            firstHp = kusari.GetStartLife();
        }
        //表示パラメーター設定
        ParameterSet(parameter.m_Strength, 0, "", hp, firstHp);
    }
    //パラメーター設定
    private void ParameterSet(int strength, int density, string mass, float chainHp, float firsthp)
    {
        if (mObjectUi == null) return;
        GameObject ui = mObjectUi.transform.FindChild("RodUi").gameObject;
        ui.transform.FindChild("Strength").GetComponent<RodParameterUi>().ParameterSet(strength.ToString());
        //ui.transform.FindChild("Density").GetComponent<RodParameterUi>().ParameterSet(density.ToString());
        //ui.transform.FindChild("Omosa").GetComponent<RodParameterUi>().ParameterSet(mass);
        float lerpCount = chainHp / firsthp;
        RectTransform trans = ui.transform.FindChild("MaskGauge").GetComponent<RectTransform>();
        //118がマックス
        trans.sizeDelta = new Vector2(Mathf.Lerp(118.0f, 0.0f, lerpCount), 6.0f);
        ui.transform.FindChild("MaskGauge").FindChild("RodGauge").GetComponent<Image>().color =
            new Color(1.0f,
            Mathf.Lerp(0.0f, 1.0f, lerpCount),
            Mathf.Lerp(0.0f, 1.0f, lerpCount));
    }

    public void DrawUiFlag(bool flag)
    {
        mDrawUiFlag = flag;

    }
}
