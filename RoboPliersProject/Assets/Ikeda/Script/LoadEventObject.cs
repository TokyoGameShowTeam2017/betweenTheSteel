using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEventObject : MonoBehaviour
{
    [SerializeField, Tooltip("閉まる扉を設定")]
    private GameObject closeDoor1;
    [SerializeField, Tooltip("閉まる扉を設定")]
    private GameObject closeDoor2;

    [SerializeField, Tooltip("開く扉を設定")]
    private GameObject openDoor1;
    [SerializeField, Tooltip("開く扉を設定")]
    private GameObject openDoor2;

    [SerializeField, Tooltip("UIのCanvasを入れる")]
    private GameObject uI;

    private bool m_IsLoadStart = false;
    private bool m_IsOnec = false;

    private enum LoadState
    {
        CloseDoorState,
        ResultState,
        LaserState,
        LineChangeState,
        MiniMapDrawState,
        OpenDoorState
    }

    private LoadState m_LoadState;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsLoadStart)
        {
            switch (m_LoadState)
            {         
                //シャッターが閉まったらUIとミニマップを非表示にする
                case LoadState.CloseDoorState:
                    CloseDoor();
                    if (closeDoor1.GetComponent<MoveObject>().IsMoveEnd() && 
                        closeDoor2.GetComponent<MoveObject>().IsMoveEnd())
                    {
                        NonActiveMiniMap();
                        NonActiveUI();
                        m_LoadState = LoadState.ResultState;
                    }
                    break;

                //リザルト画面へ
                case LoadState.ResultState:
                    /* リザルトの処理 */
                    //if (リザルトが終わったか？)
                    //{
                        //その後UIを表示
                        ActiveUI();
                        m_LoadState = LoadState.LaserState;
                    //}
                    break;


                //真ん中の△から青いレーザーが出てきて下から上に一往復
                case LoadState.LaserState:
                    /* バーコードのやつの処理 */
                    //if (バーコードの処理が終わったか？)
                    //  {
                    //UTAUがロード確認と発言
                    m_LoadState = LoadState.LineChangeState;
                    //}
                    break;

                //その後赤いラインが青色になる
                case LoadState.LineChangeState:
                    /* 赤いラインが青色になる処理 */
                    m_LoadState = LoadState.MiniMapDrawState;
                    break;

                //新しいミニマップ表示
                case LoadState.MiniMapDrawState:
                    /* 新しいミニマップ表示 */
                    ActiveMiniMap();
                    m_LoadState = LoadState.OpenDoorState;
                    break;

                case LoadState.OpenDoorState:
                    //その後シャッターを開ける
                    OpenDoor();
                    break;
            }
        }
    }

    /// <summary>
    /// ロード中全般の処理
    /// </summary>
    private void NowLoading()
    {

    }



    /// <summary>
    /// 扉を閉める
    /// </summary>
    private void CloseDoor()
    {
        closeDoor1.GetComponent<MoveObject>().isMotion = true;
        closeDoor2.GetComponent<MoveObject>().isMotion = true;
    }


    /// <summary>
    /// 扉を開く
    /// </summary>
    private void OpenDoor()
    {
        openDoor1.GetComponent<MoveObject>().isMotion = true;
        openDoor2.GetComponent<MoveObject>().isMotion = true;
    }


    /// <summary>
    /// UIを表示する
    /// </summary>
    private void ActiveUI()
    {
        uI.transform.FindChild("PliersUi").gameObject.SetActive(true);
    }

    /// <summary>
    /// ミニマップの表示
    /// </summary>
    private void ActiveMiniMap()
    {
        uI.transform.FindChild("MiniMapUi").gameObject.SetActive(true);
    }


    /// <summary>
    /// UIを非表示にする
    /// </summary>
    private void NonActiveUI()
    {
        uI.transform.FindChild("PliersUi").gameObject.SetActive(false);
    }


    /// <summary>
    /// ミニマップを非表示
    /// </summary>
    private void NonActiveMiniMap()
    {
        uI.transform.FindChild("MiniMapUi").gameObject.SetActive(false);
    }



    /// <summary>
    /// プレイヤーと当たったらイベント開始
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !m_IsOnec)
        {
            m_IsLoadStart = true;
            m_IsOnec = true;
        }
    }
}
