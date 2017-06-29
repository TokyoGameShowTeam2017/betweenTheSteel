using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUi : MonoBehaviour
{
    private Image mImage;
    private float mAlpha;
    public bool mIsDraw;
    // Use this for initialization
    void Start()
    {
        mImage = GetComponent<Image>();
        mAlpha = 0.0f;
        mIsDraw = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsDraw)
            mAlpha += Time.deltaTime;
        else
            mAlpha -= Time.deltaTime;

        mAlpha = Mathf.Clamp(mAlpha, 0.0f, 1.0f);
        mImage.color = new Color(1.0f, 1.0f, 1.0f, mAlpha);
    }


    public void IsDraw(bool flag)
    {
        mIsDraw = flag;
        if (flag)
        {
            //サウンドのリソースが実装されたら
            //SoundManager.Instance.PlaySe("ResultVoice");
            //SoundManager.Instance.PlaySe("Fanfare");
        }
    }
}
