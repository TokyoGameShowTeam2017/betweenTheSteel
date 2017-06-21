using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMeter : MonoBehaviour {
    [SerializeField, Tooltip("大きさの設定")]
    private float scale = 1.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GreenMeterBig();
	}

    /// <summary>
    /// 緑メーターを少し大きくする
    /// </summary>
    private void GreenMeterBig()
    {
        if (transform.parent.GetComponent<RotationUI>().GetArmId() == 2)
        {
            transform.localScale = new Vector3(scale, scale, 0.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        }
    }
}
