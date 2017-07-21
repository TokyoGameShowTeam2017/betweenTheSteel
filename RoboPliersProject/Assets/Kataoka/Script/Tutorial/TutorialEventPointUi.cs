using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventPointUi : MonoBehaviour
{

    //ポイントにするオブジェクト
    public GameObject mPointObject;
    //ポイントUI表示するかどうか
    public bool mIsDrawUi = false;
    //自身のtrans
    private RectTransform mRectTrans;
    //カメラ見えているか
    private bool mCameraViewObject;
    // Use this for initialization
    void Start()
    {
        mRectTrans = GetComponent<RectTransform>();
        mCameraViewObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        //表示されているとき
        if (mCameraViewObject)
        {
            GetComponent<CanvasGroup>().alpha = 1.0f;

            Vector2 objectPosition = RectTransformUtility.WorldToScreenPoint(
                GameObject.FindGameObjectWithTag("RawCamera").GetComponent<Camera>(),
                 mPointObject.transform.position);

            Debug.Log(objectPosition);
            mRectTrans.position = objectPosition;
        }
        else
            GetComponent<CanvasGroup>().alpha = 0.0f;



        mCameraViewObject = false;
    }

    void SetPointObject(GameObject pointObject)
    {
        mPointObject = pointObject;
    }
    void SetDrawFlag(bool flag)
    {
        mIsDrawUi = flag;
    }

    private void OnWillRenderObject()
    {
        if (Camera.current.tag == "RawCamera")
        {
            mCameraViewObject = true;
        }
    }
}
