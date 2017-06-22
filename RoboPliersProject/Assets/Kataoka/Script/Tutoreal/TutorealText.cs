using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class TutorealText : MonoBehaviour
{
    //自身の情報
    private RectTransform mTrans;
    //テキスト
    private Text mText;
    //ボタン押されたか
    private bool mIsButton;
    //表示するかどうか
    private bool mDrawTextFlag;
    //表示されているテキスト
    private string mDrawText;
    //ピピピピピと表示させる文字タイム
    private float mDrawTextTime;
    //ピピピする時間
    private float mPlayTextTime;
    //表示カウント
    private int mDrawTextCount;
    //流すテキスト
    private string[] m_Text;
    //テキストα値
    private float mTextAlpha;
    //テキストクリーンカウント
    private int mTextCreenCount;
    //補間系
    float mY;
    float mResY;
    float mVeloY;
    // Use this for initialization
    void Start()
    {
        mTrans = GetComponent<RectTransform>();
        mText = transform.FindChild("PlayerText").GetComponent<Text>();
        mIsButton = false;
        mDrawTextFlag = false;

        mY = -370.0f;
        mResY = mY;
        mVeloY = 0.0f;

        mDrawTextCount = 0;
        mDrawTextTime = 0.0f;
        mTextAlpha = 0.0f;

        mPlayTextTime = 0.0f;

        mTextCreenCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mDrawTextFlag)
        {
            mTextAlpha = 1.0f;
            //テキストの改行数を取得
            int returnCount=m_Text[mTextCreenCount].Count(c=>c=='\n')+1;
            //行数によって変える
            mResY = -300.0f + (50.0f*(returnCount-1));
            //mResY = 
            mDrawTextTime += Time.deltaTime;
            mPlayTextTime += Time.deltaTime;
            //ピピピピ機能
            if (mDrawTextTime >= 0.06f && mDrawTextCount < m_Text[mTextCreenCount].Length &&
                mPlayTextTime>=1.0f)
            {
                mDrawTextCount++;
                mDrawTextTime = 0.0f;
            }
            //全て終わったら表示終わる機能
            if (Input.GetKeyDown(KeyCode.V) &&
                mDrawTextCount>=m_Text[mTextCreenCount].Length)
            {
                mDrawTextCount = 0;
                mDrawTextTime = 0;
                mPlayTextTime = 0.0f;
                if (mTextCreenCount >= m_Text.Length-1)
                    mDrawTextFlag = false;
                else
                    mTextCreenCount++;
            }

            //スキップ機能
            if (Input.GetKeyDown(KeyCode.V)&&mDrawTextCount>0)
            {
                mDrawTextCount = m_Text[mTextCreenCount].Length;
            }
            mDrawText = m_Text[mTextCreenCount].Substring(0, mDrawTextCount);
            mText.text = mDrawText;
        }
        else
        {
            mTextAlpha -= 2.0f * Time.deltaTime;
            mTextCreenCount = 0;
            if (mTextAlpha <= 0.0f)
            {
                mDrawTextCount = 0;
            }
            mResY = -370.0f;
        }

        ////テスト
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    SetText(m_Text);
        //}
        //α値クランプ
        mTextAlpha = Mathf.Clamp(mTextAlpha, 0.0f, 1.0f);
        //補間
        SpringFloat(0.2f, 0.5f, 2.0f);
        mTrans.anchoredPosition = new Vector2(mTrans.anchoredPosition.x, mY);
    }
    public void SetText(string[] text)
    {
        m_Text = text;
        mTextCreenCount = 0;
        mDrawTextFlag = true;
    }
    private void SpringFloat(float stiffness, float friction, float mass)
    {
        // バネの伸び具合を計算
        float stretch = (mY - mResY);
        // バネの力を計算
        float force = -stiffness * stretch;
        // 加速度を追加
        float acceleration = force / mass;
        // 移動速度を計算
        mVeloY = friction * (mVeloY + acceleration);
        // 座標の更新
        mY += mVeloY;
    }
    public bool GetDrawTextFlag()
    {
        return mDrawTextFlag;
    }
    public int GetTextSize()
    {
        return m_Text.Length;
    }
    public int GetCreenCount()
    {
        return mTextCreenCount;
    }
}
