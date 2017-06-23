using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodTurnBone : MonoBehaviour
{
    //回すボーン
    private GameObject mRotatePoint;
    //ボーン番号
    private int mBoneNumber;
    //ロッドターン
    private GameObject m_RodTurn;
    //他のボーンたち
    private List<GameObject> mBones;
    //回転ポイント
    private List<GameObject> mRotatePoints;
    //ボーンの回転情報保存
    private Quaternion mBoneRotate;
    //速度
    private Vector3 mVelo;
    //前フレーム
    private Vector3 mPre;
    //後フレーム
    private Vector3 mCur;
    //最大限まで曲がっているか
    private bool mIsMaxRotate;
    //プレイヤー
    private GameObject mPlayer;
    //アームマネージャー
    private ArmManager mArmManager;
    //HUNGに当たっているか
    private bool mHungFlag;

    private float mAngleX;
    private float mAngleZ;

    private float mSeveAngleX;
    private float mSeveAngleZ;

    void Awake()
    {
        //ボーンを入れる
        mRotatePoint = transform.parent.gameObject;

        ParentRod();
    }
    // Use this for initialization
    void Start()
    {
        //初期化
        mPre = transform.position;
        mCur = transform.position;


        mBones = m_RodTurn.GetComponent<Rod>().GetBone();
        mRotatePoints = m_RodTurn.GetComponent<Rod>().GetRotatePoint();
        //回転情報保存
        mBoneRotate = mRotatePoint.transform.rotation;
        //ボーン何番目か
        mBoneNumber = int.Parse(gameObject.name.Replace("Bone", "")) - 1;


        mPlayer = GameObject.FindGameObjectWithTag("Player");
        mArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();

        mHungFlag = false;

    }

    // Update is called once per frame
    void Update()
    {
        //ボーン何番目か
        mBoneNumber = int.Parse(gameObject.name.Replace("Bone", "")) - 1;
        mBones = m_RodTurn.GetComponent<Rod>().GetBone();
        mRotatePoints = m_RodTurn.GetComponent<Rod>().GetRotatePoint();
        //プレイヤーが自身を掴んでなかったらリターン
        if (m_RodTurn != mArmManager.GetEnablePliersCatchRod()) return;
        //角度制限
        //x軸変換
        if (mRotatePoint.transform.localRotation.eulerAngles.x > 180) mAngleX = mRotatePoint.transform.localRotation.eulerAngles.x - 360.0f;
        else mAngleX = mRotatePoint.transform.localRotation.eulerAngles.x;
        //Z軸変換
        if (mRotatePoint.transform.localRotation.eulerAngles.z > 180) mAngleZ = mRotatePoint.transform.localRotation.eulerAngles.z - 360.0f;
        else mAngleZ = mRotatePoint.transform.localRotation.eulerAngles.z;


        //xとzをクランプ
        Vector3 clamp = m_RodTurn.GetComponent<RodTurn>().m_RotateClamp;
        mAngleX = Mathf.Clamp(mAngleX, -clamp.x, clamp.z);
        mAngleZ = Mathf.Clamp(mAngleZ, -clamp.z, clamp.z);

        //変換
        mAngleX = (mAngleX < 0) ? mAngleX + 360 : mAngleX;
        mAngleZ = (mAngleZ < 0) ? mAngleZ + 360 : mAngleZ;
        ////これ以上曲がれないよ
        //mIsMaxRotate = false;
        //if (((mAngleX >= 45.0f && mAngleX <= 315.0f) ||
        //    (mAngleZ >= 45.0f && mAngleZ <= 315.0f)))
        //{
        //    mIsMaxRotate = true;
        //    mAngleX = mSeveAngleX;
        //    mAngleZ = mSeveAngleZ;
        //    return;
        //}

        //mSeveAngleX = mAngleX;
        //mSeveAngleZ = mAngleZ;
        //オブジェクトの適応
        mRotatePoint.transform.localRotation = Quaternion.Euler(mAngleX, 0.0f, mAngleZ);
    }

    public void LateUpdate()
    {
        //プレイヤーが自身を掴んでなかったらリターン
        if (m_RodTurn != mArmManager.GetEnablePliersCatchRod()) return;
        //ボーンの速度を計算
        mCur = transform.position;
        mVelo = (mCur - mPre);
        mPre = transform.position;
    }
    //回転情報で曲げる
    public void SetRotateTurn(Quaternion quaternion)
    {
        mRotatePoint.transform.rotation = quaternion;
        //回転情報を保存
        mBoneRotate = mRotatePoint.transform.rotation;
    }
    //回転情報で曲げる　ローカル座標版
    public void SetRotteTurnLocal(Quaternion localQuaternion)
    {
        mRotatePoint.transform.localRotation = localQuaternion;
        //回転情報を保存
        mBoneRotate = mRotatePoint.transform.rotation;
    }
    //回転速度で曲げる（軸設定版）
    public void RotateAxisVelo(Vector3 axis, float veloAngle, int index = 0)
    {

        mRotatePoint.transform.Rotate(axis, veloAngle);
        //回転情報を保存
        mBoneRotate = mRotatePoint.transform.rotation;
    }

    //座標で曲げるワールド座標版（滑らせて曲げる用）
    public void SetPositionTurn(Vector3 position)
    {
        mRotatePoints[mBoneNumber].transform.LookAt(position);
        mRotatePoints[mBoneNumber].transform.rotation =
            Quaternion.Euler(transform.rotation.eulerAngles.x + 90,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z);
        //それ以降は曲がらない
        for (int i = mBoneNumber + 2; i <= mRotatePoints.Count - 1; i++)
        {
            mRotatePoints[i].transform.rotation = mBones[i].GetComponent<RodTurnBone>().GetSeveQuaternion();
        }
        //回転情報を保存
        mBoneRotate = mRotatePoint.transform.rotation;
    }
    //ボーン2を回す（重さで落ちる用）
    public void RotateBone2(int boneIndex)
    {
        ArmManager arm = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        Vector3 vec = GameObject.FindGameObjectWithTag("RawCamera").transform.right;
        mRotatePoints[boneIndex].transform.Rotate(vec, -1.0f);

    }
    //親を検索
    public void ParentRod()
    {
        //ロッド情報を取得
        GameObject obj = gameObject;
        while (true)
        {
            if (obj.GetComponent<Rod>() != null)
            {
                m_RodTurn = obj;
                break;
            }
            obj = obj.transform.parent.gameObject;
        }
    }


    //速度を返す
    public Vector3 GetVelocity()
    {
        return mVelo;
    }
    //ボーン番号を返す
    public int GetBoneNumber()
    {
        return mBoneNumber;
    }
    //回転情報を返す
    public Quaternion GetSeveQuaternion()
    {
        return mBoneRotate;
    }
    //親のロッドを返す
    public GameObject GetRod()
    {
        return m_RodTurn;
    }
    //自身の最大角度かどうかを返す
    public bool GetIsMaxRotate()
    {
        return mIsMaxRotate;
    }
    //指定したロッド最大角度かどうかフラグを返す
    public bool GetIsMaxRotateIndex(int index)
    {
        return mBones[index].GetComponent<RodTurnBone>().GetIsMaxRotate();
    }

    public void HungFlagTrue()
    {
        mHungFlag = true;
    }
    public bool GetHungFlag()
    {
        return mHungFlag;
    }
    //ボーンを返す
    public List<GameObject> GetBone()
    {
        return mBones;
    }
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(transform.position-transform.right*2.0f, transform.position+transform.right * 2.0f);
    //}
}
