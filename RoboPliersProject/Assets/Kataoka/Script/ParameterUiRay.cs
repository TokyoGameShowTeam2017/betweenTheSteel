using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterUiRay : MonoBehaviour
{
    [SerializeField, Tooltip("当たっていないときに生成するオブジェクト")]
    public GameObject m_NoColObject;
    [SerializeField, Tooltip("当たった時のオブジェクト")]
    public GameObject m_IsColOBject;
    //Ray関係
    private Ray mRay;
    private RaycastHit mHit;
    //伸びるアームのRay関係
    private Ray mExtendRay;
    private RaycastHit mExtendHit;
    //ロボアームベース
    private ArmManager mRoboArmManager;
    //Rayの当たった場所
    private Vector3 mColPos;
    private LineRenderer mLineRenderer;
    //伸びさのRayスタート位置
    private Vector3 mArmNobiStart;
    //アウトライン表示されているか
    private bool mIsOutline;
    // Use this for initialization
    void Start()
    {
        mIsOutline = false;
        //初期化
        mRoboArmManager = GetComponent<ArmManager>();
        mLineRenderer = GetComponent<LineRenderer>();

        //アームの方向にRay発射
        Transform armTrans = mRoboArmManager.GetEnablArm().transform;
        Vector3 rayStart = armTrans.position + armTrans.forward * 2.0f;
        mRay = new Ray(rayStart, armTrans.transform.forward * 100.0f);

        //アームの伸びのRay発射
        Vector3 armExtend = armTrans.position + armTrans.transform.forward * 2.0f;

        mExtendRay = new Ray(rayStart, armTrans.transform.forward * 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        mLineRenderer.enabled = true;
        mIsOutline = false;
        //アームが掴んでいる場合Rayの表示をしない
        if (mRoboArmManager.GetIsEnablArmCatching())
        {
            mLineRenderer.enabled = false;
            m_IsColOBject.SetActive(false);
            m_NoColObject.SetActive(false);
        }
        //当たらないレイヤー指定
        int layer = ~(1 << 15 | 1 << 2);
        //アームの方向にパラメーターRay発射
        Transform armTrans = mRoboArmManager.GetEnablArm().transform;
        Vector3 rayStart = mRoboArmManager.GetEnablPliers().transform.position;
        //Rayの更新
        mRay.origin = rayStart;
        mRay.direction = armTrans.transform.forward;
        //LineRenderer処理
        mLineRenderer.SetPosition(0, rayStart);
        mLineRenderer.SetPosition(1, rayStart + armTrans.transform.forward.normalized * 20.0f);
        //Rayあたり判定処理
        if (Physics.Raycast(mRay, out mHit, 20.0f, layer))
        {
            if (mHit.collider.tag == "CatchObject" ||
                mHit.collider.tag == "Tekkyu" ||
                mHit.collider.tag == "UiObject")
            {
                //RodUiがあるまで親をたどる
                GameObject rodui = mHit.collider.gameObject;
                GameObject firstObject = mHit.collider.gameObject;
                while (true)
                {
                    if (rodui.GetComponent<RodUi>() != null)
                    {
                        rodui.GetComponent<RodUi>().DrawUiFlag(true);
                        //当たったオブジェクトの情報をUIに
                        float life = firstObject.GetComponent<CutRodCollision>().GetLife();
                        rodui.GetComponent<RodUi>().ParameterSet(life);
                        mIsOutline = true;
                        break;
                    }
                    if (rodui.GetComponent<ObjectParamterUi>() != null)
                    {
                        rodui.GetComponent<ObjectParamterUi>().DrawUiFlag(true);
                        //当たったオブジェクトの情報をUIに
                        rodui.GetComponent<ObjectParamterUi>().ParameterSet();
                        mIsOutline = true;
                        break;
                    }
                    if (rodui.transform.parent == null) break;
                    rodui = rodui.transform.parent.gameObject;

                }
            }
            //オブジェクトに当たったら線は消える
            if (mHit.collider.name != "PliersBase")
            {
                mColPos = mHit.point;
                mLineRenderer.SetPosition(1, mColPos);
            }
        }


        mExtendRay.origin = mRoboArmManager.GetEnablArm().transform.position;
        mExtendRay.direction = armTrans.transform.forward;


        //アームの長さ系
        if (Physics.Raycast(mExtendRay, out mExtendHit, 6.0f, layer))
        {
            //ポイントを生成
            m_IsColOBject.SetActive(true);
            m_NoColObject.SetActive(false);
            //当たったポジションに移動
            m_IsColOBject.transform.position = mExtendHit.point + (mExtendHit.normal.normalized / 10.0f);
            //法線ベクトルから回転を取得
            m_IsColOBject.transform.LookAt(m_IsColOBject.transform.position + mExtendHit.normal);
            m_IsColOBject.transform.rotation =
                Quaternion.Euler(m_IsColOBject.transform.eulerAngles.x + 90,
                m_IsColOBject.transform.eulerAngles.y,
                m_IsColOBject.transform.eulerAngles.z);
        }
        else
        {
            m_IsColOBject.SetActive(false);
            m_NoColObject.SetActive(true);
            m_NoColObject.transform.position = mRoboArmManager.GetEnablArm().transform.position +
                armTrans.forward.normalized * 6.0f;
            m_NoColObject.transform.LookAt(GameObject.FindGameObjectWithTag("RawCamera").transform);
            //m_NoColObject.transform.rotation =
            //    Quaternion.Euler(m_NoColObject.transform.eulerAngles.x + 90,
            //                     m_NoColObject.transform.eulerAngles.y,
            //                     m_NoColObject.transform.eulerAngles.z);
        }

    }
    public Vector3 GetColRayPos()
    {
        return mColPos;
    }
    public bool GetIsOutLine()
    {
        return mIsOutline;
    }
}
