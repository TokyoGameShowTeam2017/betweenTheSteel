/**==========================================================================*/
/**    //aaa
 * プレイヤーの状態制御
 * 作成者：守屋   作成日：17/04/21
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{
    /*==所持コンポーネント==*/


    /*==外部設定変数==*/
    [SerializeField, Tooltip("移動状態")]
    private MoveState m_MoveState;

    [SerializeField, Tooltip("簡単モード　オンオフ")]
    private bool m_IsEasyMode = false;
    [SerializeField, Tooltip("簡単モード エイムアシスト座標のサーチ距離\nこれより遠くのオブジェクトはアシストしない")]
    private float m_AimAssistSearchLength = 6.0f;
    [SerializeField, Tooltip("簡単モード アームの根元からペンチの掴む中心までの最大距離\n（腕を最大まで伸ばした状態での距離をここに設定）")]
    private float m_AimAssistMaxLength = 6.0f;
    [SerializeField, Tooltip("簡単モード アームの伸び初期状態時の、アームの根元からペンチの掴む中心までの距離")]
    private float m_AimAssistPliersLength = 1.7f;
    [SerializeField, Tooltip("簡単モード AimAssistPointの検索時、カメラの正面とみなす最大角度")]
    private float m_AimAssistCameraAngle = 30.0f;

    /*==内部設定変数==*/
    private PlayerMove m_PlayerMove;
    //   [SerializeField, Tooltip("アームマネージャー")]
    private ArmManager m_ArmManager;
    //プレイヤー軸移動用のオブジェクトはここに入れる
    private GameObject m_AxisMoveObject = null;

    //アームの角度限界を超えた量はここに入れる
    private float m_ArmAngleOver = 0.0f;


    /*==外部参照変数==*/
    /// <summary>
    /// プレイヤーを動かせるか？
    /// falseにするとプレイヤーを動かせなくなります
    /// </summary>
    public bool IsMove { get; set; }

    /// <summary>
    /// ハードモードに移行できるか？
    /// </summary>
    public bool IsHardModeAble { get; set; }

    void Awake()
    {
        m_PlayerMove = this.gameObject.GetComponent<PlayerMove>();
        IsMove = true;
        IsHardModeAble = false;

        //シーン遷移の時にPlayerを消さない処理
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();
    }

    void Update()
    {
        //！！！移動後！！！
        //状態変更
        //動かないオブジェクトを１つ以上のアームが掴んでいるなら、NORMAL→CATCHへ移行
        if (m_MoveState == MoveState.NORMAL && m_ArmManager.GetCountCatchingObjects() >= 1)
            m_MoveState = MoveState.CATCH;

        //何も掴んでない状態になったら、CATCH→NORMALへ移行
        else if (m_MoveState == MoveState.CATCH && m_ArmManager.GetCountCatchingObjects() <= 0)
            m_MoveState = MoveState.NORMAL;

        if (!IsMove)
        {
            //ジャンプ動作だけは行う
            m_PlayerMove.TutorialMove();
            return;
        }
        //操作変更
        if (Input.GetKeyDown(KeyCode.Q) && IsHardModeAble)
        {
            m_IsEasyMode = !m_IsEasyMode;
            if (m_IsEasyMode)
                print("Change To Easy");
            else
                print("Change To Hard");
        }
            
        //！！！移動前！！！


        //移動
        m_PlayerMove.Move();
    }


    /// <summary>
    /// 現在の移動状態を返す
    /// </summary>
    public MoveState GetMoveState()
    {
        return m_MoveState;
    }

    
    /// <summary>
    /// 簡単モードかどうかを返す
    /// </summary>
    public bool GetIsEasyMode()
    {
        return m_IsEasyMode;
    }

    /// <summary>
    /// 簡単モード時のエイムアシストする地点の検索距離を返す
    /// </summary>
    public float GetAimAssistSearchLength()
    {
        return m_AimAssistSearchLength;
    }

    /// <summary>
    /// 簡単モード時のエイムアシストする最大距離を返す
    /// </summary>
    public float GetAimAssistMaxLength()
    {
        return m_AimAssistMaxLength;
    }
    
    /// <summary>
    /// 簡単モード時の初期伸びアームからペンチ掴み部分までの距離
    /// </summary>
    public float GetAimAssistPliersLength()
    {
        return m_AimAssistPliersLength;
    }

    /// <summary>
    /// 簡単モード時のAimAssistPointの検索時、カメラの正面とみなす最大角度
    /// </summary>
    public float GetAimAssistCameraAngle()
    {
        return m_AimAssistCameraAngle;
    }

    ///// <summary>
    /////　動かせるオブジェクトをセット
    ///// </summary>
    //public void SetDynamicObject(GameObject obj)
    //{
    //    m_CatchingDynamicObject = obj;
    //}
    ///// <summary>
    ///// 動かせるオブジェクトにnullを入れる
    ///// </summary>
    //public void ReleaseDynamicObject()
    //{
    //    m_CatchingDynamicObject = null;
    //}
    ///// <summary>
    ///// 動かせるオブジェクトを返す
    ///// </summary>
    //public GameObject GetDynamicObject()
    //{
    //    return m_CatchingDynamicObject;
    //}


    ///// <summary>
    /////　動かないオブジェクトをセット
    ///// </summary>
    //public void SetStaticObject(GameObject obj)
    //{
    //    m_CatchingStaticObject = obj;
    //}
    ///// <summary>
    ///// 動かないオブジェクトにnullを入れる
    ///// </summary>
    //public void ReleaseStaticObject()
    //{
    //    m_CatchingStaticObject = null;
    //}
    ///// <summary>
    ///// 動かないオブジェクトを返す
    ///// </summary>
    //public GameObject GetStaticObject()
    //{
    //    return m_CatchingStaticObject;
    //}


    /// <summary>
    ///　プレイヤー軸移動用のオブジェクトにセット
    /// </summary>
    public void SetAxisMoveObject(GameObject obj)
    {
        m_AxisMoveObject = obj;
    }
    /// <summary>
    /// プレイヤー軸移動用のオブジェクトをnullにする
    /// </summary>
    public void ReleaseAxisMoveObject()
    {
        m_AxisMoveObject = null;
    }
    /// <summary>
    /// プレイヤー軸移動用のオブジェクトを返す
    /// オブジェクト名は下記の通り
    /// "RotY"（親）＞"RotX"＞"Pos"　の順
    /// </summary>
    public GameObject GetAxisMoveObject()
    {
        return m_AxisMoveObject;
    }
    /// <summary>
    /// 動かないオブジェクトを掴んでいるときにプレイヤーを動かしたい場合はこれで動かせる
    /// プレイヤー軸移動用のオブジェクトを移動させる
    /// trueなら移動に成功　falseなら移動に失敗
    /// </summary>
    public bool MoveAxisMoveObject(Vector3 velocity)
    {
        if (m_AxisMoveObject != null)
        {
            m_AxisMoveObject.transform.position += velocity;
            return true;
        }
        else
            return false;
    }


    /// <summary>
    /// アームの角度限界を超えている場合はこれで値をセット
    /// </summary>
    public void SetArmAngleOver(float value)
    {
        m_ArmAngleOver = value;
    }

    /// <summary>
    /// アームの角度限界を超えた角度量を返す
    /// </summary>
    public float GetArmAngleOver()
    {
        return m_ArmAngleOver;
    }

    public PlayerMove GetPlayerMove()
    {
        return m_PlayerMove;
    }

    [ContextMenu("IsMove Switch")]
    private void IsMoveSwitch()
    {
        IsMove = !IsMove;
    }

    /// <summary>
    /// プレイヤー、カメラの移動可能状態と、ペンチＵＩの表示状態を一斉に変更する
    /// ステージ開始時のプレイヤー動ける／動けない状態の切り替え等に使用
    /// </summary>
    public void SetIsMoveAndUI(bool value)
    {
        IsMove = value;
        GameObject.Find("CameraMove").GetComponent<CameraMove>().IsMove = value;
       // m_ArmManager.SetUIVisible(value);
    }
}
