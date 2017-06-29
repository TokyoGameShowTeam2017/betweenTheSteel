/**==========================================================================*/
/**    //aaa
 * アームの移動
 * 作成者：守屋   作成日：17/04/21
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField, Tooltip("動かすアームのオブジェクト　伸ばしたいオブジェクトはすべてここに入れる")]
    private GameObject[] m_MoveObjects;
    [SerializeField, Tooltip("アームの伸び最大値")]
    private float m_MaxArmLength = 0.7f;

    [SerializeField, Tooltip("アームの向く地点")]
    private GameObject m_ArmLookingObject;

    [SerializeField, Tooltip("プレイヤーマネージャー")]
    private PlayerManager m_PlayerManager;

    [SerializeField, Tooltip("エイムアシストを行うか？")]
    private bool m_IsAimAssist = false;

    /*==内部設定変数==*/
    //[SerializeField, Tooltip("アームマネージャー")]
    private ArmManager m_ArmManager;
    //自身のID
    private int m_ID;
    //アームの初期位置
    private Vector3[] m_StartArmPositions;
    //アームの伸び時の目標となる線形補完値(0～1)
    private float m_ArmLengthTargetValue = 0.0f;
    //アームの伸びに実際に使用する線形補完値(0～1)
    private float m_ArmLengthValue = 0.0f;

    //動かないオブジェクトを掴んだ地点
    private Vector3 m_StaticCatchPoint;

    //エイムアシスト中か？
    private bool m_IsAimAssisting = false;
    //エイムアシスト地点
    private Vector3 m_AimAssistPosition;
    //エイムアシスト距離
    private float m_AimAssistLength;

    //伸ばした量の合計
    private Vector3 m_ArmVelocityTotal;

    //簡単モード　掴み動作中か？
    private bool m_IsCatching = false;
    private bool m_IsPrevCatchingInput = false;
    private const float MAX_LENGTH = 999999.0f;

    //簡単モード　伸び終わったか？
    private bool m_IsStretched = false;
    //簡単モード　（掴んだ後）伸び状態をキープしているか？
    private bool m_IsStretchKeep = true;

    //簡単モード　伸ばし入力が押されたか？
    private bool m_IsInputStretch = false;
    //簡単モード　ターゲットがいないときの伸び縮み切り替えフラグ
    private bool m_IsNonTargetStretch = false;
    private bool m_IsPrevStretchInput = false;


    private AimAssistRockon m_AimAssistRockon;

    //アームの伸びた量
    private float m_ArmStretchLength;
    private float m_PrevArmStretchLength;

    //アームを向ける座標
    private Vector3 m_LookPosition;
    private Vector3 m_LookPositionPrev;
    private Vector3 m_LookOffset = Vector3.zero;

    //アシスト座標のオブジェクト名
    private string m_AimAssistName = "";
    //エイムアシストの検索で、何かが引っかかっているか？
    private bool m_IsSearched = false;

    //ローカル回転量
    private float m_LocalEulerY;

    private bool m_NonAssistCatch = false;

    /*==外部参照変数==*/


    void Awake()
    {
        //コンポーネント取得
        tr = GetComponent<Transform>();
    }

    void Start()
    {
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        m_StartArmPositions = new Vector3[m_MoveObjects.Length];
        for (int i = 0; i < m_MoveObjects.Length; i++)
            m_StartArmPositions[i] = m_MoveObjects[i].transform.localPosition;

        m_AimAssistPosition = Vector3.zero;
        m_AimAssistRockon = GameObject.Find("AimAssistRockon").GetComponent<AimAssistRockon>();

        m_LocalEulerY = tr.localEulerAngles.y;
    }

    void Update()
    {

        ////選択中のアームがオブジェクトを掴んでいるときは方向変化しない
        //CatchObject catchobj = m_ArmManager.GetEnablArmCatchingObject();
        //if (catchobj != null)
        //{
        //    //動かないオブジェクトを掴んでいるとき
        //    if (catchobj.catchType == CatchObject.CatchType.Static)
        //    {
        //        //掴んだ地点を向く
        //        m_LookPosition =catchobj.transform.position);
        //    }
        //}
    }

    void LateUpdate()
    {
        if (!m_ArmManager.IsMove) return;

        int catchid = m_ArmManager.GetCatchingArmID();
        if (catchid != -1 && catchid == m_ID)
        {
            tr.LookAt(m_ArmManager.GetPliersMoveByID(m_ID).GetPlayerAxisMoveObject().transform.position);
        }

        //if (m_ArmManager.GetEnablArmID() == m_ID)
        //{
        //    Vector3 l = Vector3.Lerp(m_LookPositionPrev, m_LookPosition, 0.3f);
        //    tr.LookAt(l);
        //    m_LookPositionPrev = l;
        //}
        //Rotation();
            
    }


    /// <summary>
    /// 更新処理　（選択中のアームのみ実行、アームマネージャーから呼び出して動かす）
    /// 常に実行するものはUpdate()に書く
    /// </summary>
    public void ArmUpdate()
    {
        if (m_PlayerManager.GetIsEasyMode())
            EasyMode();
        else
            NormalMode();

        ////元々ここで回転計算
        //Rotation();

        StretceLerp();
        m_PrevArmStretchLength = m_ArmStretchLength;
        m_ArmStretchLength = Mathf.Lerp(0.0f, m_MaxArmLength, m_ArmLengthValue);
                    ////ペンチのズレの分を加味 アームの関節の個数で割って値を均等に振り分ける
                    //- Mathf.Lerp(0.0f, m_PlayerManager.GetAimAssistPliersLength(), 1.0f - m_ArmLengthValue) / m_MoveObjects.Length;
        m_ArmStretchLength *= m_MoveObjects.Length;


        if (m_ArmManager.GetEnablArmID() == m_ID)
        {
            Vector3 l = Vector3.Lerp(m_LookPositionPrev, m_LookPosition, 0.3f);
            tr.LookAt(l);
            m_LookPositionPrev = l;
        }
        Rotation();
    }

    //入力処理
    private void NormalInput()
    {
        bool input = InputManager.GetPliersCatch() > 0.0f;
        if (input && !m_IsPrevCatchingInput)//トリガー判定
        {
            m_IsCatching = !m_IsCatching;
            SoundManager.Instance.PlaySe("xg-2armmove");
        }
        m_IsPrevCatchingInput = input;

        input = InputManager.GetArmStretch() > 0.0f;
        if (input && !m_IsPrevStretchInput)//トリガー判定
        {
            m_IsInputStretch = input;
            m_IsNonTargetStretch = !m_IsNonTargetStretch;
            SoundManager.Instance.PlaySe("xg-2armmove");
        }
        m_IsPrevStretchInput = input;
    }

    //簡単モード時の入力処理
    private void EasyInput()
    {
        bool input = InputManager.GetCatchingEasyMode();
        if (input && !m_IsPrevCatchingInput )//トリガー判定
        {
            if (m_ArmManager.GetPliersMoveByID(m_ID).GetIsCatch() && m_ArmManager.IsRelease)
            {
                CatchingCancel();
            }
            else if (m_IsCatching && m_ArmManager.IsRelease)
            {
                CatchingCancel();
            }
            else if (!m_IsCatching)
            {
                m_IsCatching = true;
            }

            SoundManager.Instance.PlaySe("xg-2armmove");
        }
        m_IsPrevCatchingInput = input;

        if (m_ArmManager.IsStretch)
        {
            input = InputManager.GetArmStretchEasyMode();
            if (input && !m_IsPrevStretchInput)//トリガー判定
            {
                m_IsInputStretch = input;
                m_IsNonTargetStretch = !m_IsNonTargetStretch;
                SoundManager.Instance.PlaySe("xg-2armmove");
            }
            m_IsPrevStretchInput = input;
        }

        ////ロックオン掴みをキャンセル
        //if (m_ArmManager.GetEnablPliersMove().GetIsCatch() && m_IsStretched && InputManager.GetCatchingEasyMode())
        //{
        //    //m_IsCatching = false;
        //    //m_IsNonTargetStretch = false;
        //    //m_ArmLengthTargetValue = 0.0f;
        //    CatchingCancel();
        //}

    }

    //伸ばし入力値の補完を行う
    private void StretceLerp()
    {
        m_ArmLengthValue = Mathf.Lerp(m_ArmLengthValue, m_ArmLengthTargetValue, m_ArmManager.GetStretchLerpValue());
    }

    //伸ばす
    private void Stretch()
    {
        for (int i = 0; i < m_MoveObjects.Length; i++)
        {
            m_MoveObjects[i].transform.localPosition =
                m_StartArmPositions[i]
                + Vector3.forward * Mathf.Lerp(0.0f, m_MaxArmLength, m_ArmLengthValue);
        }
    }
    private void Stretch(float length)
    {
        for (int i = 0; i < m_MoveObjects.Length; i++)
        {
            m_MoveObjects[i].transform.localPosition =
                m_StartArmPositions[i]
                + Vector3.forward * length;
        }
    }


    //通常モード時のアーム計算処理
    private void NormalMode()
    {
        m_AimAssistRockon.gameObject.SetActive(false);
        NormalInput();

        //選択中のペンチがオブジェクトを掴んでいるとき
        CatchObject catchobj = m_ArmManager.GetEnablArmCatchingObject();
        if (catchobj != null)
        {
            //アームの伸ばし入力があったら、伸び縮みできない状態解除
            if (m_IsInputStretch)
                m_IsStretchKeep = false;
            if (!m_IsStretchKeep)
            {
                //入力量に応じて伸びる量を変化
                m_ArmLengthTargetValue = InputManager.GetArmStretch();
            }

            //動かないオブジェクトを掴んでいるとき
            if (catchobj.catchType == CatchObject.CatchType.Static)
            {
                //掴んだ地点を向く
                m_LookPosition = m_StaticCatchPoint;
                //m_LookPosition = m_PlayerManager.GetAxisMoveObject().transform.position;

                //伸び縮みできない状態解除中かつ、他のアームが掴んでいなければ伸ばす
                if (!m_IsStretchKeep && m_ArmManager.GetCountCatchingObjects() <= 1)
                    Stretch();


                ////アーム伸び縮みテスト
                //if (!m_IsStretchKeep)
                //{
                //    //伸ばす量を計算　
                //    float l =
                //        Mathf.Lerp(0.0f, m_MaxArmLength, m_ArmLengthValue)
                //        //ペンチのズレの分を加味 アームの関節の個数で割って値を均等に振り分ける
                //        - Mathf.Lerp(0.0f, m_PlayerManager.GetAimAssistPliersLength(), 1.0f - m_ArmLengthValue) / m_MoveObjects.Length;
                //    //伸ばす
                //    Stretch(l);

                //    //posも移動
                //    float moveLength = l - m_PrevArmStretchLength;
                //    Transform pos = m_PlayerManager.GetAxisMoveObject().transform.FindChild("RotX").FindChild("Pos");
                //    pos.position += -tr.forward * moveLength;

                //    print(moveLength);
                //}
                return;
            }
        }

        //伸ばし入力取得
        m_ArmLengthTargetValue = InputManager.GetArmStretch();
        //m_ArmLengthTargetValue += (InputManager.GetArmStretch() - m_ArmLengthTargetValue) / 10.0f;
        //m_ArmLengthTargetValue = Mathf.Clamp(m_ArmLengthTargetValue, 0.0f, 1.0f);

        //伸ばす
        Stretch();

        //掴んでないときは通常の方向変化を行う
        m_LookPosition =m_ArmLookingObject.transform.position;
    }

    //簡単モード時のアーム計算処理
    private void EasyMode()
    {
        EasyInput();

        //選択中のペンチがオブジェクトを掴んでいるとき
        CatchObject catchobj = m_ArmManager.GetEnablArmCatchingObject();
        if (catchobj != null)
        {
            m_AimAssistRockon.SetSpriteDraw(false);


            //アームの伸ばし入力があったら、伸び縮みできない状態解除
            if (m_IsInputStretch)
            {
                m_IsStretchKeep = false;
            }
            if (!m_IsStretchKeep)
            {
                //伸びる量を変化
                if (m_IsNonTargetStretch)
                    m_ArmLengthTargetValue = 1.0f;
                else
                    m_ArmLengthTargetValue = 0.0f;
            }

            //動かないオブジェクトを掴んでいるとき
            if (catchobj.catchType == CatchObject.CatchType.Static)
            {
                //掴んだ地点を向く
                //m_LookPosition = m_StaticCatchPoint;
                m_LookPosition = m_ArmManager.GetPliersMoveByID(m_ID).GetPlayerAxisMoveObject().transform.position;
                //m_LookPosition = m_PlayerManager.GetAxisMoveObject().transform.position;


                //伸び縮みできない状態解除中かつ、他のアームが掴んでいなければ伸ばす
                if (!m_IsStretchKeep && m_ArmManager.GetCountCatchingObjects() <= 1)
                {
                    //print("called");
                    Stretch();
                }

                //入力があったら掴み、伸び解除
                if (!m_IsCatching && !m_NonAssistCatch && m_ArmManager.IsRelease)
                {
                    m_IsStretched = false;
                    m_IsStretchKeep = true;
                    m_IsNonTargetStretch = false;
                }

                ////目標地点まで伸びる
                //m_ArmLengthTargetValue = m_AimAssistLength / m_PlayerManager.GetAimAssistMaxLength();

                //if (!m_IsStretchKeep)
                //{
                //    //伸ばす量を計算　
                //    float l =
                //        Mathf.Lerp(0.0f, m_MaxArmLength, m_ArmLengthValue)
                //        //ペンチのズレの分を加味 アームの関節の個数で割って値を均等に振り分ける
                //        - Mathf.Lerp(0.0f, m_PlayerManager.GetAimAssistPliersLength(), 1.0f - m_ArmLengthValue) / m_MoveObjects.Length;
                //    //伸ばす
                //    Stretch(l);

                //    //posも移動
                //    float moveLength = m_ArmStretchLength - m_PrevArmStretchLength;
                //    Transform pos = m_PlayerManager.GetAxisMoveObject().transform.FindChild("RotX").FindChild("Pos");
                //    pos.position += tr.forward * moveLength;
                //}
                return;
            }
            //動かせるオブジェクトを掴んでいるとき
            else if (catchobj.catchType == CatchObject.CatchType.Dynamic)
            {
                //伸ばす
                Stretch();


                if (m_ArmManager.GetCountCatchingDynamicObjects() >= 2 &&
                    (InputManager.GetArmPositiveTurn() || InputManager.GetArmNegativeTurn()))
                {
                    m_LookOffset += m_ArmManager.GetPliersMoveByID(m_ID).GetCatchObjVelocity();
                    //print(m_ArmManager.GetPliersMoveByID(m_ID).GetCatchObjVelocity().ToString("f4"));
                }
                
                //方向変化
                m_LookPosition = m_ArmLookingObject.transform.position + m_LookOffset;


                //入力があったら掴み、伸び解除
                if (!m_IsCatching && m_ArmManager.IsRelease)
                {
                    m_IsStretched = false;
                    m_IsStretchKeep = true;
                    m_IsNonTargetStretch = false;
                }
                return;
            }
        }

        //以下　通常時

        ////AimAssistPointを取得
        //GameObject[] all = GameObject.FindGameObjectsWithTag("AimAssistPoint");
        ////アームの正面にあるものだけに絞る
        //List<Vector3> points = new List<Vector3>();
        //Vector3 f = Vector3.Normalize(m_ArmLookingObject.transform.position - tr.position);
        //foreach (GameObject obj in all)
        //{
        //    float d = Vector3.Dot(f, Vector3.Normalize(obj.transform.position - tr.position));
        //    float rad = Mathf.Cos(m_PlayerManager.GetAimAssistCameraAngle() * Mathf.Deg2Rad);
        //    if (d > rad)
        //        points.Add(obj.transform.position);
        //}
        ////距離の最も近い地点を探す
        //float length = MAX_LENGTH;
        //Vector3 nearest = Vector3.zero;
        //foreach (Vector3 p in points)
        //{
        //    float l = Vector3.Magnitude(p - tr.position);
        //    if (l < length && l < m_PlayerManager.GetAimAssistSearchLength())
        //    {
        //        length = l;
        //        nearest = p;
        //    }
        //}



        ////===================================新しい=========================================
        ////AimAssistPointを取得
        //GameObject[] all = GameObject.FindGameObjectsWithTag("AimAssistPoint");
        ////ペンチからある程度近く、かつアーム根元より正面のものだけに絞る
        //List<Vector3> points = new List<Vector3>();
        //Vector3 plierspos = m_ArmManager.GetEnablPliers().transform.position;
        //Vector3 f = Vector3.Normalize(m_ArmLookingObject.transform.position - tr.position);
        //foreach (GameObject obj in all)
        //{
        //    float l = Vector3.Magnitude(obj.transform.position - tr.position);
        //    if (l < m_PlayerManager.GetAimAssistSearchLength())
        //    {
        //        float d = Vector3.Dot(f, Vector3.Normalize(obj.transform.position - tr.position));
        //        float rad = Mathf.Cos(m_PlayerManager.GetAimAssistCameraAngle() * Mathf.Deg2Rad);
        //        if (d > rad)
        //            points.Add(obj.transform.position);
        //    }
        //}

        ////Vector3 f = Vector3.Normalize(m_ArmLookingObject.transform.position - tr.position);
        ////アームの根元から注視点へ向かう線分から、最も近い座標を探す
        //float length = MAX_LENGTH;
        //Vector3 nearest = Vector3.zero;
        //float assistLength = 0.0f;
        //foreach (Vector3 p in points)
        //{
        //    float l = LineAndPointLength(tr.position, m_ArmLookingObject.transform.position, p);
        //    if (l < length)
        //    {
        //        length = l;
        //        nearest = p;
        //        assistLength = Vector3.Magnitude(p - tr.position);
        //    }
        //}
        ////===================================新しい=========================================



        //===================================チュートリアル対応版=========================================
        //AimAssistPointを取得
        GameObject[] all = GameObject.FindGameObjectsWithTag("AimAssistPoint");
        //ペンチからある程度近く、かつアーム根元より正面のものだけに絞る
        List<GameObject> points = new List<GameObject>();
        Vector3 plierspos = m_ArmManager.GetEnablPliers().transform.position;
        Vector3 f = Vector3.Normalize(m_ArmLookingObject.transform.position - tr.position);
        foreach (GameObject obj in all)
        {
            float l = Vector3.Magnitude(obj.transform.position - tr.position);
            if (l < m_PlayerManager.GetAimAssistSearchLength())
            {
                float d = Vector3.Dot(f, Vector3.Normalize(obj.transform.position - tr.position));
                float rad = Mathf.Cos(m_PlayerManager.GetAimAssistCameraAngle() * Mathf.Deg2Rad);
                if (d > rad)
                    points.Add(obj);
            }
        }

        //Vector3 f = Vector3.Normalize(m_ArmLookingObject.transform.position - tr.position);
        //アームの根元から注視点へ向かう線分から、最も近い座標を探す
        float length = MAX_LENGTH;
        Vector3 nearest = Vector3.zero;
        float assistLength = 0.0f;
        foreach (GameObject p in points)
        {
            float l = LineAndPointLength(tr.position, m_ArmLookingObject.transform.position, p.transform.position);
            if (l < length)
            {
                length = l;
                nearest = p.transform.position;
                assistLength = Vector3.Magnitude(p.transform.position - tr.position);
                m_AimAssistName = p.name;
            }
        }
        //==================================================================================

        //アシスト座標が無い場合はfalse
        m_IsSearched = false; 
        //最も近い地点が求まったら
        if (length < MAX_LENGTH)
        {
            m_IsSearched = true;
            m_IsAimAssisting = true;
            m_AimAssistPosition = nearest;
            m_AimAssistLength = assistLength;

            m_AimAssistRockon.SetSpriteDraw(true);
            m_AimAssistRockon.SetTargetPosition(nearest);

            //通常の方向変化
            m_LookPosition = m_ArmLookingObject.transform.position;
        }
        //近くにAimAssistPointが無い場合は通常モードと同じ動きを行う。
        else
        {
            m_IsAimAssisting = false;
            m_IsCatching = false;
            m_IsStretched = false;

            //伸ばし入力取得
            if (m_IsNonTargetStretch)
                m_ArmLengthTargetValue = 1.0f;
            else
                m_ArmLengthTargetValue = 0.0f;

            //伸ばす
            Stretch();
            //掴んでないときは通常の方向変化を行う
            m_LookPosition = m_ArmLookingObject.transform.position;

            m_AimAssistRockon.SetSpriteDraw(false);
            return;
        }


        //掴み動作を行う
        if (m_IsCatching && m_ArmManager.IsCatchAble)
        {
            //アームを目標地点に向ける
            m_LookPosition =m_AimAssistPosition;
            //目標地点まで伸びる
            m_ArmLengthTargetValue = m_AimAssistLength / m_PlayerManager.GetAimAssistMaxLength();

            //伸ばす量を計算　
            float l =
                Mathf.Lerp(0.0f, m_MaxArmLength, m_ArmLengthValue)
                //ペンチのズレの分を加味 アームの関節の個数で割って値を均等に振り分ける
                - Mathf.Lerp(0.0f, m_PlayerManager.GetAimAssistPliersLength(), 1.0f - m_ArmLengthValue) / m_MoveObjects.Length;
            //伸ばす
            Stretch(l);

            //伸びきっているかの判定
            if (Mathf.Abs(m_ArmLengthTargetValue - m_ArmLengthValue) < 0.01f)
                m_IsStretched = true;
            else
                m_IsStretched = false;
        }
        //それ以外
        else
        {
            if(m_ArmManager.IsStretch)
            {
                if (m_IsNonTargetStretch)
                    m_ArmLengthTargetValue = 1.0f;
                else
                    m_ArmLengthTargetValue = 0.0f;
                Stretch();
            }

        }
    }

    private void Rotation()
    {
        ////角度限界を超えた時
        //360→-180～180に変換
        float y = tr.localEulerAngles.y - (90.0f * m_ID);
        if (m_ID == 3)
        {
            y += 360.0f;
        }
        //float y = tr.localEulerAngles.y;
        float rotY = y > 180.0f ? y - 360.0f : y;



        //角度限界を超えた分の値をプレイヤーに渡す
        float limit = m_ArmManager.GetArmAngleMax();
        if (m_ArmManager.GetCatchingArmID() == m_ID)
            limit = m_ArmManager.StaticCatchingArmAngleMax;
        float over = 0.0f;


        if (rotY > limit)
            over = rotY - limit;
        else if (rotY < -limit)
            over = rotY + limit;

        m_PlayerManager.SetArmAngleOver(over);

        //角度限界で制限
        rotY = Mathf.Clamp(rotY, -limit, limit);


        //360に戻す
        //float angleY = rotY;
        float angleY = rotY < 0 ? rotY + 360.0f : rotY;
        angleY += 90.0f * m_ID;

        //float angleY = rotY;
        //angleY -= 90.0f * m_ID;

        //計算した角度を適用
        Vector3 angles = tr.localEulerAngles;
        angles.y = angleY;
        
        tr.localEulerAngles = angles;
    }


    /// <summary>
    /// 線分と座標の距離を返す
    /// </summary>
    private float LineAndPointLength(Vector3 s,Vector3 e,Vector3 p)
    {
        Vector3 AB, AP;
        AB = e - s;
        AP = p - s;

        float D = Vector3.Cross(AB, AP).magnitude;
        float L = Vector3.Distance(s, e);
        return  D / L;
    }

    /// <summary>
    /// アームリセット時の動き
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetMove()
    {
        float timer = 0.0f;
        float maxtime = m_ArmManager.GetResetEndTime();

        Quaternion start = tr.localRotation;
        Quaternion target = Quaternion.Euler(new Vector3(0.0f, 90.0f * m_ID, 0.0f));

        while (timer <= maxtime)
        {
            timer += Time.deltaTime;

            m_ArmLengthValue -= 1.0f / maxtime * Time.deltaTime;
            m_ArmLengthValue = Mathf.Clamp(m_ArmLengthValue, 0.0f, 1.0f);
            for (int i = 0; i < m_MoveObjects.Length; i++)
                m_MoveObjects[i].transform.localPosition = m_StartArmPositions[i] + Vector3.forward * Mathf.Lerp(0.0f, m_MaxArmLength, m_ArmLengthValue);

            tr.localRotation = Quaternion.Slerp(start, target, timer / maxtime);
            yield return null;
        }
        m_IsNonTargetStretch = false;
        yield break;
    }


    /// <summary>
    /// IDをセットする
    /// </summary>
    public void SetID(int id)
    {
        m_ID = id;
    }

    /// <summary>
    /// エイムアシストの座標を変更
    /// </summary>
    public void SetAimAssistPosition(Vector3 pos)
    {
        m_AimAssistPosition = pos;
    }

    /// <summary>
    /// 位置等をリセットする
    /// </summary>
    public void Reset()
    {
        StartCoroutine(ResetMove());

        //tr.localEulerAngles = new Vector3(0.0f, 90.0f * m_ID, 0.0f);
        //m_ArmLengthValue = 0.0f;
        //for (int i = 0; i < m_MoveObjects.Length; i++)
        //    m_MoveObjects[i].transform.localPosition = m_StartArmPositions[i];
    }


    /// <summary>
    /// アームの移動量を取得
    /// </summary>
    public Vector3 GetArmVelocity()
    {
        return m_ArmVelocityTotal;
    }


    /// <summary>
    /// 掴むために腕を伸ばし中か？
    /// </summary>
    /// <returns></returns>
    public bool GetIsCatching()
    {
        return m_IsCatching;
    }

    public bool GetIsStreched()
    {
        return m_IsStretched;
    }

    public Vector3 GetAimAssistPosition()
    {
        return m_AimAssistPosition;
    }

    public bool GetIsAimAssisting()
    {
        return m_IsAimAssisting;
    }

    public void SetStaticCatchPoint(Vector3 p)
    {
        m_StaticCatchPoint = p;
    }

    /// <summary>
    /// アームの伸びた量を取得
    /// </summary>
    public float GetArmStretch()
    {
        return m_ArmStretchLength;
    }

    /// <summary>
    /// キャッチ動作をやめる
    /// </summary>
    public void CatchingCancel()
    {
        m_IsCatching = false;
        m_IsNonTargetStretch = false;
        m_ArmLengthTargetValue = 0.0f;

        m_IsInputStretch = false;
        m_IsStretchKeep = true;
        m_IsStretched = false;

        m_NonAssistCatch = false;
        m_LookOffset = Vector3.zero;
    }

    public string GetAimAssistName()
    {
        return m_AimAssistName;
    }


    public bool GetIsSearched()
    {
        return m_IsSearched;
    }

    /// <summary>
    /// 強制的にオブジェクトを掴む
    /// </summary>
    public void ForceCatching()
    {
        m_IsCatching = true;

        //m_IsInputStretch = true;
        //m_IsStretchKeep = false;
    }

    public void ForceInput()
    {
        //if (m_IsCatching)
        //{
        //    CatchingCancel();
        //}
        //else if (!m_IsCatching)
        //{
        //    m_IsCatching = true;
        //    m_IsStretchKeep = true;
        //    m_IsInputStretch = false;
        //}


        //m_IsCatching = true;
        m_IsStretchKeep = true;
        m_IsInputStretch = false;
        m_NonAssistCatch = true;
    }
}