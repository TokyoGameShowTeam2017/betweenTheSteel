using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUi : MonoBehaviour
{
    private Image mImage;
    private float mAlpha;
    private float mDrawTime;
    private bool mIsDraw;
    [SerializeField, Tooltip("表示時間")]
    public float m_DrawTime=5.0f;
    // Use this for initialization
    void Start()
    {
        mImage = GetComponent<Image>();
        mAlpha = 0.0f;
        mDrawTime = 0.0f;
        mIsDraw = true;
        //サウンドのリソースが実装されたら
        //SoundManager.Instance.PlaySe("ResultVoice");
        //SoundManager.Instance.PlaySe("Fanfare");
    }

    // Update is called once per frame
    void Update()
    {
        mDrawTime+=Time.deltaTime;
        if (m_DrawTime <= mDrawTime)
            mIsDraw = false;

        if (mIsDraw)
            mAlpha += Time.deltaTime;
        else
            mAlpha -= Time.deltaTime;
        mAlpha = Mathf.Clamp(mAlpha, 0.0f, 1.0f);
        mImage.color = new Color(1.0f, 1.0f, 1.0f, mAlpha);
    }
}
