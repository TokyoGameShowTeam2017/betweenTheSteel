/**==========================================================================*/
/**    //aaa
 * ペンチの処理
 * 作成者：守屋   作成日：17/04/21
/**==========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersMove : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField, Tooltip("ペンチ左")]
    private Transform m_PliersLeft;
    [SerializeField, Tooltip("ペンチ右")]
    private Transform m_PliersRight;



    [Space(5)]
    [SerializeField, Tooltip("挟むまでの最大移動距離")]
    private float m_MaxMoveLength = 0.3f;
    //[SerializeField, Tooltip("掴む力")]
    //private float m_CatchPower = 1.0f;
    [SerializeField, Tooltip("動かないオブジェクトを掴んだときの、自身からの掴んだ座標生成地点への距離")]
    private float m_CatchLength = 1.0f;

    [Space(5)]
    [SerializeField, Tooltip("RotXの生成座標(RotYからの相対距離) アームの根元座標とプレイヤーの座標の高さの差をここに入力")]
    private Vector3 m_RotXCreatePos = new Vector3(0.0f, -1.25f, 0.0f);

    /*==内部設定変数==*/
    //[SerializeField, Tooltip("アームマネージャー")]
    private ArmManager m_ArmManager;
    private PlayerManager m_PlayerManager;
    //[SerializeField, Tooltip("ペンチ用ＵＩのメーター棒")]
    private RectTransform m_PliersGaugeArrow;
    private float m_GaugeArrowValue = -1.0f;
    private float m_GaugeArrowTargetValue = -1.0f;

    //自身のID
    private int m_ID;

    //プレイヤー
    private GameObject m_Player;

    //自身の初期ローカル座標
    private Vector3 m_InitLocalPos;

    //ペンチのRigidbody
    private Rigidbody m_LeftRB;
    private Rigidbody m_RightRB;

    //開始地点
    private Vector3 m_LeftStartPosition;
    private Vector3 m_RightStartPosition;
    //終了地点
    private Vector3 m_LeftEndPosition;
    private Vector3 m_RightEndPosition;

    //掴む強さ計算用
    private float m_Power = 0.0f;


    //掴める範囲内に入ってきたオブジェクト
    private CatchObject m_HitObject;
    private CutRodCollision m_HitCutObject;
    private Kusari m_HitKusariObject;
    //掴んだオブジェクト
    private CatchObject m_CatchObject;
    //手放した（以前にキャッチした）オブジェクト
    private GameObject m_ReleasedObject;
    //掴んだオブジェクトの大本の親
    private Transform m_CatchParent;



    //プレイヤー軸回転用のオブジェクト
    private GameObject m_PlayerAxisMoveY;
    private GameObject m_PlayerAxisMoveX;
    //プレイヤーの位置計算用オブジェクト
    private GameObject m_AxisMovePlayerPosition;
    private GameObject m_AxisMovePliersPosition;

    //オブジェクトを掴んでいるか？
    private bool m_IsCatch = false;

    //ペンチのZ軸回転量
    private float m_Roll;

    //１フレーム毎の移動量
    private Vector3 m_MoveVelocity;
    private Vector3 m_PrevPosition;

    //入力取得用
    private bool m_LateInput = false;
    private bool m_Input = false;
    private bool m_LateEasyMoveInput = false;
    private bool m_EasyMoveInput = false;

    //2つで挟んで親子関係が変わった際の変数
    private bool m_ParentNotCathFlag;
    private GameObject m_SeveObject;

    private float m_StartPositionZ;

    //簡単モード用　掴み中か？
    private bool m_IsEasyModeCatching = false;

    //左右のアームのBoxCollider
    private BoxCollider m_LeftColl;
    private BoxCollider m_RightColl;

    private TutorialSetting m_TutorialSetting;

    private Vector3 m_CatchObjPrevPos;
    //掴んだオブジェクトの1フレ移動量
    private Vector3 m_CatchObjVelocity;

    /*==外部参照変数==*/



    void Awake()
    {
        tr = GetComponent<Transform>();

        m_LeftRB = m_PliersLeft.GetComponent<Rigidbody>();
        m_RightRB = m_PliersRight.GetComponent<Rigidbody>();

        m_LeftStartPosition = m_PliersLeft.localPosition;
        m_RightStartPosition = m_PliersRight.localPosition;

        m_LeftEndPosition = m_PliersLeft.localPosition + Vector3.right * m_MaxMoveLength;
        m_RightEndPosition = m_PliersRight.localPosition - Vector3.right * m_MaxMoveLength;

        m_LeftColl = m_PliersLeft.GetComponent<BoxCollider>();
        m_RightColl = m_PliersRight.GetComponent<BoxCollider>();
    }

    void Start()
    {
        m_ArmManager = GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>();


        switch(m_ID)
        {
            case 0: m_PliersGaugeArrow = GameObject.Find("Ygaugearrow").GetComponent<RectTransform>(); break;
            case 1: m_PliersGaugeArrow = GameObject.Find("Agaugearrow").GetComponent<RectTransform>(); break;
            case 2: m_PliersGaugeArrow = GameObject.Find("Bgaugearrow").GetComponent<RectTransform>(); break;
            case 3: m_PliersGaugeArrow = GameObject.Find("Xgaugearrow").GetComponent<RectTransform>(); break;
        }
        
        m_ParentNotCathFlag = false;


        m_InitLocalPos = tr.localPosition;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerManager = m_Player.GetComponent<PlayerManager>();
        m_StartPositionZ = tr.localPosition.z;
        m_TutorialSetting = m_Player.GetComponent<TutorialSetting>();

        SetPliersCollider(false);


    }

    void FixedUpdate()
    {
        if (!m_ArmManager.IsMove) return;

        //移動量
        m_MoveVelocity = tr.position - m_PrevPosition;
        m_PrevPosition = tr.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "CatchObject") && (m_CatchObject == null))
        {
            m_HitObject = other.gameObject.GetComponent<CatchObject>();
            CutRodCollision c = other.gameObject.GetComponent<CutRodCollision>();
            if (c != null)
                m_HitCutObject = c;
        }

        if (other.tag == "RodCollision")
        {

        }

        if (other.tag == "Kusari")
        {
            m_HitKusariObject = other.gameObject.GetComponent<Kusari>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "CatchObject") && (m_HitObject != null))
        {
            if (other.gameObject.GetComponent<CatchObject>() == m_HitObject)
                m_HitObject = null;

            ////掴んでいる状態だったなら、掴み状態解除
            //if (m_CatchObject != null)
            //    CatchObjectRelease();
        }
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        //プレイヤー軸回転用のオブジェクトをシーンビューに表示
        if (m_PlayerAxisMoveY == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(m_PlayerAxisMoveY.transform.position, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_PlayerAxisMoveX.transform.position, 0.5f);

        Gizmos.DrawWireCube(m_AxisMovePliersPosition.transform.position, Vector3.one * 0.5f);

        Gizmos.color = Color.blue;
        Vector3 direction = m_AxisMovePlayerPosition.transform.TransformDirection(Vector3.forward);
        Gizmos.DrawRay(m_AxisMovePlayerPosition.transform.position, direction);

#endif
    }

    //ペンチ先端部分の当たり判定を変更
    private void SetPliersCollider(bool value)
    {
        m_LeftColl.enabled = m_RightColl.enabled = value;
    }

    //回転（ペンチはＺ軸回転のみ）
    private void Rotation()
    {
        float roll = 0.0f;
        if (InputManager.GetArmNegativeTurn())
            roll += m_ArmManager.GetPliersRollSpeed() * Time.deltaTime;
        if (InputManager.GetArmPositiveTurn())
            roll -= m_ArmManager.GetPliersRollSpeed() * Time.deltaTime;

        tr.Rotate(Vector3.forward, roll);
        m_Roll = roll;
    }

    private void Input()
    {
        //ペンチの挟むパワーを入力から計算
        m_Input = InputManager.GetPliersCatch() > 0;
        m_Power += (m_Input ? 1 : -1);
        m_Power = Mathf.Clamp(m_Power, -1, 1);//* m_CatchPower;
    }


    private float a = 0.0f;

    private void EasyInput()
    {
        //入力取得
        if (m_ArmManager.GetEnablArmMove().GetIsCatching() && m_ArmManager.IsCatchAble)
        {
            a = 0;
            m_Input = m_ArmManager.GetEnablArmMove().GetIsStreched();
            //print(m_Input);
        }
        else
        {
            //a += Time.deltaTime;
            //print(a);
            //print(m_ArmManager.GetEnablArmMove().GetIsStreched());

            if (!m_ArmManager.GetEnablArmMove().GetIsStreched() && m_ArmManager.IsRelease)
            {
                m_LateEasyMoveInput = m_EasyMoveInput;
                m_EasyMoveInput = InputManager.GetCatchingEasyMode();
                //入力があったとき　かつ押した瞬間
                if (m_EasyMoveInput && !m_LateEasyMoveInput)
                {
                    m_Input = !m_Input;
                    m_ArmManager.GetEnablArmMove().ForceInput();
                }
            }
        }
        //ペンチの挟むパワーを入力から計算
        m_Power += (m_Input ? 1 : -1);
        m_Power = Mathf.Clamp(m_Power, -1, 1);

    }

    //オブジェクト切断処理
    private void Cut()
    {
        //切れるオブジェクトを掴んでいるなら
        if (m_CatchObject != null && m_HitCutObject != null && m_Input)
        {
            float power = m_ArmManager.GetPliersPower(m_ID);

            //挟む強さが強度より大きい場合、耐久値を減らしていく
            float life = m_HitCutObject.DamageAndGetLife(power);
            m_GaugeArrowTargetValue = life / m_HitCutObject.GetStartLife();

            if (life <= 0.0f)
            {
                //パーティクル生成
                GameObject par = Instantiate(m_ArmManager.CutParticlePrefab,m_CatchObject.transform.position,Quaternion.identity);
                GameObject.Destroy(par, 2.0f);



                CatchObjectRelease();

                ////壊れた場合はnullを入れる
                //m_ReleasedObject = m_CatchObject.gameObject;
                //m_CatchObject = null;
                //m_IsCatch = false;
                //エイムアシストによるキャッチをやめる
                m_ArmManager.GetArmMoveByID(m_ID).CatchingCancel();

                //GameObject.Destroy(m_PlayerAxisMoveY);
                //m_PlayerAxisMoveY = null;
                //m_PlayerManager.ReleaseAxisMoveObject();

                //子供にMainRod(Clone)が残っている場合親子関係解除
                Transform chi;
                do
                {
                    chi = tr.FindChild("MainRod(Clone)");
                    if (chi != null)
                        chi.parent = null;
                } while (chi != null);




                //CatchObjectRelease();
            }


            ////掴んでいるときのずり落ちる動き
            //float gravity = m_HitCutObject.GetStrength() - power;
            //gravity = Mathf.Clamp(gravity, 0.0f, 10.0f) * Time.deltaTime;
            ////移動
            //if (m_Player.GetComponent<PlayerManager>().MoveAxisMoveObject(new Vector3(0.0f, -gravity, 0.0f)) && m_CatchObject != null)
            //{
            //    //移動に成功したら、掴んだ地点も移動
            //    m_CatchObject.SetCatchPoint(m_CatchObject.GetCatchPoint().transform.position + new Vector3(0.0f, -gravity, 0.0f));
            //}
        }
        ////破壊するだけならこちら
        //if (m_ArmManager.GetPliersPower(m_ID) > 3.0f && m_HitCutObject != null && m_Input)
        //    m_HitCutObject.m_IsBreakFlag = true;
    }

    //鉄球の鎖切断処理
    private void KusariCut()
    {
        //鉄球の鎖を壊す
        if (m_ArmManager.GetPliersPower(m_ID) > 3.0f && m_HitKusariObject != null && m_Input)
        {
            m_HitKusariObject.m_IsCollision = true;
            SoundManager.Instance.PlaySe("break1");
        }
    }

    //動かないオブジェクトを掴んだ時の処理
    private void CatchedStatic()
    {
        //レイを飛ばす
        Vector3 dir = tr.forward;
        Ray ray = new Ray(tr.position - tr.forward, dir);
        int mask = LayerMask.NameToLayer("ArmAndPliers");
        RaycastHit hit;
        bool ishit = Physics.Raycast(ray, out hit, 3.0f, mask);

        Vector3 catchPosition;
        if (ishit)
        {
            //ちょっと前にずらす
            Vector3 f = tr.forward;
            f = new Vector3(f.x, 0.0f, f.z).normalized;
            //掴んだ座標決定
            catchPosition = hit.point + f * 0.3f;
        }            
        else
            catchPosition = tr.position + tr.forward * 0.3f;


        //プレイヤー軸回転用オブジェクトを生成
        m_PlayerAxisMoveY = new GameObject("RotY");
        m_PlayerAxisMoveX = new GameObject("RotX");
        m_AxisMovePlayerPosition = new GameObject("Pos");

        GameObject pliersrotx = new GameObject("PliersRotX");
        m_AxisMovePliersPosition = new GameObject("PliersPos");

        //m_AxisMovePlayerPosition.AddComponent<AxisMovePosition>();

        //プレイヤー軸回転用オブジェクトの親子関係を設定
        //m_PlayerAxisMoveY>m_PlayerAxisMoveX>m_AxisMovePlayerPosition　の順
        m_PlayerAxisMoveX.transform.parent = m_PlayerAxisMoveY.transform;
        m_AxisMovePlayerPosition.transform.parent = m_PlayerAxisMoveX.transform;
        pliersrotx.transform.parent = m_PlayerAxisMoveY.transform;
        m_AxisMovePliersPosition.transform.parent = pliersrotx.transform;
        //それぞれ座標と向きをセット
        //RotY
        m_PlayerAxisMoveY.transform.position = catchPosition;
        m_PlayerAxisMoveY.transform.LookAt(m_Player.transform.position);
        Vector3 a =  m_PlayerAxisMoveY.transform.eulerAngles;
        a = new Vector3(0.0f, a.y, 0.0f);
        m_PlayerAxisMoveY.transform.eulerAngles = a;
        //RotX
        m_PlayerAxisMoveX.transform.position = m_PlayerAxisMoveY.transform.position + m_RotXCreatePos;
        ///m_PlayerAxisMoveX.transform.forward = -tr.forward;
        m_PlayerAxisMoveX.transform.LookAt(m_PlayerAxisMoveX.transform.position - tr.forward);

        ////Pos
        //Transform armtr = m_ArmManager.GetEnablArm().transform;
        //Vector3 armforward = armtr.forward;
        //float armlength = m_ArmManager.GetEnablArmMove().GetArmStretch();
        
        ////アーム根元座標からプレイヤー座標に向かうベクトル
        //Vector3 arm2player = m_Player.transform.position - armtr.position;
        ////掴んだ座標からペンチの根元座標に向かうベクトル
        //Vector3 catch2pliers = tr.position - catchPosition;

        //Vector3 pos =
        //    catchPosition
        //    + catch2pliers
        //    + -armforward * armlength
        //    + arm2player;

        m_AxisMovePlayerPosition.transform.position = m_Player.transform.position;
        m_AxisMovePlayerPosition.transform.rotation = m_Player.transform.rotation;

        //ペンチ
        pliersrotx.transform.position = m_PlayerAxisMoveY.transform.position;
        pliersrotx.transform.LookAt(tr.position);
        m_AxisMovePliersPosition.transform.position = tr.position;
        m_AxisMovePlayerPosition.transform.rotation = tr.rotation;

        //マネージャーに軸回転用のオブジェクトを渡す
        m_Player.GetComponent<PlayerManager>().SetAxisMoveObject(m_PlayerAxisMoveY);


        //掴んだポイントをセット
        m_ArmManager.GetEnablArmMove().SetStaticCatchPoint(m_PlayerAxisMoveY.transform.position);
        //m_CatchObject.SetCatchPoint(m_PlayerAxisMoveY.transform.position);
        m_CatchObjPrevPos = m_CatchObject.transform.position;


        //他のアーム、ペンチをリセット
        m_ArmManager.ResetOther(m_ID);
    }

    //動かせるオブジェクトを掴んだ時の処理
    private void CatchedDynamic()
    {
        ////他のオブジェクトが掴んでいるなら掴めない
        if (m_ArmManager.GetCountCatchingDynamicObjects() >= 2)
        {
            Reset();
            return;
        }

        //掴んだオブジェクトを子にする
        //Rigidbodyが見つかるまで親をたどる
        Rigidbody r;
        Transform t = m_CatchObject.transform;

        do
        {
            r = t.GetComponent<Rigidbody>();
            //見つかったら終了
            if (r != null) break;

            //見つからなかったらその親を調べる
            t = t.parent;

        } while (true);

        //掴んだオブジェクトの大本の親が判明
        m_CatchParent = t;

        //その親を掴んでいるペンチがあるなら何もしない
        PliersMove pm = m_CatchParent.GetComponentInParent<PliersMove>();
        if (pm != null)//nullじゃないなら、何かしらのアームが掴んでいる
        {
            m_CatchParent = null;
        }
        else
        {
            //その大本の親の親に自身を設定して、持ち運ぶ
            m_CatchParent.parent = this.transform;
            m_CatchParent.GetComponent<Rigidbody>().isKinematic = true;
        }

        //他のアーム、ペンチをリセット
        m_ArmManager.ResetOther(m_ID);
    }

    //掴んでいるオブジェクトを手放す処理
    private void CatchObjectRelease()
    {
        switch (m_CatchObject.GetCatchType())
        {
            //動かないオブジェクト
            case CatchObject.CatchType.Static:
                //削除
                GameObject.Destroy(m_PlayerAxisMoveY);
                m_PlayerAxisMoveY = null;
                m_PlayerManager.ReleaseAxisMoveObject();

                //他に動かないオブジェクトを掴んでいるアームがある場合
                if (m_ArmManager.GetCountCatchingObjects() >= 2)
                {
                    //その掴んでいるアーム（ペンチ）から軸回転用のオブジェクトを取得し、適用
                    PliersMove pli = m_ArmManager.GetCatchingArm(m_ID);
                    m_PlayerManager.SetAxisMoveObject(pli.GetPlayerAxisMoveObject());
                }
                break;

            //動かせるオブジェクト
            case CatchObject.CatchType.Dynamic:
                bool flag = true;
                if (m_CatchParent != null)
                {
                    if (m_CatchParent.name == "MainRod" || m_CatchParent.name == "MainRod(Clone)")
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            GameObject catchObject = m_ArmManager.GetPliersCatchRod(i);
                            GameObject enableCatchObject = m_ArmManager.GetEnablePliersReleasedRod();
                            m_SeveObject = m_ArmManager.GetPliers(i);
                            if (catchObject == null) continue;
                            if (catchObject == enableCatchObject)
                            {
                                if (m_ArmManager.GetEnablPliers() == m_ArmManager.GetPliers(i))
                                {
                                    continue;
                                }
                                m_ParentNotCathFlag = true;
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            //親子関係を解除
                            m_CatchParent.parent = null;
                            //自由落下させる
                            m_CatchParent.GetComponent<Rigidbody>().isKinematic = false;
                        }
                        else
                            return;
                    }
                    else
                    {
                        //親子関係を解除
                        m_CatchParent.parent = null;
                        //自由落下させる
                        m_CatchParent.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
                break;
        }
        m_IsCatch = false;
        m_ReleasedObject = m_CatchObject.gameObject;
        m_CatchObject = null;

        m_ArmManager.GetArmMoveByID(m_ID).CatchingCancel();



        
    }

    //簡単モード時の処理
    private void EasyMode()
    {
        //当たり判定をなくす
        SetPliersCollider(false);

        //回転
        Rotation();

        EasyInput();

        ////オブジェクトを切断する
        //Cut();
        KusariCut();


        //前フレームと同じ入力ならここで終了　これ以降の行はボタンを入力した瞬間or離した瞬間のみ呼ばれる
        if (m_LateInput == m_Input) return;
        m_LateInput = m_Input;

        if (m_Input)
            SoundManager.Instance.PlaySe("xg-2sand02");
            
        ////掴める範囲内にオブジェクトがなければ終了
        //if (m_HitObject == null) return;
        ////掴める範囲内のオブジェクトが動いていないかつ、入力があるとき
        //if ((m_HitObject.rigidBody.velocity.magnitude < m_ArmManager.GetCatchingThreshold()) && m_Input)
        //

        if (m_HitObject != null && m_CatchObject == null && m_Input)
        {
            if (!m_ArmManager.IsCatchAble) return;
            if (m_ArmManager.GetEnablArmMove().GetIsAimAssisting() && !m_ArmManager.GetEnablArmMove().GetIsCatching()) return;

            //キャッチする
            m_CatchObject = m_HitObject;
            m_IsCatch = true;
            SoundManager.Instance.PlaySe("xg-2sand01");

            //掴む物のタイプで分岐
            switch (m_CatchObject.GetCatchType())
            {
                //動かないオブジェクト
                case CatchObject.CatchType.Static:
                    CatchedStatic();
                    break;

                //動かせるオブジェクト
                case CatchObject.CatchType.Dynamic:
                    CatchedDynamic();
                    break;
            }
        }
        //掴んでいる(オブジェクトがnullではない）かつ離す入力
        else if (m_CatchObject != null && !m_Input)
        {
            if (!m_ArmManager.IsRelease) return;

            m_HitObject = null;
            //オブジェクトを手放す
            CatchObjectRelease();
        }
    }

    //通常モード時の処理
    private void NormalMode()
    {
        //回転
        Rotation();

        //入力取得
        Input();

        //オブジェクトを切断する
        ///Cut();
        KusariCut();




        //前フレームと同じ入力ならここで終了　これ以降の行はボタンを入力した瞬間or離した瞬間のみ呼ばれる
        if (m_LateInput == m_Input) return;
        m_LateInput = m_Input;

        if (m_Input)
            SoundManager.Instance.PlaySe("xg-2sand02");

        ////掴める範囲内にオブジェクトがなければ終了
        //if (m_HitObject == null) return;

        ////掴める範囲内のオブジェクトが動いていないかつ、入力があるとき
        //if ((m_HitObject.rigidBody.velocity.magnitude < m_ArmManager.GetCatchingThreshold()) && m_Input)

        if (m_HitObject != null && m_CatchObject == null && m_Input)
        {
            //キャッチする
            m_CatchObject = m_HitObject;
            m_IsCatch = true;
            SoundManager.Instance.PlaySe("xg-2sand01");

            //掴む物のタイプで分岐
            switch (m_CatchObject.GetCatchType())
            {
                //動かないオブジェクト
                case CatchObject.CatchType.Static:
                    CatchedStatic();
                    break;

                //動かせるオブジェクト
                case CatchObject.CatchType.Dynamic:
                    CatchedDynamic();
                    break;
            }
        }

        //掴んでいる(オブジェクトがnullではない）状態でボタンを離したとき
        else if (m_CatchObject != null)
        {
            //オブジェクトを手放す
            CatchObjectRelease();
        }
    }

    /// <summary>
    /// 更新処理　（選択中のアーム（ペンチ）のみ実行、アームマネージャーから呼び出して動かす）
    /// </summary>
    public void PliersUpdate()
    {
        if (m_PlayerManager.GetIsEasyMode())
            EasyMode();
        else
            NormalMode();
    }



    void LateUpdate()
    {
        Cut();

        if (!m_ArmManager.IsMove)
        {
            m_LeftRB.velocity = Vector3.zero;
            m_RightRB.velocity = Vector3.zero;
            return;
        }



        //左
        m_LeftRB.velocity = m_PliersLeft.right * m_Power;
        Vector3 clampPosition = m_LeftStartPosition;
        clampPosition.x = Mathf.Clamp(m_PliersLeft.localPosition.x, m_LeftStartPosition.x, m_LeftEndPosition.x);
        m_PliersLeft.localPosition = clampPosition;

        //右
        m_RightRB.velocity = m_PliersRight.right * -m_Power;
        Vector3 clampPosition2 = m_RightStartPosition;
        clampPosition2.x = Mathf.Clamp(m_PliersRight.localPosition.x, m_RightEndPosition.x, m_RightStartPosition.x);
        m_PliersRight.localPosition = clampPosition2;

        //X以外の方向移動を修正
        m_PliersLeft.localPosition = new Vector3(m_PliersLeft.localPosition.x, m_LeftStartPosition.y, m_LeftStartPosition.z);
        m_PliersRight.localPosition = new Vector3(m_PliersRight.localPosition.x, m_RightStartPosition.y, m_RightStartPosition.z);
        m_PliersLeft.localEulerAngles = Vector3.zero;
        m_PliersRight.localEulerAngles = Vector3.zero;

        //オブジェクトを掴んでいるなら
        if (m_CatchObject != null)
        {
            //当たり判定をつける
            SetPliersCollider(true);

            //カットできるオブジェクト以外を掴んでいる場合はＵＩの矢印を移動させる
            if (m_HitCutObject == null)
                m_GaugeArrowTargetValue = 1.0f;


            //掴んでいるオブジェクトが移動した分、プレイヤー回転用座標も移動する
            if(m_PlayerAxisMoveY != null)
            {
                m_CatchObjVelocity = m_CatchObject.transform.position - m_CatchObjPrevPos;
                m_PlayerAxisMoveY.transform.position += m_CatchObjVelocity;
                m_CatchObjPrevPos = m_CatchObject.transform.position;
            }
            else
            {
                m_CatchObjVelocity = m_CatchObject.transform.position - m_CatchObjPrevPos;
                m_CatchObjPrevPos = m_CatchObject.transform.position;
            }

        }
        else
        {
            m_GaugeArrowTargetValue = 0.0f;
        }


        //ＵＩの矢印の移動
        m_GaugeArrowValue = Mathf.Lerp(m_GaugeArrowValue, m_GaugeArrowTargetValue, 0.2f);
        m_GaugeArrowValue = Mathf.Clamp(m_GaugeArrowValue, 0.0f, 1.0f);

        if (m_PliersGaugeArrow != null)
            m_PliersGaugeArrow.localPosition = new Vector3(Mathf.Lerp(-40.0f, 40.0f, m_GaugeArrowValue), 32, 0);


        //2つでつかんだ後に前に掴んだアームを選択すると離す
        if (m_ParentNotCathFlag && (m_SeveObject == m_ArmManager.GetEnablPliers()))
        {
            //親子関係を解除
            m_CatchParent.parent = null;
            //自由落下させる
            m_CatchParent.GetComponent<Rigidbody>().isKinematic = false;

            m_IsCatch = false;
            m_ReleasedObject = m_CatchObject.gameObject;
            m_CatchObject = null;
            //フラグ初期化
            m_ParentNotCathFlag = false;
        }




    }



    /// <summary>
    /// IDをセットする
    /// </summary>
    public void SetID(int id)
    {
        m_ID = id;
    }

    /// <summary>
    /// オブジェクトを掴んでいるかを返す
    /// </summary>
    public bool GetIsCatch()
    {
        return m_IsCatch;
    }

    /// <summary>
    /// 掴んでいるオブジェクトを返す
    /// </summary>
    public CatchObject GetCatchObject()
    {
        return m_CatchObject;
    }

    /// <summary>
    /// プレイヤー軸回転用オブジェクト(親)を返す
    /// オブジェクト名は下記の通り
    /// "RotY"（親）＞"RotX"＞"Pos"　の順
    /// </summary>
    public GameObject GetPlayerAxisMoveObject()
    {
        return m_PlayerAxisMoveY;
    }

    /// <summary>
    /// Z軸回転量を返す(ペンチの回転入力がない場合には0、入力がある場合には回転に使用した回転量を返す)
    /// </summary>
    public float GetRollValue()
    {
        return m_Roll;
    }

    /// <summary>
    /// 回転量をリセットする
    /// </summary>
    public void RollReset()
    {
        tr.localEulerAngles = Vector3.zero;
    }
    /// <summary>
    /// 回転量をリセットするコルーチン
    /// </summary>
    private IEnumerator RollResetMove()
    {
        float timer = 0.0f;
        float maxtime = m_ArmManager.GetResetEndTime();

        Quaternion start = tr.localRotation;
        Quaternion target = Quaternion.Euler(Vector3.zero);

        while (timer <= maxtime)
        {
            timer += Time.deltaTime;

            tr.localRotation = Quaternion.Slerp(start, target, timer / maxtime);
            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// リセットする(掴み状態解除、向きを戻す)
    /// </summary>
    public void Reset()
    {
        if(m_CatchObject != null)
        {
            CatchObjectRelease();
        }
        StartCoroutine(RollResetMove());
        m_Input = false;
        m_LateEasyMoveInput = false;
        m_EasyMoveInput = false;
        m_GaugeArrowTargetValue = 0.0f;

  
        if (!m_TutorialSetting.GetIsActiveArm(m_ID))
            m_Power = 0.0f;
        else
            m_Power = -1.0f;
    }


    /// <summary>
    /// 座標の変化量を返す
    /// </summary>
    public Vector3 GetMoveVelocity()
    {
        return m_MoveVelocity;
    }


    public GameObject GetReleasedObject()
    {
        return m_ReleasedObject;
    }

    /// <summary>
    /// アームが実際に伸びた量を取得
    /// </summary>
    public float GetArmLength()
    {
        return Vector3.Magnitude(
            m_ArmManager.GetArmByID(m_ID).transform.position + (tr.forward * m_StartPositionZ)
            - tr.position);
    }

    public bool GetIsCatchingHitCutObject()
    {
        return m_HitCutObject != null;
    }

    public CutRodCollision GetHitCutObject()
    {
        return m_HitCutObject;
    }

    /// <summary>
    /// 強制的にオブジェクトを手放す
    /// </summary>
    public void ForceCatchRelease()
    {
        if (m_CatchObject != null)
        {
            CatchObjectRelease();
        }
    }

    /// <summary>
    /// HungRod処理時専用、強制的にオブジェクトを手放す
    /// </summary>
    public void ForceCatchReleaseHungRod()
    {
        if (m_CatchObject != null)
        {
            switch (m_CatchObject.GetCatchType())
            {
                //動かないオブジェクト
                case CatchObject.CatchType.Static:
                    //削除
                    GameObject.Destroy(m_PlayerAxisMoveY);
                    m_PlayerAxisMoveY = null;
                    m_PlayerManager.ReleaseAxisMoveObject();

                    //他に動かないオブジェクトを掴んでいるアームがある場合
                    if (m_ArmManager.GetCountCatchingObjects() >= 2)
                    {
                        //その掴んでいるアーム（ペンチ）から軸回転用のオブジェクトを取得し、適用
                        PliersMove pli = m_ArmManager.GetCatchingArm(m_ID);
                        m_PlayerManager.SetAxisMoveObject(pli.GetPlayerAxisMoveObject());
                    }
                    break;

                //動かせるオブジェクト
                case CatchObject.CatchType.Dynamic:
                    bool flag = true;
                    if (m_CatchParent != null)
                    {
                        if (m_CatchParent.name == "MainRod" || m_CatchParent.name == "MainRod(Clone)")
                        {
                            for (int i = 0; i <= 3; i++)
                            {
                                GameObject catchObject = m_ArmManager.GetPliersCatchRod(i);
                                GameObject enableCatchObject = m_ArmManager.GetEnablePliersReleasedRod();
                                m_SeveObject = m_ArmManager.GetPliers(i);
                                if (catchObject == null) continue;
                                if (catchObject == enableCatchObject)
                                {
                                    if (m_ArmManager.GetEnablPliers() == m_ArmManager.GetPliers(i))
                                    {
                                        continue;
                                    }
                                    m_ParentNotCathFlag = true;
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                //親子関係を解除
                                m_CatchParent.parent = null;
                                //自由落下させる
                                m_CatchParent.GetComponent<Rigidbody>().isKinematic = false;
                            }
                            else
                                return;
                        }
                        else
                        {
                            //親子関係を解除
                            m_CatchParent.parent = null;
                            //自由落下させる
                            m_CatchParent.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                    break;
            }
            m_IsCatch = false;
            m_ReleasedObject = m_CatchObject.gameObject;
            m_CatchObject = null;
        }
    }

    /// <summary>
    /// 強制的にオブジェクトを掴む
    /// </summary>
    public void ForceCatching(CatchObject catchobj)
    {
        m_Input = true;
        //キャッチする
        m_HitObject = catchobj;
        m_CatchObject = m_HitObject;
        m_IsCatch = true;
        //SoundManager.Instance.PlaySe("xg-2sand01");

        //掴む物のタイプで分岐
        switch (m_CatchObject.GetCatchType())
        {
            //動かないオブジェクト
            case CatchObject.CatchType.Static:
                CatchedStatic();
                break;

            //動かせるオブジェクト
            case CatchObject.CatchType.Dynamic:
                CatchedDynamic();
                break;
        }
    }

    public Vector3 GetCatchObjVelocity()
    {
        return m_CatchObjVelocity;
    }

    /// <summary>
    /// シーンが変わった後で呼ぶ処理
    /// </summary>
    public void SceneChange()
    {
        //ペンチゲージＵＩを取得しなおす
        switch (m_ID)
        {
            case 0: m_PliersGaugeArrow = GameObject.Find("Ygaugearrow").GetComponent<RectTransform>(); break;
            case 1: m_PliersGaugeArrow = GameObject.Find("Agaugearrow").GetComponent<RectTransform>(); break;
            case 2: m_PliersGaugeArrow = GameObject.Find("Bgaugearrow").GetComponent<RectTransform>(); break;
            case 3: m_PliersGaugeArrow = GameObject.Find("Xgaugearrow").GetComponent<RectTransform>(); break;
        }
        
    }
}
