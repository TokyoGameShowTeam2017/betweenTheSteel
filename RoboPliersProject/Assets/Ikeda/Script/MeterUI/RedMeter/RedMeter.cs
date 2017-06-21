using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMeter : MonoBehaviour {

    [SerializeField, Tooltip("大きさの設定")]
    private float scale = 1.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //一番上に来たら大きくする
        RedMeterBig();
	}

    /// <summary>
    /// 一番上に来たら大きくする
    /// </summary>
    private void RedMeterBig()
    {
        if (transform.parent.GetComponent<RotationUI>().GetArmId() == 0)
        {
            transform.localScale = new Vector3(scale, scale, 0.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        }
    }
}
