using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchObjectManager : MonoBehaviour {

    private List<CatchObject> _sceneCatchObjects;

	// Use this for initialization
	void Start () {
        _sceneCatchObjects = new List<CatchObject>();

        GameObject[] gs = GameObject.FindGameObjectsWithTag("CatchObject");

        for(int i = 0; i < gs.Length; i++)
        {
            _sceneCatchObjects.Add(gs[i].GetComponent<CatchObject>());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //引数のワールド座標から一番近いCatchObjectを取得
    public CatchObject GetNearCatchObject(Vector3 position)
    {
        CatchObject find = _sceneCatchObjects[0];
        foreach(var i in _sceneCatchObjects)
        {
            if (Vector3.Distance(find.transform.position, position) > Vector3.Distance(i.transform.position, position))
            {
                find = i;
            }
        }
        return find;
    }
}
