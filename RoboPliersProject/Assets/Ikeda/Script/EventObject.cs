using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour {

    private bool isStart = false;

    [SerializeField]
    private GameObject [] MoveObjects;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        OtherCondition();

        if (isStart == true)
        {
            foreach(GameObject Obj in MoveObjects)
            {
                if (Obj == null) return;
                Obj.GetComponent<MoveObject>().isMotion = true;                
            }
        }
    }

    //他の条件がある場合
    private void OtherCondition()
    {

    }

    //Playerが触れたら動き始める
    public void OnTriggerEnter(Collider other)
    {
        isStart = true;
    }
}
