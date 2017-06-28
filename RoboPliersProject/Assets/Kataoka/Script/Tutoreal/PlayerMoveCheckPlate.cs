using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCheckPlate : MonoBehaviour
{
    private bool mDeadFlag;
    private float mCount;
    // Use this for initialization
    void Start()
    {
        mDeadFlag = false;
        mCount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = GetComponent<Renderer>().material.color;
        mCount += Time.deltaTime;
        if (mDeadFlag)
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, mCount));
            transform.localScale =
                new Vector3(
                    Mathf.Lerp(0.2f, 0.3f, mCount),
                    Mathf.Lerp(0.2f, 0.3f, mCount),
                    Mathf.Lerp(0.2f, 0.3f, mCount));
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0.0f, 1.0f, Mathf.Sin(Mathf.Sin(mCount * 360.0f * Mathf.Deg2Rad))));
            if (mCount >= 1.0f)
                mCount = 0.0f;
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
        mCount = 0.0f;
    }
}
