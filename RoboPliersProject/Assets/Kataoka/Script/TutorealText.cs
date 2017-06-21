using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class TutorealText : MonoBehaviour
{

    //自身の情報
    private RectTransform mTrans;
    //テキスト
    private Text mText;
    //テキストスピード
    private float mTextScroolSpeed = 30.0f;

    // Use this for initialization
    void Start()
    {
        mTrans = GetComponent<RectTransform>();
        mText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mTrans.anchoredPosition -= new Vector2(mTextScroolSpeed, 0) * Time.deltaTime;
    }
    public void SetText(string text)
    {
        //文字のバイト数を見て横幅を決める
        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        int num = sjisEnc.GetByteCount(text);
        //サイズ設定
        mTrans.sizeDelta = new Vector2(num * 27.27f, 100.0f);

        mTrans.anchoredPosition = new Vector2(50.0f + (num * 27.27f) / 8.0f / 2.0f, 0.0f);

        mText.text = text;
    }
    public void SetTextScroolSpeed(float speed)
    {
        mTextScroolSpeed = speed;
    }
}
