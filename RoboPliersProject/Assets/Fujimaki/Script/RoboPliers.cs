using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboPliers : MonoBehaviour {

    [SerializeField]
    private GameObject _pliers_right;
    [SerializeField]
    private GameObject _pliers_left;

    private Rigidbody _pliers_right_rigidbody;
    private Rigidbody _pliers_left_rigidbody;

    private Vector3 _pliers_right_startPosition;
    public Vector3 _pliers_left_startPosition;

    private Vector3 _pliers_right_endPosition;
    public Vector3 _pliers_left_endPosition;

    private float _pliers_right_velocityX;
    private float _pliers_left_velocityX;

    private float pliersPower;
    private float catchPower = 0.5f;
    private bool lateInput;

    public RoboArm roboArm { get; set; }
    public CatchObject targettingCatchObject { get; private set; }
    private CatchObject overlappingObject;

    private GameObject staticObjectCatchPosition;

    // Use this for initialization
    void Start () {
        _pliers_left_rigidbody = _pliers_left.GetComponent<Rigidbody>();
        _pliers_right_rigidbody = _pliers_right.GetComponent<Rigidbody>();

        _pliers_left_startPosition = _pliers_left.transform.localPosition;
        _pliers_right_startPosition = _pliers_right.transform.localPosition;

        _pliers_left_endPosition = _pliers_left.transform.localPosition + Vector3.right * 0.3f;
        _pliers_right_endPosition = _pliers_right.transform.localPosition - Vector3.right * 0.3f;
    }
	
	// Update is called once per frame
	void Update () {


    }

    public void PliersUpdate(bool input)
    {

        //ペンチの挟むパワーを入力から計算
        pliersPower += Time.deltaTime * 30 * (input ? 1 : -1);

        //入力をクランプ
        pliersPower = Mathf.Clamp(pliersPower, -1, 1);

        //入力が更新されていなければやめる
        if (lateInput == input)
        {
            return;
        }

        lateInput = input;


        //オーバーラップしたオブジェクトがあれば実行
        if (overlappingObject != null)
        {
            print((float)overlappingObject.rigidBody.velocity.magnitude);

            //入力があり、つかんだものが動いていなければキャッチする
            if ((overlappingObject.rigidBody.velocity.magnitude < 0.5f) && (input))
            {
                targettingCatchObject = overlappingObject;

                //つかむ物のタイプで分岐
                switch (targettingCatchObject.GetCatchType())
                {
                        //動かせないやつ
                    case CatchObject.CatchType.Static:
                        //プレイヤー軸回転用のオブジェクトを生成
                        print("static catch");
                        staticObjectCatchPosition = new GameObject("catch position");
                        staticObjectCatchPosition.transform.position = transform.position + (transform.forward * 0.5f);
                        //プレイヤーの親変更
                        GameObject p = GameObject.FindGameObjectWithTag("Player");
                        p.transform.parent = staticObjectCatchPosition.transform;
                        //PlayerManagerに軸回転用のオブジェクトを渡す
                        p.GetComponent<PlayerManager>().SetAxisMoveObject(staticObjectCatchPosition);
                        break;

                        //動かせるやつ
                    case CatchObject.CatchType.Dynamic:
                        targettingCatchObject.transform.parent = this.transform;
                        targettingCatchObject.Catch(true);
                        break;
                }
            }
            //つかんだものがあり、入力があれば離す
            else if (targettingCatchObject != null) 
            {

                switch (targettingCatchObject.GetCatchType())
                {
                    case CatchObject.CatchType.Static:
                        //プレイヤーとの親子関係を解消
                        GameObject p = staticObjectCatchPosition.transform.FindChild("Player").gameObject;
                        p.transform.parent = null;
                        //削除
                        GameObject.Destroy(staticObjectCatchPosition);
                        p.GetComponent<PlayerManager>().ReleaseAxisMoveObject();
                        break;

                    case CatchObject.CatchType.Dynamic:
                        targettingCatchObject.transform.parent = null;
                        targettingCatchObject.Catch(false);
                        break;
                }
                targettingCatchObject = null;
            }
        }
    }

    void FixedUpdate()
    {

        _pliers_left_velocityX -= _pliers_left.transform.localPosition.x;
        _pliers_right_velocityX -= _pliers_right.transform.localPosition.x;

        //catchDifference = Mathf.Abs((int)((_pliers_left_velocityX - _pliers_right_velocityX) * 1000));
        //print(catchDifference);

        _pliers_left_velocityX = _pliers_left.transform.localPosition.x;
        _pliers_right_velocityX = _pliers_right.transform.localPosition.x;

        //ひだり

        _pliers_left_rigidbody.velocity = _pliers_left.transform.right * pliersPower * catchPower;

        Vector3 clampPosition = _pliers_left_startPosition;

        clampPosition.x = Mathf.Clamp(_pliers_left.transform.localPosition.x, _pliers_left_startPosition.x, _pliers_left_endPosition.x);
        _pliers_left.transform.localPosition = clampPosition;


        //右

        _pliers_right_rigidbody.velocity = _pliers_right.transform.right * -pliersPower * catchPower;

        Vector3 clampPosition2 = _pliers_right_startPosition;

        clampPosition2.x = Mathf.Clamp(_pliers_right.transform.localPosition.x, _pliers_right_endPosition.x, _pliers_right_startPosition.x);
        _pliers_right.transform.localPosition = clampPosition2;



        //X以外の方向移動を修正
        _pliers_left.transform.localPosition = new Vector3(_pliers_left.transform.localPosition.x, _pliers_left_startPosition.y, _pliers_left_startPosition.z);
        _pliers_right.transform.localPosition = new Vector3(_pliers_right.transform.localPosition.x, _pliers_right_startPosition.y, _pliers_right_startPosition.z);

        _pliers_left.transform.localEulerAngles = Vector3.zero;
        _pliers_right.transform.localEulerAngles = Vector3.zero;

    }

    public CatchObject GetCatchObject()
    {
        return targettingCatchObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "CatchObject") && (targettingCatchObject == null)) 
        {
            overlappingObject = other.gameObject.GetComponent<CatchObject>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "CatchObject") && (overlappingObject != null)) 
        {
            if (other.gameObject.GetComponent<CatchObject>() == overlappingObject)
            {
                overlappingObject = null;
            }
        }
    }
}
