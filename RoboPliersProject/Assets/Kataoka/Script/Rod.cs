using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rod : MonoBehaviour
{
    //回転ポイント
    private List<GameObject> mRotatePoints;
    //ボーンたち
    private List<GameObject> mBones;
    //今どっちが始点か
    private StartPoint mNowPoint;
    //アウトラインを表示させるか
    private bool mDrawOutLine;
    //アームマネージャー
    private ArmManager mArm;
    //キャッチフラグ
    private bool mNoCathFlag;
    //キャッチタイプ
    private CatchObject.CatchType m_CatchType;
    //固定されたペンチオブジェクト
    private GameObject mSevePliers;
    //今アウトラインを表示しているか
    private bool mNowOutLineFlag;
    //ホックにあたっているか
    private bool mIsHong;

    //支点(回転用)
    public enum StartPoint
    {
        NULL_POINT,
        LEFT_POINT,
        RIGHT_POINT
    }
    [SerializeField, Tooltip("マテリアル")]
    public Material m_Material;
    [SerializeField, Tooltip("支点は右か左か")]
    public StartPoint m_point;

    public void Awake()
    {
        SetChild();
    }

    public void Start()
    {
        mNowPoint = StartPoint.NULL_POINT;
        mDrawOutLine = false;
        mNowOutLineFlag = false;
        mIsHong = false;
        mNoCathFlag = false;
        mArm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        SetRotatePoint(m_point);
        mSevePliers = mArm.GetPliers(0);

        DrawOutLine(false);
    }
    void Update()
    {
        ////引っ掛かっているときの曲げる処理
        //if (m_CatchType == CatchObject.CatchType.Static&&
        //    mArm.GetEnablePliersCatchRod()==gameObject)
        //{
        //    //どのボーン番号が当たっているか
        //    int isHung=999;
        //    foreach (var i in mBones)
        //    {
        //        if (i.GetComponent<RodTurnBone>().GetHungFlag())
        //        {
        //            isHung = i.GetComponent<RodTurnBone>().GetBoneNumber();
        //            break;
        //        }
        //    }
        //    //当たっていない場合は入らない
        //    if (isHung == 999) return;

        //    if (mArm.GetEnablArmCatchingObject().GetComponent<RodTurnBone>().GetBoneNumber() > isHung)
        //        m_point = StartPoint.RIGHT_POINT;
        //    else
        //        m_point = StartPoint.LEFT_POINT;
        //}


    }
    public void LateUpdate()
    {
        ////2つのアームで曲げた時
        //TowArmRotate();
        //アウトライン設定
        SetOutLine();
        //中身が何もなかったら自信を消す
        if (mRotatePoints.Count <= 0)
        {
            Destroy(gameObject);
        }
        m_point = StartPoint.NULL_POINT;
    }
    //二つのアームで曲げた時の処理
    private void TowArmRotate()
    {
        if (mArm.GetCountCatchingDynamicObjects() == 2 &&
            m_CatchType == CatchObject.CatchType.Dynamic)
        {
            SetPoint();
        }
        if (m_point == StartPoint.LEFT_POINT && mNowPoint != StartPoint.LEFT_POINT)
        {
            GameObject pliers;
            for (int i = 0; i <= 3; i++)
            {
                GameObject mainRod = mArm.GetPliersCatchRod(i);
                if (mArm.GetPliersCatchRod(i) == gameObject)
                {
                    pliers = mArm.GetPliers(i).transform.gameObject;

                    //回転軸変更
                    SetPointPosAndInitialize(new Vector3(0, 1, 0));
                    //親を再設定
                    for (int j = 0; j <= mRotatePoints.Count - 1; j++)
                    {
                        mRotatePoints[j].transform.parent = null;
                        if (j != mRotatePoints.Count - 1)
                            mRotatePoints[j].transform.parent = mRotatePoints[j + 1].transform;
                    }
                    mRotatePoints[mRotatePoints.Count - 1].transform.parent = transform;
                    break;
                }
            }
            //メッシュの削除と生成
            Destroy(mRotatePoints[mRotatePoints.Count - 1].GetComponent<cakeslice.Outline>());
            Destroy(mRotatePoints[mRotatePoints.Count - 1].GetComponent<MeshRenderer>());
            mRotatePoints[0].AddComponent<MeshRenderer>();
            mRotatePoints[0].AddComponent<cakeslice.Outline>();
            mRotatePoints[0].GetComponent<MeshRenderer>().material = m_Material;

            mNowPoint = StartPoint.LEFT_POINT;
        }
        if (m_point == StartPoint.RIGHT_POINT && mNowPoint != StartPoint.RIGHT_POINT)
        {

            GameObject pliers;
            for (int i = 0; i <= 3; i++)
            {
                GameObject mainRod = mArm.GetPliersCatchRod(i);
                if (mArm.GetPliersCatchRod(i) == gameObject)
                {
                    pliers = mArm.GetPliers(i).transform.gameObject;
                    //回転軸変更
                    SetPointPosAndInitialize(new Vector3(0, -1, 0));
                    foreach (var j in mRotatePoints)
                    {
                        j.transform.parent = null;
                    }
                    //親を再設定
                    for (int j = mRotatePoints.Count - 1; 0 <= j; j--)
                    {
                        if (j != 0)
                            mRotatePoints[j].transform.parent = mRotatePoints[j - 1].transform;
                    }
                    mRotatePoints[0].transform.parent = transform;
                    break;
                }
                mNowPoint = StartPoint.RIGHT_POINT;
            }
            //メッシュの削除と生成
            Destroy(mRotatePoints[0].GetComponent<cakeslice.Outline>());
            Destroy(mRotatePoints[0].GetComponent<MeshRenderer>());

            mRotatePoints[mRotatePoints.Count - 1].AddComponent<MeshRenderer>();
            mRotatePoints[mRotatePoints.Count - 1].AddComponent<cakeslice.Outline>();

            mRotatePoints[mRotatePoints.Count - 1].GetComponent<MeshRenderer>().material = m_Material;
            mNowPoint = StartPoint.RIGHT_POINT;
        }
    }
    //回転ポイントを変える
    public void SetRotatePoint(StartPoint point)
    {
        //ボーン＆回転ポイント
        List<GameObject> bones = mBones;
        List<GameObject> rotatePoints = mRotatePoints;
        //ポイント設定
        m_point = point;
        if (m_point == StartPoint.LEFT_POINT && mNowPoint != StartPoint.LEFT_POINT)
        {
            //回転軸変更
            SetPointPosAndInitialize(new Vector3(0, 1, 0));
            //親を再設定
            for (int j = 0; j <= rotatePoints.Count - 1; j++)
            {
                rotatePoints[j].transform.parent = null;
                if (j != rotatePoints.Count - 1)
                    rotatePoints[j].transform.parent = rotatePoints[j + 1].transform;
            }
            //親設定
            rotatePoints[rotatePoints.Count - 1].transform.parent = transform;

            //メッシュの削除と生成
            Destroy(mRotatePoints[rotatePoints.Count - 1].GetComponent<cakeslice.Outline>());
            Destroy(mRotatePoints[rotatePoints.Count - 1].GetComponent<MeshRenderer>());
            //mRotatePoints[0].AddComponent<cakeslice.Outline>();
            if (rotatePoints[0].GetComponent<MeshRenderer>() != null)
                rotatePoints[0].GetComponent<MeshRenderer>().material = m_Material;

            mNowPoint = StartPoint.LEFT_POINT;
        }
        if (m_point == StartPoint.RIGHT_POINT && mNowPoint != StartPoint.RIGHT_POINT)
        {
            //回転軸変更
            SetPointPosAndInitialize(new Vector3(0, -1, 0));
            foreach (var j in rotatePoints)
            {
                j.transform.parent = null;
            }
            //親を再設定
            for (int j = rotatePoints.Count - 1; 0 <= j; j--)
            {
                if (j != 0)
                    rotatePoints[j].transform.parent = rotatePoints[j - 1].transform;
            }
            rotatePoints[0].transform.parent = transform;
            //メッシュの削除と生成
            Destroy(rotatePoints[0].GetComponent<cakeslice.Outline>());
            Destroy(rotatePoints[0].GetComponent<MeshRenderer>());

            rotatePoints[rotatePoints.Count - 1].AddComponent<MeshRenderer>();
            rotatePoints[rotatePoints.Count - 1].AddComponent<cakeslice.Outline>();

            rotatePoints[rotatePoints.Count - 1].GetComponent<MeshRenderer>().material = m_Material;
            mNowPoint = StartPoint.RIGHT_POINT;
        }
    }

    public void SetPointPosAndInitialize(Vector3 localPosition)
    {
        //親子関係初期化　＆　回転原点移動
        int count = 0;
        List<GameObject> bones = mBones;
        List<GameObject> rotatePoints = mRotatePoints;
        foreach (var i in rotatePoints)
        {
            i.transform.parent = null;
            bones[count].transform.parent = null;
            i.transform.parent = bones[count].transform;
            count++;
        }
        count = 0;
        foreach (var i in rotatePoints)
        {
            i.transform.localPosition = localPosition;
            i.transform.parent = null;
            bones[count].transform.parent = i.transform;
            count++;
        }
    }
    //ポイントをセット
    public void SetPoint()
    {
        GameObject enableCathRod = mArm.GetEnablePliersCatchRod();
        for (int i = 0; i <= 3; i++)
        {
            GameObject cathRod = mArm.GetPliersCatchRod(i);
            //全員が自身を掴んでいたら
            if (enableCathRod == cathRod == gameObject)
            {
                //自身は通さない
                if (mArm.GetEnablPliers() == mArm.GetPliers(i)) continue;
                //選択中のペンチが掴んでいるオブジェクト
                GameObject enableCathObject = mArm.GetEnablArmCatchingObject().gameObject;
                //もう一つのペンチが掴んでいるオブジェクト
                GameObject cathObject = mArm.GetArmCatchingObject(i).gameObject;
                //string a = enableCathRod.name.Substring(4);
                //選択中のペンチが掴んでいるボーン番号
                int numberEnable = int.Parse(enableCathObject.name.Substring(4));
                //他のペンチが掴んでいるボーン番号
                int number = int.Parse(cathObject.name.Substring(4));

                if (mNowPoint == StartPoint.LEFT_POINT &&
                    numberEnable > number)
                {
                    m_point = StartPoint.RIGHT_POINT;
                }
                else if (mNowPoint == StartPoint.RIGHT_POINT &&
                    numberEnable < number)
                {
                    m_point = StartPoint.LEFT_POINT;
                }

            }
        }
    }
    //子をリストに入れる（成功の場合はtrue）
    public bool SetChild()
    {
        //BoneRotatePointたちを取得
        Transform[] mTransforms;
        mTransforms = transform.GetComponentsInChildren<Transform>();
        if (mTransforms.Length <= 0) return false;

        mBones = new List<GameObject>();
        mRotatePoints = new List<GameObject>();


        foreach (Transform trans in mTransforms)
        {
            if (trans.gameObject.name == "BoneRotatePoint")
            {
                mRotatePoints.Add(trans.gameObject);
            }
        }
        //ボーンたちを取得
        for (int i = 0; i <= mRotatePoints.Count - 1; i++)
        {
            string s = "Bone";
            s = s + (i + 1).ToString();
            mBones.Add(mRotatePoints[i].transform.FindChild(s).gameObject);
        }
        return true;
    }

    //タイプを設定する
    public void SetCatchType(CatchObject.CatchType type)
    {
        m_CatchType = type;
        foreach (var i in mBones)
        {
            i.GetComponent<CatchObject>().SetType(m_CatchType);
        }
    }
    //タイプを設定する
    public void SetCatchTypeNoRigitBody(CatchObject.CatchType type)
    {
        m_CatchType = type;
        foreach (var i in mBones)
        {
            i.GetComponent<CatchObject>().SetNoRigidBody(m_CatchType);
        }
    }
    //アウトラインを表示するかどうか
    public void DrawOutLine(bool flag)
    {
        //OutLine重さ対策
        if (flag)
        {
            foreach (var i in mBones)
            {
                MeshRenderer mesh = i.GetComponent<MeshRenderer>();
                cakeslice.Outline outline = i.GetComponent<cakeslice.Outline>();
                if (mesh != null && outline == null)
                {
                    i.AddComponent<cakeslice.Outline>();
                }
            }
            foreach (var i in mRotatePoints)
            {
                MeshRenderer mesh = i.GetComponent<MeshRenderer>();
                cakeslice.Outline outline = i.GetComponent<cakeslice.Outline>();
                if (mesh != null && outline == null)
                {
                    i.AddComponent<cakeslice.Outline>();
                }
            }
        }
        else
        {
            foreach (var i in mBones)
            {
                cakeslice.Outline line = i.GetComponent<cakeslice.Outline>();
                if (line != null)
                {
                    line.eraseRenderer = false;
                    Destroy(line);

                }
            }
            foreach (var i in mRotatePoints)
            {
                cakeslice.Outline line = i.GetComponent<cakeslice.Outline>();
                if (line != null)
                {
                    line.eraseRenderer = false;
                    Destroy(line);
                }
            }
        }

        mDrawOutLine = flag;
    }
    //アウトライン設定
    private void SetOutLine()
    {
        //前回とフラグが違かったら
        if (mNowOutLineFlag != mDrawOutLine)
        {
            //アウトライン設定
            foreach (var i in mBones)
            {
                cakeslice.Outline line = i.GetComponent<cakeslice.Outline>();
                if (line != null)
                    i.GetComponent<cakeslice.Outline>().eraseRenderer = !mDrawOutLine;
            }
            foreach (var i in mRotatePoints)
            {
                cakeslice.Outline line = i.GetComponent<cakeslice.Outline>();
                if (line != null)
                    i.GetComponent<cakeslice.Outline>().eraseRenderer = !mDrawOutLine;
            }
            mNowOutLineFlag = mDrawOutLine;

        }
    }



    public List<GameObject> GetBone()
    {
        return mBones;
    }
    public List<GameObject> GetRotatePoint()
    {
        return mRotatePoints;
    }

    public void OnDrawGizmos()
    {
        ////離すバグ再発のため
        //GameObject thisObj = mArm.GetEnablePliersCatchRod();
        //if (thisObj == null) return;
        //for (int i = 0; i <= 3; i++)
        //{
        //    //同じペンチの場合返す
        //    if (mArm.GetEnablPliers() == mArm.GetPliers(i)) continue;
        //    GameObject obj = mArm.GetPliersCatchRod(i);
        //    if (thisObj == obj == gameObject)
        //    {
        //        testPoint = mArm.GetEnablPliers().transform.position;
        //        testVec1=mArm.GetPliers(i).transform.position;
        //        testVec2=mArm.GetPliers(i).transform.position + mArm.GetPliers(i).transform.forward*2.0f;
        //        Gizmos.color = Color.yellow;
        //        Gizmos.DrawCube(testPoint, new Vector3(0.2f, 0.2f, 0.2f));
        //        Gizmos.DrawLine(testVec1,testVec2);
        //    }
        //}

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 5.0f);
    }
    public void IsHong(bool flag)
    {
        mIsHong = flag;
    }
    public bool GetHongFlag()
    {
        return mIsHong;
    }
    public CatchObject.CatchType GetCatchType()
    {
        return m_CatchType;
    }
    public StartPoint GetStartPoint()
    {
        return mNowPoint;
    }
}
