using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellorMeter : MonoBehaviour {

    [SerializeField, Tooltip("大きさを設定する")]
    private float scale = 1.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        YellorMeterBig();
	}


    /// <summary>
    /// 黄色メーターを大きくする
    /// </summary>
    private void YellorMeterBig()
    {
        if (transform.parent.GetComponent<RotationUI>().GetArmId() == 3)
        {
            transform.localScale = new Vector3(scale, scale, 0.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        }
    }
}
