using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldManager : MonoBehaviour
{
    private List<GameObject> mCollisions;
    // Use this for initialization
    void Start()
    {
        mCollisions = new List<GameObject>();
        //子のくさりを全部リストへ
        Transform[] trans;
        trans = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in trans)
        {
            //自分自身は入れない
            if (t.name != name &&
                t.name.Substring(0, 4) != "AimA" &&
                t.name.Substring(0, 4) != "Catc" &&
                t.name.Substring(0, 4) != "Snap")
                mCollisions.Add(t.gameObject);
        }
        SetNoRigitBodyType(CatchObject.CatchType.Static);
    }
    public void SetNoRigitBodyType(CatchObject.CatchType type)
    {
        foreach (var i in mCollisions)
        {
            i.GetComponent<CatchObject>().SetNoRigidBody(type);
        }
    }
    public void SetType(CatchObject.CatchType type)
    {
        foreach (var i in mCollisions)
        {
            i.GetComponent<CatchObject>().SetType(type);
        }
    }
}
