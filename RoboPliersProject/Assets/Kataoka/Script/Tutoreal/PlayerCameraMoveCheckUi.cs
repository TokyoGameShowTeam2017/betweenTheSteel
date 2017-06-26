using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCameraMoveCheckUi : MonoBehaviour {

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
        if (mDeadFlag)
        {
            mCount += Time.deltaTime;

            GetComponent<Image>().color = new Color(255.0f, 0.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, mCount));
            GetComponent<RectTransform>().localScale =
                new Vector3(
                    Mathf.Lerp(0.3f, 0.4f, mCount),
                    Mathf.Lerp(0.3f, 0.4f, mCount),
                    Mathf.Lerp(0.3f, 0.4f, mCount));
        }

    }

    public void SetColor(float time)
    {
        GetComponent<Image>().color =
            new Color(1.0f, Mathf.Lerp(1.0f, 0.0f, time), Mathf.Lerp(1.0f, 0.0f, time), Mathf.Lerp(1.0f, 0.0f, mCount));
    }
    public void IsDead()
    {
        mDeadFlag = true;
    }
}
