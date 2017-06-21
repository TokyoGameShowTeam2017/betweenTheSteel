using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObject : MonoBehaviour
{

    public enum CatchType
    {
        Static,
        Dynamic
    }

    [SerializeField]
    private CatchType _catchType;

    [SerializeField]
    private GameObject _catchPoint;

    public Rigidbody rigidBody { get; private set; }

    public CatchType catchType { get; private set; }




    // Use this for initialization
    void Awake()
    {
        
        Initialize();
        SetType(_catchType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Catch(bool enable)
    {
        if (catchType == CatchType.Dynamic)
        {
            //Rigidbodyが見つかるまで親をたどる
            Rigidbody r;
            Transform t = this.transform.parent;

            do
            {
                r = t.GetComponent<Rigidbody>();
                //見つかったら終了
                if (r != null) break;

                //見つからなかったらその親を調べる
                t = t.parent;

            } while (true);

            r.isKinematic = enable;
        }
    }

    public CatchType GetCatchType()
    {
        return catchType;
    }

    public void SetCatchPoint(Vector3 point)
    {
        _catchPoint.transform.position = point;
    }

    public GameObject GetCatchPoint()
    {
        return _catchPoint;
    }
    public void SetType(CatchType type)
    {
        catchType = type;
        _catchType = type;
        if (catchType == CatchType.Dynamic)
            rigidBody.isKinematic = false;
        else
            rigidBody.isKinematic = true;
    }
    public void SetNoRigidBody(CatchType type)
    {
        catchType = type;
        _catchType = type;
    }
    public void Initialize()
    {
        //一番近い親のリジットボディを取得
        rigidBody = GetComponent<Rigidbody>();
        GameObject obj = gameObject;
        while (true)
        {
            if (rigidBody != null)
            {
                break;
            }
            obj = obj.transform.parent.gameObject;
            rigidBody = obj.GetComponent<Rigidbody>();
        }

        //rigidBody.isKinematic = catchType == CatchType.Static;
        rigidBody.isKinematic = (catchType == CatchType.Static);
    }

}
