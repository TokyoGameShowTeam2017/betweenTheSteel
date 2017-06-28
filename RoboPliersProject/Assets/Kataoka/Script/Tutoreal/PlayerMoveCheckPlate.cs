using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCheckPlate : MonoBehaviour
{
    [SerializeField, Tooltip("点滅スピード")]
    public float m_TimeSpeed = 1.0f;

    private bool mDeadFlag;
    private float mCount;
    private Vector3 mStartScale;
    // Use this for initialization
    void Start()
    {
        mDeadFlag = false;
        mCount = 0.0f;
        mStartScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = GetComponent<Renderer>().material.color;
        mCount += m_TimeSpeed * Time.deltaTime;
        if (mDeadFlag)
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, mCount));
            transform.localScale =
                new Vector3(
                    Mathf.Lerp(mStartScale.x, mStartScale.x + 0.1f, mCount),
                    Mathf.Lerp(mStartScale.y, mStartScale.y + 0.1f, mCount),
                    Mathf.Lerp(mStartScale.z, mStartScale.z + 0.1f, mCount));
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
