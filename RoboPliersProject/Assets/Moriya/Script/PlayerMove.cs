﻿/**==========================================================================*/
/**    //aaa
 * プレイヤーキャラクターの移動
 * 作成者：守屋   作成日：17/04/11
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;
    private Rigidbody rb;
    private CharacterController cc;

    /*==外部設定変数==*/
    [SerializeField, Tooltip("通常移動速度")]
    private float m_WalkSpeed = 3.0f;
    [SerializeField, Tooltip("ダッシュ時移動速度")]
    private float m_DashSpeed = 6.0f;
    [SerializeField, Tooltip("ジャンプ力")]
    private float m_JumpPower = 300.0f;
    [SerializeField, Tooltip("重力")]
    private float m_Gravity = 10.0f;
    [SerializeField, Tooltip("動かないオブジェクト掴んだ時の軸移動速度")]
    private float m_AxisMoveSpeed = 30.0f;
    [SerializeField, Tooltip("動かないオブジェクト掴んだ時の移動角度制限値")]
    private float m_AxisMoveAngleLimit = 65.0f;
    //[SerializeField, Tooltip("動かないオブジェクト掴んだ時の高さ制限値")]
    //private float m_AxisMoveHeightLimit = 2.0f;

    [SerializeField, Tooltip("動かないオブジェクト掴んだ時のペンチ座標ずらし値")]
    private float m_PliersOffset = 1.0f;


    public float WalkSpeed { get { return m_WalkSpeed; } }

    [Space(5)]
    [SerializeField, Tooltip("地面との判定線の開始点の高さ")]
    private float m_RayStartHeight = 0.0f;
    [SerializeField, Tooltip("地面との判定線の長さ")]
    private float m_RayLength = 1.0f;
    [SerializeField, Tooltip("回転の速さ調整値(1で回転最速、それ以上でゆっくり回転)")]
    private float m_NormalMoveRotationValue = 5.0f;

    [Space(5)]

    [SerializeField, Tooltip("カメラ")]
    private Transform m_CameraTransform;
    [SerializeField, Tooltip("ジャンプ時に使用する判定")]
    private LegCollision[] m_LegCollisions;
    [SerializeField, Tooltip("地面に接してる脚が1本以下のときの押し出しの強さ")]
    private float m_PushPower = 100.0f;


    /*==内部設定変数==*/
    //[SerializeField, Tooltip("アームマネージャー")]
    private ArmManager m_ArmManager;
    private PlayerManager m_Manager;
    //カメラのトランスフォーム
    //カメラの移動
    private CameraMove m_CameraMove;

    //移動速度
    private float m_MoveSpeed;
    //移動ベクトルの最終計算結果
    private Vector3 m_LastVelocity = Vector3.zero;
    //ジャンプ可能か？
    private bool m_IsJumpPossible = false;
    //現在の重力による移動量
    private float m_GravityMoveValue = 0.0f;
    //地面との接触判定経過フレーム数
    private int m_GroundHitFrameNum = 0;
    private int m_GroundHitFrameNumPrev = 0;

    //向ける地点
    private Transform m_LookTr;
    //軸回転保存用
    private Vector3 m_PrevRotYEuler;

    //着地した瞬間用
    private bool m_PrevIsHit;

    //ジャンプさせるときのベクトル
    private Vector3 m_JumpVector = Vector3.up;
    //XZ強さ
    private float m_JumpPowerXZ = 0.0f;

    //地面に接しているか？
    public bool IsGround { get; set; }
    //アーム移動入力Y方向
    public float ArmInputY { get; set; }

    //4本の足からもらった地面との判定情報はここへ
    private RaycastHit m_HitInfo;
    //private Vector3 m_PlayerToLeg;
    //押し出す方向はここへ
    private Vector3 m_PushVec;


    //初期段差オフセット値
    private float m_StartStepOffset;

    //カプセルに当たった地点
    public Vector3 HitPosition { get; set; }
    public bool IsHit { get; set; }

    void Awake()
    {
        //コンポーネント取得
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        m_Manager = GetComponent<PlayerManager>();

        HitPosition = Vector3.zero;
        IsHit = false;

    }

    void Start()
    {
        //外部オブジェクト取得
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        m_LookTr = GameObject.Find("ArmLookPosition").transform;
        m_CameraMove = m_CameraTransform.GetComponent<CameraMove>();
        SceneLoader.GetInstance()._playerMove = this;

        m_MoveSpeed = m_WalkSpeed;
        m_StartStepOffset = cc.stepOffset;
    }

    void LateUpdate()
    {
        if (!m_Manager.IsMove) return;

        //print(m_ArmManager.GetEnablePliersRollValue());
        switch (m_Manager.GetMoveState())
        {
            case MoveState.NORMAL:
                //アームの角度限界を超えた分だけ回転
                //tr.Rotate(Vector3.up, m_Manager.GetArmAngleOver() / m_NormalMoveRotationValue);

                //
                //tr.Rotate(Vector3.up,InputManager.GetZC() * 3.0f);

                //アームの向く先に向ける
                tr.LookAt(m_LookTr.position);
                tr.eulerAngles = new Vector3(0.0f, tr.eulerAngles.y, 0.0f);
                break;

            case MoveState.CATCH:

                break;

            case MoveState.NOT:

                break;
        }

        if (tr.position.y <= -40.0f)
            tr.position = Vector3.zero;
    }


    //void FixedUpdate()
    //{
    //    switch (m_Manager.GetMoveState())
    //    {
    //        case MoveState.NORMAL:
    //            //アームの角度限界を超えた分だけ回転
    //            //tr.Rotate(Vector3.up, m_Manager.GetArmAngleOver() / m_NormalMoveRotationValue);

    //            //
    //            ////tr.Rotate(Vector3.up,InputManager.GetZC() * 3.0f);
    //            //tr.eulerAngles = new Vector3(0.0f, tr.eulerAngles.y, 0.0f);
    //            break;

    //        case MoveState.CATCH:

    //            break;

    //        case MoveState.NOT:

    //            break;
    //    }
    //}


    void OnControllerColliderHit(ControllerColliderHit col)
    {
        IsHit = true;
        HitPosition = col.point;
        //押し出す方向を決定
        m_PushVec = tr.position - col.point;
        m_PushVec.y = 0;
        m_PushVec.Normalize();

        //Rodのボーンに当たったとき
        if (col.gameObject.name.Substring(0, 4) == "Bone")
        {
            m_PushVec *= 2.0f;
        }

        //重力をリセット
        if (m_GravityMoveValue <= -5.0f)//ジャンプ直後にリセットするのの防止
            m_GravityMoveValue = 0.0f;

        ////上昇中に衝突したら落下させる
        //if (m_GravityMoveValue > 0)
        //{
        //    m_JumpVector = Vector3.up;
        //    m_JumpPowerXZ = 0.0f;
        //    m_GravityMoveValue = 0.0f;
        //}
    }

    ////床とヒット中の処理
    //void OnControllerColliderHit(ControllerColliderHit col)
    //{
    //    m_IsJumpPossible = true;
    //    m_GravityMoveValue = 0.0f;
    //    m_GroundHitFrameNum++;
    //}

    ////床から離れたときの処理
    //private void ControllerColliderExit()
    //{
    //    print("called");
    //    cc.stepOffsetm_StartStepOffset;
    //}


    //通常移動
    private void NormalMove()
    {
        //入力取得
        Vector2 input = InputManager.GetMove();

        //移動方向基準取得
        Vector3 v = m_CameraTransform.forward;
        Vector3 front = new Vector3(v.x, 0.0f, v.z).normalized;
        Vector3 right = m_CameraTransform.right;

        //移動方向、速度計算
        m_LastVelocity = front * input.y + right * input.x;
        //if (InputManager.GetDash())
        //    m_MoveSpeed = m_DashSpeed;
        //else
        //    m_MoveSpeed = m_WalkSpeed;

        //移動量を掛ける
        m_LastVelocity *= m_MoveSpeed;


        //入力取得
        ArmInputY = InputManager.GetCameraMove().y;
    }

    //ジャンプ移動
    private void JumpMove()
    {

        ////レイを飛ばす
        //Vector3 dir = -tr.up;
        //Ray ray = new Ray(tr.position - tr.forward, dir);
        //int mask = LayerMask.NameToLayer("ArmAndPliers");
        //RaycastHit hit;
        //bool ishit = Physics.Raycast(ray, out hit, 2.0f, mask);
        //IsGround = ishit;

        bool ishit = IsLegCollisionAnyHit();
        IsGround = ishit;

        //ずり落ちる方向
        Vector3 pushvelocity = Vector3.zero;
        //触れている足の数が1本以下の場合はずり落ちる
        if (LegCollisionHitCount() <= 1)
        {
            pushvelocity = m_PushVec * m_PushPower * Time.deltaTime;
        }
        else
        {
            pushvelocity = m_PushVec = Vector3.zero;
        }

        //着地中の処理
        if (m_GravityMoveValue < 0 && ishit)
        {
            //着地時の音再生
            if (!m_PrevIsHit)
                SoundManager.Instance.PlaySe("xg-2landing");

            m_GravityMoveValue = 0.0f;

            m_JumpVector = Vector3.up;
            m_JumpPowerXZ = 0.0f;

            //入力取得
            if (InputManager.GetJump() && LegCollisionHitCount() >= 2 && m_Manager.IsMove)
            {
                SoundManager.Instance.PlaySe("xg-2jump");
                m_IsJumpPossible = false;
                m_GravityMoveValue = m_JumpPower;
            }

            if (m_HitInfo.transform.tag == "Beltconveyor")
                m_LastVelocity += m_HitInfo.transform.GetComponent<BeltConveyor>().GetBeltVelocity();
        }


        m_GravityMoveValue -= m_Gravity * Time.deltaTime;
        //ジャンプ移動処理
        Vector3 jumpv = m_JumpVector;
        jumpv *= m_JumpPowerXZ;
        jumpv.y = m_GravityMoveValue;
        m_LastVelocity += jumpv + pushvelocity;

        //いままでのジャンプ移動処理
        //m_LastVelocity.y = m_GravityMoveValue;




        ////ジャンプをせずに床から離れたかを判定
        //if(m_GroundHitFrameNum != 0 && m_GroundHitFrameNum == m_GroundHitFrameNumPrev)
        //{
        //    //床から離れたときの処理
        //    ControllerColliderExit();
        //}
        m_GroundHitFrameNumPrev = m_GroundHitFrameNum;
        m_PrevIsHit = ishit;
    }

    //動かないオブジェクトを掴んだ時の軸移動
    private void CatchMove()
    {
        m_GravityMoveValue = 0.0f;
        //CharacterControllerを使った前後左右移動は行わない(あたり判定を行わせるために動かす)
        m_LastVelocity = new Vector3(0.0f, -0.5f, 0.0f);

        //2本以上のアームが動かないオブジェクトを掴んでいるとき
        if (m_ArmManager.GetCountCatchingObjects() >= 2)
        {
            //現状何もしない
            print("called");

        }
        //選択中のアームが掴んでいるなら
        else if (m_ArmManager.GetIsEnablArmCatching())
        {
            TestAxisMove();

        }
        //選択中以外のアームが掴んでいるなら
        else
        {
            //水平回転のみ行う
            float inputX = InputManager.GetMove().x;

            Transform roty = m_Manager.GetAxisMoveObject().transform;
            Transform prilesposTr = roty.FindChild("PliersRotX").FindChild("PliersPos");
            float h = -inputX * m_AxisMoveSpeed * Time.deltaTime;
            //水平回転
            roty.localEulerAngles += new Vector3(0.0f, h, 0.0f);
            tr.localEulerAngles += new Vector3(0.0f, h, 0.0f);

 

            //キャッチしているアームを特定
            int catchid = m_ArmManager.GetIsCatchArmID();

            //アームの伸び縮みを考慮した移動
            Transform armtr = m_ArmManager.GetArmByID(catchid).transform;
            Vector3 armforward = armtr.forward;
            float armlength = m_ArmManager.GetArmMoveByID(catchid).GetArmStretch();

            //アーム根元座標からプレイヤー座標に向かうベクトル
            Vector3 arm2player = tr.position - armtr.position;
            //掴んだ座標からペンチの根元座標に向かうベクトル
            Vector3 catch2pliers = prilesposTr.position - roty.position;
            Vector3 offset = catch2pliers.normalized * m_PliersOffset;

            Vector3 pos =
                roty.position   //キャッチした座標
                + catch2pliers  //キャッチした座標からペンチまでのベクトル
                + -armforward * armlength   //アームの伸びベクトル
                + arm2player   //アームの根元からプレイヤー座標までのベクトル
                + offset;

            tr.position = pos;


            //壁抜け防止
            Vector3 dir;
            if (h > 0)
                dir = -armtr.right;
            else
                dir = armtr.right;

            bool iswallhit = false;
            for (int i = 0; i < 9; i++)
            {
                float x = ((float)i - 4.0f) / 4.0f;
                Vector3 o = armtr.forward * x;
                Ray ray = new Ray((tr.position + o) - dir, dir);
                int mask = LayerMask.NameToLayer("ArmAndPliers");
                RaycastHit hit;
                iswallhit = Physics.Raycast(ray, out hit, 3.0f, mask);
                if (iswallhit)
                {
                    roty.localEulerAngles -= new Vector3(0.0f, h, 0.0f);
                    tr.localEulerAngles -= new Vector3(0.0f, h, 0.0f);
                    break;
                }
            }





            ////以前
            //m_Manager.GetAxisMoveObject().transform.Rotate(tr.up, inputX * m_AxisMoveSpeed * Time.deltaTime);
            ////計算後のトランスフォーム
            //Transform posTr = m_Manager.GetAxisMoveObject().transform.FindChild("RotX").FindChild("Pos");
            ////移動
            //tr.position = posTr.position;

            //Vector3 euler = posTr.rotation.eulerAngles;
            //euler.x = euler.z = 0.0f;
            ////回転計算
            //tr.eulerAngles += euler - m_PrevRotYEuler;
            //m_PrevRotYEuler = euler;
        }
    }

    /// <summary>
    /// 移動実行処理
    /// </summary>
    public void Move()
    {
        switch (m_Manager.GetMoveState())
        {
            case MoveState.NORMAL:
                NormalMove();
                JumpMove();
                break;

            case MoveState.CATCH:
                CatchMove();
                break;

            case MoveState.NOT:

                break;
        }

        //print(m_LastVelocity);
        //CharacterControllerを通して移動する

        cc.Move(m_LastVelocity * Time.deltaTime);

        IsHit = false;
    }


    /// <summary>
    /// 外部から強制的にジャンプさせる
    /// </summary>
    public void Jump(float jumpPower)
    {
        m_IsJumpPossible = false;
        m_GravityMoveValue = jumpPower;
    }
    public void Jump(Vector3 v, float powerXZ, float powerY)
    {
        m_IsJumpPossible = false;
        m_JumpVector = v;
        m_JumpPowerXZ = powerXZ;
        m_GravityMoveValue = powerY;
    }


    private void TutoJump()
    {
        JumpMove();
        Vector3 v = m_LastVelocity;
        v.x = 0.0f;
        v.z = 0.0f;
        cc.Move(v * Time.deltaTime);    

    }


    private void TutoCatch()
    {
        m_GravityMoveValue = 0.0f;
        m_LastVelocity = Vector3.zero;

        //軸回転用のオブジェクトを回し、その計算後のトランスフォームを使ってプレイヤーの座標と回転を決定する
        Transform roty = m_Manager.GetAxisMoveObject().transform;
        Transform rotx = roty.FindChild("RotX");
        Transform pliersrotx = roty.FindChild("PliersRotX");

        //プレイヤー本体の移動計算
        //回転計算後のプレイヤー座標用のトランスフォーム取得
        Transform posTr = rotx.FindChild("Pos");
        Transform prilesposTr = roty.FindChild("PliersRotX").FindChild("PliersPos");
        //アームの伸び縮みを考慮した移動
        Transform armtr = m_ArmManager.GetEnablArm().transform;
        Vector3 armforward = armtr.forward;
        float armlength = m_ArmManager.GetEnablArmMove().GetArmStretch();
        //アーム根元座標からプレイヤー座標に向かうベクトル
        Vector3 arm2player = tr.position - armtr.position;
        //掴んだ座標からペンチの根元座標に向かうベクトル
        Vector3 catch2pliers = prilesposTr.position - roty.position;
        Vector3 offset = catch2pliers.normalized * m_PliersOffset;
        Vector3 pos =
            roty.position   //キャッチした座標
            + catch2pliers  //キャッチした座標からペンチまでのベクトル
            + -armforward * armlength   //アームの伸びベクトル
            + arm2player   //アームの根元からプレイヤー座標までのベクトル
            + offset;
        tr.position = pos;
    }

    /// <summary>
    /// チュートリアル用の移動制限掛かったときの動き
    /// </summary>
    public void TutorialMove()
    {
        switch (m_Manager.GetMoveState())
        {
            case MoveState.NORMAL:
                TutoJump();
                break;

            case MoveState.CATCH:
                TutoCatch();
                break;

            case MoveState.NOT:

                break;
        }
    }

    /// <summary>
    /// 足のあたり判定のどれかが地面に当たっているかを取得
    /// </summary>
    /// <returns></returns>
    private bool IsLegCollisionAnyHit()
    {
        bool result = false;
        foreach (LegCollision col in m_LegCollisions)
        {
            if (col.IsHit)
            {
                result = true;
                m_HitInfo = col.HitInfo;
            }
                
        }
        return result;
    }

    /// <summary>
    /// 足のあたり判定がすべて地面に当たっているかを取得
    /// </summary>
    /// <returns></returns>
    private bool IsLegCollisionAllHit()
    {
        bool result = true;
        foreach(LegCollision col in m_LegCollisions)
        {
            if (!col.IsHit)
                result = false;
        }
        return result;
    }


    /// <summary>
    /// 足のあたり判定の地面に当たっている数を取得
    /// </summary>
    /// <returns></returns>
    private int LegCollisionHitCount()
    {
        int result = 0;
        foreach (LegCollision col in m_LegCollisions)
        {
            if (col.IsHit)
            {
                result++;
                //m_PlayerToLeg = -col.ToPlayerVec;
            }
                
        }
        return result;
    }


    private void AxisMove()
    {
        //軸回転用のオブジェクトを回し、その計算後のトランスフォームを使ってプレイヤーの座標と回転を決定する
        Transform roty = m_Manager.GetAxisMoveObject().transform;
        Transform rotx = roty.FindChild("RotX");
        Transform pliersrotx = roty.FindChild("PliersRotX");

        //入力取得
        Vector2 leftInput, rightInput;
        leftInput = InputManager.GetMove();
        rightInput = InputManager.GetCameraMove();
        //右スティックを使ったか？
        bool isRightStick = (rightInput.magnitude > 0.0f);

        //水平、垂直回転量
        float h = 0, v = 0;


        //プレイヤー本体の移動計算
        //回転計算後のプレイヤー座標用のトランスフォーム取得
        Transform posTr = rotx.FindChild("Pos");
        Transform prilesposTr = roty.FindChild("PliersRotX").FindChild("PliersPos");

        ////いままでの移動
        //tr.position = posTr.position;

        //アームの伸び縮みを考慮した移動
        Transform armtr = m_ArmManager.GetEnablArm().transform;
        Vector3 armforward = armtr.forward;
        float armlength = m_ArmManager.GetEnablArmMove().GetArmStretch();

        //アーム根元座標からプレイヤー座標に向かうベクトル
        Vector3 arm2player = tr.position - armtr.position;
        //掴んだ座標からペンチの根元座標に向かうベクトル
        Vector3 catch2pliers = prilesposTr.position - roty.position;
        Vector3 offset = catch2pliers.normalized * m_PliersOffset;

        Vector3 pos =
            roty.position   //キャッチした座標
            + catch2pliers  //キャッチした座標からペンチまでのベクトル
            + -armforward * armlength   //アームの伸びベクトル
            + arm2player   //アームの根元からプレイヤー座標までのベクトル
            + offset;

        tr.position = pos;


        //操作がないならここで終了
        if (leftInput.magnitude == 0.0f && rightInput.magnitude == 0.0f)
            return;


        float inputX = leftInput.x;
        if (inputX == 0.0f)
            inputX = rightInput.x;

        float inputY = leftInput.y;
        if (inputY == 0.0f)
            inputY = rightInput.y;

        ArmInputY = inputY;



        Vector3 euler = posTr.rotation.eulerAngles;
        euler.x = euler.z = 0.0f;


        h = -inputX * m_AxisMoveSpeed * Time.deltaTime;
        if (isRightStick)
            h = -h;
        v = -inputY * m_AxisMoveSpeed * Time.deltaTime;





        //左スティックの場合
        if (!isRightStick)
        {
            //回転計算
            tr.eulerAngles += euler - m_PrevRotYEuler;
            m_PrevRotYEuler = euler;
        }
        //右スティックの場合
        else
        {
            float over = m_Manager.GetArmAngleOver();
            //アームの角度限界を超えている場合、左スティックと同じ計算をして以降の処理を行わない
            if ((inputX > 0.0f && over > 0.0f) ||
                (inputX < 0.0f && over < 0.0f))
            {
                //水平回転
                roty.localEulerAngles += new Vector3(0.0f, h, 0.0f);
                m_CameraMove.Rotation(h, 0.0f);
                //回転計算
                tr.eulerAngles += euler - m_PrevRotYEuler;
                m_PrevRotYEuler = euler;
                return;
            }

            m_PrevRotYEuler = euler;
        }

        //水平回転
        roty.localEulerAngles += new Vector3(0.0f, h, 0.0f);
        //垂直回転
        rotx.localEulerAngles += new Vector3(v, 0.0f, 0.0f);
        pliersrotx.localEulerAngles += new Vector3(v, 0.0f, 0.0f);



        //地面との判定
        if (v > 0.0f)
        {
            Vector3 dir = -tr.up;
            Ray ray = new Ray(tr.position + tr.up, dir);
            int mask = LayerMask.NameToLayer("ArmAndPliers");
            RaycastHit hit;
            bool ishit = Physics.Raycast(ray, out hit, 1.4f, mask);
            IsGround = ishit;
            if (ishit)
            {//地面と当たっているなら押し返す
                rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
                pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
            }
        }


        //上または下に行き過ぎないように制限
        float angle = 180.0f - (rotx.eulerAngles.x - 180.0f);

        if (angle > 180.0f)
            angle -= 360.0f;
        //print(angle);

        if (angle > m_AxisMoveAngleLimit || angle < -m_AxisMoveAngleLimit)
        {
            rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
            pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        }

        //float angle = armtr.localEulerAngles.x;
        //if (angle > 180.0f)
        //    angle -= 360.0f;
        //print(angle);

        //if ((v < 0 && angle > m_AxisMoveAngleLimit) ||
        //    (v > 0 && angle < -m_AxisMoveAngleLimit))
        //{
        //    rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        //    pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        //}



        //カメラを回転
        if (inputX != 0.0f)
            m_CameraMove.Rotation(h, 0.0f);
        if (isRightStick)
            m_CameraMove.Rotation(0.0f, -v);
    }

    private void TestAxisMove()
    {
        //軸回転用のオブジェクトを回し、その計算後のトランスフォームを使ってプレイヤーの座標と回転を決定する
        Transform roty = m_Manager.GetAxisMoveObject().transform;
        Transform rotx = roty.FindChild("RotX");
        Transform pliersrotx = roty.FindChild("PliersRotX");

        //プレイヤー本体の移動計算
        //回転計算後のプレイヤー座標用のトランスフォーム取得
        Transform posTr = rotx.FindChild("Pos");
        //掴んだ後のペンチの根元座標
        Transform prilesposTr = pliersrotx.FindChild("PliersPos");

        //アームの伸び縮みを考慮した移動
        Transform armtr = m_ArmManager.GetEnablArm().transform;
        //掴んだ座標からペンチの根元座標に向かうベクトル
        Vector3 catch2pliers = prilesposTr.position - roty.position;
        tr.position =
            roty.position   //キャッチした座標
            + catch2pliers  //キャッチした座標からペンチまでのベクトル
            + -armtr.forward * m_ArmManager.GetEnablArmMove().GetArmStretch()   //アームの伸びベクトル
            + tr.position - armtr.position   //アームの根元からプレイヤー座標までのベクトル
            + catch2pliers.normalized * m_PliersOffset;


        //入力取得
        Vector2 leftInput, rightInput;
        leftInput = InputManager.GetMove();
        rightInput = InputManager.GetCameraMove();
        //右スティックを使ったか？
        bool isRightStick = (rightInput.magnitude > 0.0f);

        //操作がないならここで終了
        if (leftInput.magnitude == 0.0f && rightInput.magnitude == 0.0f)
            return;

        //入力確定
        float inputX = leftInput.x;
        if (inputX == 0.0f)
            inputX = rightInput.x;

        float inputY = leftInput.y;
        if (inputY == 0.0f)
            inputY = rightInput.y;

        ArmInputY = inputY;


        //水平、垂直回転量計算
        float h = 0, v = 0;
        h = -inputX * m_AxisMoveSpeed * Time.deltaTime;
        if (isRightStick)
            h = -h;
        v = -inputY * m_AxisMoveSpeed * Time.deltaTime;

        //プレイヤー回転用の回転取得
        Vector3 euler = posTr.rotation.eulerAngles;
        //Vector3 euler = tr.eulerAngles;
        euler.x = euler.z = 0.0f;

        //左スティックの場合
        if (!isRightStick)
        {
            //回転計算
            tr.eulerAngles += euler - m_PrevRotYEuler;
            m_PrevRotYEuler = euler;

            //Vector3 e = GameObject.Find("PlayerCamera").transform.eulerAngles;
            //e.x = e.z = 0.0f;
            //tr.eulerAngles = e;
            //m_ArmManager.BaseLookCameraFront();

        }
        //右スティックの場合
        else
        {
            float over = m_Manager.GetArmAngleOver();
            //アームの角度限界を超えている場合、左スティックと同じ計算をして以降の処理を行わない
            if ((inputX > 0.0f && over > 0.0f) ||
                (inputX < 0.0f && over < 0.0f))
            {
                //水平回転
                roty.localEulerAngles += new Vector3(0.0f, h, 0.0f);
                m_CameraMove.Rotation(h, 0.0f);
                //回転計算
                tr.eulerAngles += euler - m_PrevRotYEuler;
                m_PrevRotYEuler = euler;
                return;
            }
            m_PrevRotYEuler = euler;
        }

        //水平回転
        roty.localEulerAngles += new Vector3(0.0f, h, 0.0f);

        //左右に壁があるなら押し返し
        //if(h != 0.0f)
        //{
        //    if (IsHit)
        //    {
        //        //ヒットした地点への方向
        //        Vector3 dir = Vector3.Normalize(HitPosition - tr.position);
        //        //左右判定
        //        float cos = Vector3.Cross(tr.forward, dir).y;

        //        print("cos:" + cos);

        //        if ((h > 0 && cos > 0) ||
        //            (h < 0 && cos < 0))
        //        {
        //            roty.localEulerAngles -= new Vector3(0.0f, h, 0.0f);
        //            m_CameraMove.Rotation(-h, 0.0f);
        //            print("called");
        //        }
        //    }
        //}

        //if (h != 0.0f)
        //{
        //    Vector3 dir;
        //    if (h > 0)
        //        dir = -armtr.right;
        //    else
        //        dir = armtr.right;

        //    Ray ray = new Ray(tr.position + tr.up, dir);
        //    int mask = LayerMask.NameToLayer("ArmAndPliers");
        //    RaycastHit hit;
        //    bool iswallhit = Physics.Raycast(ray, out hit, 1.0f, mask);
        //    if (iswallhit)
        //    {
        //        roty.localEulerAngles -= new Vector3(0.0f, h, 0.0f);
        //        m_CameraMove.Rotation(-h, 0.0f);
        //    }
        //}

        if (h != 0.0f)
        {
            Vector3 dir;
            if (h > 0)
                dir = -armtr.right;
            else
                dir = armtr.right;


            bool iswallhit = false;
            for(int i = 0;i<9;i++)
            {
                float x = ((float)i - 4.0f) / 4.0f;
                Vector3 offset = armtr.forward * x;
                Ray ray = new Ray((tr.position + offset) - dir, dir);
                int mask = LayerMask.NameToLayer("ArmAndPliers");
                RaycastHit hit;
                iswallhit = Physics.Raycast(ray, out hit, 3.0f, mask);
                if (iswallhit)
                {
                    roty.localEulerAngles -= new Vector3(0.0f, h, 0.0f);
                    m_CameraMove.Rotation(-h, 0.0f);
                    break;
                }
            }

        }



        //垂直回転
        //rotx.localEulerAngles += new Vector3(v, 0.0f, 0.0f);
        pliersrotx.localEulerAngles += new Vector3(v, 0.0f, 0.0f);


        //地面との判定
        if (v > 0.0f)
        {
            //Vector3 dir = -tr.up;
            //Ray ray = new Ray(tr.position + tr.up, dir);
            //int mask = LayerMask.NameToLayer("ArmAndPliers");
            //RaycastHit hit;
            //bool ishit = Physics.Raycast(ray, out hit, 1.4f, mask);
            bool ishit = IsLegCollisionAnyHit();
            IsGround = ishit;
            if (ishit)
            {//地面と当たっているなら押し返す
                //rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
                pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
            }
        }


        //上または下に行き過ぎないように制限
        //float angle = 180.0f - (rotx.eulerAngles.x - 180.0f);
        float angle = 180.0f - (pliersrotx.eulerAngles.x - 180.0f);

        if (angle > 180.0f)
            angle -= 360.0f;
        //if (angle > m_AxisMoveAngleLimit || angle < -m_AxisMoveAngleLimit)
        //{
        //    rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        //    pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        //}

        if ((v < 0 && angle > m_AxisMoveAngleLimit) ||
            (v > 0 && angle < -m_AxisMoveAngleLimit))
        {
            //rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
            pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        }

        //float angle = armtr.localEulerAngles.x;
        //if (angle > 180.0f)
        //    angle -= 360.0f;
        //print(angle);
        //print("v < 0:" + (v < 0));
        //print("over :" + (angle > m_AxisMoveAngleLimit));
        //if ((v < 0 && angle > m_AxisMoveAngleLimit) ||
        //    (v > 0 && angle < -m_AxisMoveAngleLimit))
        //{
        //    rotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        //    pliersrotx.localEulerAngles -= new Vector3(v, 0.0f, 0.0f);
        //}




        //カメラを回転
        if (inputX != 0.0f)
            m_CameraMove.Rotation(h, 0.0f);
        if (isRightStick)
            m_CameraMove.Rotation(0.0f, -v);
    }
}
