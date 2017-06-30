using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialEventTextSet : MonoBehaviour
{
    private bool mFlag;
    private Text mText;
    private float mAlpha;
    private float mTime;
    // Use this for initialization
    void Start()
    {
        mText = GetComponent<Text>();
        mFlag = false;
        mTime = 0.0f;
        mAlpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        mTime += Time.deltaTime;
        if (mFlag)
        {
            mAlpha = Mathf.Sin((mTime * 250.0f) * Mathf.Deg2Rad)+0.3f;
        }
        else
        {
            mAlpha = 0.0f;
        }
        mText.color = new Color(1, 1, 1, mAlpha);
    }

    public void SetEventText(string text)
    {
        mText.text = text;
    }
    public void SetDrawFlag(bool flag)
    {
        mFlag = flag;
    }
}
