using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    //カメラ
    private GameObject mCamera;
    //クオタニオン
    private Quaternion mQu;
    //ミニマップ用の空のプレイヤー
    private GameObject mMiniMapPlayer;
    [SerializeField, Tooltip("ミニマップUIオブジェクト")]
    public GameObject m_MiniMapUI;
    [SerializeField, Tooltip("ミニマップ表示させるか"), Space(15)]
    public bool m_DrawMiniMap = true;
    //現在表示しているMAP
    private int mDrawMapIndex;
    // Use this for initialization
    void Awake()
    {
        mQu = transform.rotation;
        mCamera = GameObject.FindGameObjectWithTag("RawCamera");

        if (!m_DrawMiniMap)
            m_MiniMapUI.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //UIフェード処理
        if (m_DrawMiniMap)
            m_MiniMapUI.GetComponent<CanvasGroup>().alpha += 1.0f * Time.deltaTime;
        else
            m_MiniMapUI.GetComponent<CanvasGroup>().alpha -= 1.0f * Time.deltaTime;

        //クランプ
        m_MiniMapUI.GetComponent<CanvasGroup>().alpha =
            Mathf.Clamp(m_MiniMapUI.GetComponent<CanvasGroup>().alpha, 0.0f, 1.0f);

        //マップの回転
        transform.rotation = mQu
            * Quaternion.AngleAxis(-mCamera.transform.eulerAngles.x, new Vector3(1, 0, 0))
            * Quaternion.AngleAxis(-mCamera.transform.eulerAngles.y, new Vector3(0, 1, 0));
    }

}
