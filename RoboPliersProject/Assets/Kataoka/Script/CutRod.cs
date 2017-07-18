using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutRod : MonoBehaviour
{
    //ボーンたち
    private List<GameObject> m_Bones;
    //回転ポイント
    private List<GameObject> m_RotatePoints;
    //生成するプレハブ
    private GameObject mGravityPrefab;
    //一回生成したかどうか
    private bool mIsSpawnPrefab;
    //重力を適応させるか
    private bool mIsPrefabGravity;
    //カットされたか
    private bool mIsCutFlag;
    [SerializeField, Tooltip("ここはいじらないで")]
    public bool m_StartRodFlag;
    [SerializeField, Tooltip("両端固定されているか"), Space(15)]
    public bool m_FixBothEnds;
    [SerializeField, Tooltip("どこも固定されていないか")]
    public bool m_Free;

    // Use this for initialization
    void Start()
    {
        //初期化
        mIsSpawnPrefab = false;
        mIsPrefabGravity = m_FixBothEnds;
        mIsCutFlag = false;
        //取得
        m_Bones = GetComponent<Rod>().GetBone();
        m_RotatePoints = GetComponent<Rod>().GetRotatePoint();
        //プレハブ取得
        mGravityPrefab = (GameObject)Resources.Load("MainRod");

        if (m_Free)
            gameObject.GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i <= m_Bones.Count - 1; i++)
        {
            //壊れたら
            if (m_Bones[i].GetComponent<CutRodCollision>().m_isBreak)
            {
                //端は切れない（変える）
                if (i == m_Bones.Count - 1) return;
                //切れたよ
                mIsCutFlag = true;
                //親情報を初期化
                m_Bones[i + 1].transform.parent.parent = null;
                //新しいオブジェクト生成
                GameObject prefab = Instantiate(mGravityPrefab);
                prefab.transform.position = m_RotatePoints[i + 1].transform.position;
                prefab.transform.rotation = transform.rotation;

                bool leftPoint = false;
                if (GetComponent<Rod>().GetStartPoint() == Rod.StartPoint.LEFT_POINT)
                {
                    GetComponent<Rod>().SetRotatePoint(Rod.StartPoint.RIGHT_POINT);
                    leftPoint = true;
                }


                m_RotatePoints[i + 1].transform.parent = prefab.transform;



                //メッシュ削除＆アウトライン
                Destroy(m_Bones[i + 1].transform.parent.gameObject.GetComponent<cakeslice.Outline>());
                Destroy(m_Bones[i + 1].transform.parent.gameObject.GetComponent<MeshFilter>());
                Destroy(m_Bones[i + 1].transform.parent.gameObject.GetComponent<MeshRenderer>());

                //削除
                Destroy(m_RotatePoints[i]);
                //リストからかけ離れたオブジェクトを削除＆リネーム
                int count = 0;
                int index = m_Bones.Count - 1;
                for (int j = i; j <= index; j++)
                {
                    m_Bones[i].name = "Bone" + count.ToString();
                    m_Bones[i].GetComponent<CatchObject>().Initialize();
                    m_Bones[i].GetComponent<RodTurnBone>().ParentRod();
                    m_Bones.Remove(m_Bones[i]);
                    count++;
                }
                index = m_RotatePoints.Count - 1;
                for (int j = i; j <= index; j++)
                {
                    m_RotatePoints.Remove(m_RotatePoints[i]);
                }
                prefab.GetComponent<Rod>().SetChild();
                //重力の設定
                //元のRodの場合
                if (m_StartRodFlag)
                {
                    //両端固定だったら生成される一回目のオブジェクトは固定＆回転ポイントが変わる
                    if (m_FixBothEnds && !mIsSpawnPrefab)
                    {
                        prefab.GetComponent<Rod>().SetRotatePoint(Rod.StartPoint.LEFT_POINT);
                        prefab.GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Static);
                        prefab.GetComponent<CutRod>().m_FixBothEnds = m_FixBothEnds;

                    }
                    //それ以外は非固定
                    else
                    {
                        prefab.GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
                        prefab.GetComponent<CutRod>().m_FixBothEnds = false;
                    }
                }
                //生成されたやつら
                else
                {
                    if (m_FixBothEnds)
                    {
                        //生成された奴らが固定されるため
                        GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
                        prefab.GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Static);
                        GetComponent<CutRod>().m_FixBothEnds = false;
                        prefab.GetComponent<CutRod>().m_FixBothEnds = true;
                    }
                    else
                    {
                        prefab.GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
                        GetComponent<Rod>().SetCatchType(CatchObject.CatchType.Dynamic);
                    }
                }
                //パラメーターを設定
                //prefab.GetComponent<ObjectParameter>().m_Density = GetComponent<ObjectParameter>().m_Density;
                prefab.GetComponent<ObjectParameter>().m_Strength = GetComponent<ObjectParameter>().m_Strength;
                prefab.GetComponent<ObjectParameter>().m_Life = GetComponent<ObjectParameter>().m_Life;
                //prefab.GetComponent<ObjectParameter>().m_BoneMass = GetComponent<ObjectParameter>().m_BoneMass;
                prefab.GetComponent<ObjectParameter>().SetRodParameter();
                //prefab.GetComponent<RodReSet>().SetChild();
                GetComponent<RodReSet>().SetChild();
                if (leftPoint)
                    GetComponent<Rod>().SetRotatePoint(Rod.StartPoint.LEFT_POINT);
                //一回生成された
                mIsSpawnPrefab = true;
            }
        }
    }
    public bool GetCutFlag()
    {
        return mIsCutFlag;
    }
}
