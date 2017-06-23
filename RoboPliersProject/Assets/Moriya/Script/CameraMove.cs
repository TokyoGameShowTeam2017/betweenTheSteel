/**==========================================================================*/
/**    //aaa
 * カメラの移動
 * このオブジェクトそのものの移動はプレイヤーにくっついて移動するだけ。
 * これの子オブジェクトにカメラの座標用のオブジェクトを作成し、
 * その座標用オブジェクトの位置に実際のメインカメラを移動させる。
 * 作成者：守屋   作成日：17/04/12
/**==========================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour 
{
    /*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField, Tooltip("横移動速度")]
    private float m_MoveSpeedH = 3.0f;
    [SerializeField, Tooltip("縦移動速度")]
    private float m_MoveSpeedV = 3.0f;
    [SerializeField, Tooltip("縦角度最低値値")]
    private float m_AngleVMin = -80.0f;
    [SerializeField, Tooltip("縦角度限界値")]
    private float m_AngleVMax = 50.0f;

    [SerializeField, Tooltip("レイの根元をプレイヤーとして、レイを飛ばし始める距離")]
    private float m_RayStartLength = 0.5f;
    [SerializeField, Tooltip("カメラ線形補完値（数値が低いほどゆっくり）")]
    private float m_PositionLerpValue = 0.4f;
    [SerializeField, Tooltip("カメラ回転線形補完値（数値が低いほどゆっくり）")]
    private float m_RotationLerpValue = 0.02f;

    [Space(5)]
    //[SerializeField, Tooltip("アームマネージャー")]
    //private ArmManager m_ArmManager;

    /*==内部設定変数==*/
    private ArmManager m_ArmManager;
    //プレイヤー
    private GameObject m_Player;
    private PlayerManager m_PlayerManager;
    //実際のカメラ
    private Transform m_CameraTr;
    //カメラの座標用の子オブジェクトのトランスフォーム
    private Transform m_CameraPositionTransform;
    //プレイヤーのトランスフォーム
    private Transform m_PlayerTransform;
    //初期回転量
    private Quaternion m_StartRotation;
    //回転量
    private float m_AngleH = 0.0f;
    private float m_AngleV = 0.0f;
    //プレイヤーとカメラの距離
    private float m_PlayerLength;
    //最終的に実際のカメラに渡す座標
    private Vector3 m_LastCameraPosition;
    //最終的に実際のカメラに渡す回転
    private Quaternion m_LastCameraRotation;
    //アームが向く座標のトランスフォーム
    private Transform m_ArmLook;

    //選択中のアームid
    private int m_EnabledArmID = 0;
    //選択中のアームに向けるための数値
    private float m_SelectArmAngle = 0.0f;

    /// <summary>
    /// 動かせるか？
    /// trueで動かせるようになり、右スティックで操作。
    /// falseで動かなくなり、外部から移動、回転が可能になる。
    /// </summary>
    public bool IsMove { get; set; }

    /*==外部参照変数==*/

    void Awake()
    {
        //コンポーネント取得
        tr = GetComponent<Transform>();
        m_StartRotation = tr.rotation;
    }

    void Start()
    {
        //オブジェクト取得
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
        m_Player = tr.parent.gameObject;
        m_PlayerManager = m_Player.GetComponent<PlayerManager>();
        m_CameraPositionTransform = tr.FindChild("CameraPosition");
        m_CameraTr = m_CameraPositionTransform.FindChild("PlayerCamera");
        m_PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //プレイヤーとカメラの距離計算
        m_PlayerLength = Vector3.Magnitude(m_CameraPositionTransform.position - tr.position);
        m_ArmLook = tr.FindChild("ArmLookPosition").transform;

        IsMove = true;
    }

    void Update()
    {
        if (!IsMove) return;

        switch (m_PlayerManager.GetMoveState())
        {
            case MoveState.NORMAL:
                NormalMove();
                break;

            case MoveState.CATCH:
                CatchMove();
                break;

            case MoveState.NOT:

                break;
        }

        //最終的な結果を計算
        Quaternion q;
        //簡単モード
        if (m_PlayerManager.GetIsEasyMode() && m_ArmManager.GetEnablArmMove().GetIsAimAssisting())
        {
            ////地面との判定
            //m_LastCameraPosition = GroundHitCheck(
            //    m_ArmManager.GetEnablArmMove().GetAimAssistPosition(),
            //    //m_ArmManager.GetEnablArmMove().transform.position + new Vector3(0.0f,1.0f,0.0f) ,
            //     m_CameraPositionTransform.position,
            //    m_RayStartLength, m_PlayerLength);
            ////補完
            //m_LastCameraPosition = Vector3.Lerp(m_CameraTr.position, m_LastCameraPosition, m_PositionLerpValue);
            ////回転
            //q = Quaternion.LookRotation(m_ArmLook.position - m_LastCameraPosition);

            //地面との判定
            m_LastCameraPosition = GroundHitCheck(tr.position, m_CameraPositionTransform.position, m_RayStartLength, m_PlayerLength);
            //補完
            m_LastCameraPosition = Vector3.Lerp(m_CameraTr.position, m_LastCameraPosition, m_PositionLerpValue);
            //回転
            q = m_CameraPositionTransform.rotation;
        }
        //通常モード
        else
        {
            //地面との判定
            m_LastCameraPosition = GroundHitCheck(tr.position, m_CameraPositionTransform.position, m_RayStartLength, m_PlayerLength);
            //補完
            m_LastCameraPosition = Vector3.Lerp(m_CameraTr.position, m_LastCameraPosition, m_PositionLerpValue);
            //回転
            q = m_CameraPositionTransform.rotation;
        }

        m_LastCameraRotation = Quaternion.Slerp(m_LastCameraRotation, q, m_RotationLerpValue);

        //実際のカメラに座標と回転を渡す
        m_CameraTr.position = m_LastCameraPosition;
        m_CameraTr.rotation = m_LastCameraRotation;

    }

    //void LateUpdate()
    //{
    //    switch (m_PlayerManager.GetMoveState())
    //    {
    //        case MoveState.NORMAL:

    //            break;

    //        case MoveState.CATCH:


    //            break;

    //        case MoveState.NOT:

    //            break;
    //    }
    //}

    //void FixedUpdate()
    //{

    //}

    //線分が地面に当たっているかを調べる　当たっている場合はその地点を、当たっていない場合は終点を返す
    private Vector3 GroundHitCheck(Vector3 start, Vector3 end, float startLength, float lineLength)
    {
        Vector3 dir = Vector3.Normalize(end - start);
        Ray ray = new Ray(start + (dir * startLength), dir);
        int mask = LayerMask.NameToLayer("ArmAndPliers");
        RaycastHit hit;
        bool ishit = Physics.Raycast(ray, out hit, lineLength, mask);
        Debug.DrawRay(start + (dir * startLength), dir * lineLength, Color.blue, 1);

        Vector3 result;
        if (ishit)
            result = hit.point;
        else
            result = end;

        return result;
    }

    //通常移動
    private void NormalMove()
    {
        //回転の中心座標をアームの根元に移動
        tr.position = m_ArmManager.GetEnablArm().transform.position;

        //自身を回転させることで、子オブジェクトのカメラ座標も一緒に回転
        Vector2 input = InputManager.GetCameraMove();
        m_AngleH += input.x * m_MoveSpeedH * Time.deltaTime;
        m_AngleV += input.y * m_MoveSpeedV * Time.deltaTime;
        m_AngleV = Mathf.Clamp(m_AngleV, m_AngleVMin, m_AngleVMax);

        tr.rotation = m_StartRotation * Quaternion.AngleAxis(m_AngleH + m_SelectArmAngle, Vector3.up) * Quaternion.AngleAxis(m_AngleV, Vector3.right);

    }

    //動かないオブジェクトを掴んでいるときの移動
    private void CatchMove()
    {
        //現在選択中のアームがオブジェクトを掴んでいるとき
        if(m_ArmManager.GetIsEnablArmCatching())
        {
            //PlayerMoveで一括で計算して回してます

        }
        //他のアームが掴んでいるとき
        else
        {
            //通常通りの移動
            NormalMove();
        }





        //if (InputManager.GetSelectArm().isDown)
        //{
        //    m_EnabledArmID = InputManager.GetSelectArm().id - 1;
        //    m_SelectArmAngle = -90.0f * m_EnabledArmID;
        //}

        ////掴んでいるアームを選択状態
        //if (m_RoboArmBase.GetEnableArm().GetCatchObject() != null)
        //{

        //}

        ////掴んでいないアームを選択（別のアームが掴んでいる）状態
        //else
        //{
        //    //回転の中心座標をアームの根元に移動
        //    tr.position = m_RoboArmBase.GetEnableArm().transform.position;

        //    //自身を回転させることで、子オブジェクトのカメラ座標も一緒に回転
        //    Vector2 input = InputManager.GetCameraMove();
        //    m_AngleH += input.x * m_MoveSpeedH;
        //    m_AngleV += input.y * m_MoveSpeedV;
        //    m_AngleV = Mathf.Clamp(m_AngleV, m_AngleVMin, m_AngleVMax);

        //    tr.rotation = m_StartRotation * Quaternion.AngleAxis(m_AngleH + m_SelectArmAngle, Vector3.up) * Quaternion.AngleAxis(m_AngleV, Vector3.right);



        //    //float inputY = InputManager.GetMove().y;
        //    //if (inputY == 0.0f)
        //    //    inputY = InputManager.GetCameraMove().y;

        //    ////回転
        //    //tr.Rotate(Vector3.right, inputY * m_MoveSpeedV);
        //    ////回転制限
        //    //float rotateY = (tr.eulerAngles.x > 180) ? tr.eulerAngles.x - 360 : tr.eulerAngles.x;
        //    //if (rotateY < m_AngleVMin)
        //    //    tr.eulerAngles = new Vector3(-80, 0, 0);
        //    //else if (rotateY > m_AngleVMax)
        //    //    tr.eulerAngles = new Vector3(50, 0, 0);
        //}




        //回転
        tr.rotation = m_StartRotation * Quaternion.AngleAxis(m_AngleH + m_SelectArmAngle, Vector3.up) * Quaternion.AngleAxis(m_AngleV, Vector3.right);
    }


    /// <summary>
    /// 外部から回転させる場合はこれを使用
    /// </summary>
    public void Rotation(float angleH,float angleV)
    {
        m_AngleH += angleH;
        m_AngleV += angleV;
        m_AngleV = Mathf.Clamp(m_AngleV, m_AngleVMin, m_AngleVMax);

        tr.rotation = m_StartRotation * Quaternion.AngleAxis(m_AngleH + m_SelectArmAngle, Vector3.up) * Quaternion.AngleAxis(m_AngleV, Vector3.right);
    }

    /// <summary>
    /// 座標の補完に使用している値を変更する
    /// 1にすると補完が無くなります
    /// </summary>
    public void SetPositionLerpValue(float value)
    {
        m_PositionLerpValue = value;
    }

    /// <summary>
    /// 座標の補完に使用している値を取得する
    /// </summary>
    public float GetPositionLerpValue()
    {
        return m_PositionLerpValue;
    }

    public Transform GetPlayerCamera()
    {
        return m_CameraTr;
    }
}
