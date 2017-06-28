using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCheckPlate : MonoBehaviour {
    private bool mDeadFlag;
    private float mCount;
	// Use this for initialization
	void Start () {
        mDeadFlag = false;
        mCount = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (mDeadFlag)
        {
            Color color = GetComponent<Renderer>().material.color;
            mCount += Time.deltaTime;
            GetComponent<Renderer>().material.color = new Color(255.0f,0.0f,0.0f, Mathf.Lerp(1.0f, 0.0f, mCount));
            transform.localScale =
                new Vector3(
                    Mathf.Lerp(0.2f, 0.3f, mCount),
                    Mathf.Lerp(0.2f, 0.3f, mCount),
                    Mathf.Lerp(0.2f, 0.3f, mCount));
        }

	}

    public void SetColor(float time)
    {
        GetComponent<Renderer>().material.color =
            new Color(1.0f, Mathf.Lerp(1.0f, 0.0f, time), Mathf.Lerp(1.0f, 0.0f, time), Mathf.Lerp(1.0f, 0.0f, mCount));
    }
    public void IsDead()
    {
        mDeadFlag = true;
    }
}
