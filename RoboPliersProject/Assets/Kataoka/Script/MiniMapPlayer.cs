using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayer : MonoBehaviour
{
    private GameObject mGameObject;
    private GameObject mStage;
    private GameObject mMiniMapPlayer;
    // Use this for initialization
    void Start()
    {
        //座標を取得するため
        mMiniMapPlayer = Instantiate(new GameObject());
        ChangeMap();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = mGameObject.transform.position;
        //動く
        mMiniMapPlayer.transform.position = playerPos;
        transform.localPosition = mMiniMapPlayer.transform.localPosition;

        transform.localRotation = Quaternion.Euler(0.0f, GameObject.FindGameObjectWithTag("RawCamera").transform.eulerAngles.y, 0.0f);
    }
    //Mapをチェンジした時呼ぶ
    public void ChangeMap()
    {
        mGameObject = GameObject.FindGameObjectWithTag("Player");
        mStage = GameObject.FindGameObjectWithTag("Stage");
        mMiniMapPlayer.transform.parent = null;
        mMiniMapPlayer.transform.parent = mStage.transform;
    }
}
