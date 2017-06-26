﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadDoor : MonoBehaviour {

    private float doorSlideDistance_ = 6.4f;
    

    [SerializeField, Tooltip("次の読み込むシーン名")]
    private string nextSceneName_;

    [SerializeField, Tooltip("破棄するシーン名")]
    private string unloadSceneName_;

    [SerializeField,Tooltip("シーンロード後のStartCrriderの設置位置")]
    private Vector3 startPosition;

    [SerializeField, Tooltip("前の上のドア")]
    private GameObject frontUpDoor_;

    [SerializeField, Tooltip("前の下のドア")]
    private GameObject frontDownDoor_;

    [SerializeField, Tooltip("後ろ上のドア")]
    private GameObject backUpDoor_;

    [SerializeField, Tooltip("後ろ下のドア")]
    private GameObject backDownDoor_;

    [SerializeField, Tooltip("ローディングキャンバス")]
    private GameObject loadingCanvasPrefab_;

    [SerializeField, Tooltip("エミッシブマテリアル")]
    private Renderer[] emissiveRenders_;

    [SerializeField, Tooltip("ライト")]
    private Light light_;

    [SerializeField, Tooltip("ライト2")]
    private Light light2_;

    [SerializeField, Tooltip("リフレクションプローブ")]
    private ReflectionProbe reflection_;

    [SerializeField, Tooltip("青色")]
    private Color blueColor_;

    [SerializeField, Tooltip("ライト青色")]
    private Color blueLightColor_;

    [SerializeField, Tooltip("スキャン板")]
    private GameObject scanPlane_;

    [SerializeField, Tooltip("最終シーン用フラグ")]
    private bool endFrag_;

    public delegate void AnimEndCallback();
    private GameObject playerObject_;

    void Start ()
    {
        SceneLoadInitializer.Instance.usedAreas.Add(gameObject);
        DontDestroyOnLoad(transform.gameObject);
        reflection_.RenderProbe();
        scanPlane_.SetActive(false);
    }

    private IEnumerator DoorAnim(bool open, bool front, AnimEndCallback callBack = null)
    {

        float firstY = open ? 0 : doorSlideDistance_;
        float endY = open ? doorSlideDistance_ : 0;

        GameObject upDoor = front ? frontUpDoor_ : backUpDoor_;
        GameObject downDoor = front ? frontDownDoor_ : backDownDoor_;

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            Vector3 upDoorLocalposition = upDoor.transform.localPosition;
            upDoor.transform.localPosition = Vector3.Lerp(new Vector3(upDoorLocalposition.x, firstY, upDoorLocalposition.z), new Vector3(upDoorLocalposition.x, endY, upDoorLocalposition.z), time);
            downDoor.transform.localPosition = Vector3.Lerp(new Vector3(upDoorLocalposition.x, open ? firstY : -firstY, upDoorLocalposition.z), new Vector3(upDoorLocalposition.x, open ? -endY : endY, upDoorLocalposition.z), time);

            yield return null;
        }

        if (callBack != null)
        {
            callBack();
        }
    }

    public void OpenBackDoor(GameObject player)
    {
        StartCoroutine(DoorAnim(true, false));
        light_.enabled = true;
        light2_.enabled = true;
    }

    public void CloseBackDoor(GameObject player)
    {
        StartCoroutine(DoorAnim(false, false, LoadNextScene));
        playerObject_ = player;
    }

    private void LoadNextScene()
    {
        StartCoroutine(ScanAnim());
        SceneLoadInitializer.Instance.continueScene = true;
        
    }

    private IEnumerator LoadScene()
    {
        Destroy(SceneLoadInitializer.Instance.usedArea);

        yield return SceneManager.LoadSceneAsync("LoadTmpScene", LoadSceneMode.Additive);
        Move();

        yield return SceneManager.UnloadSceneAsync(unloadSceneName_);

        GameObject g = Instantiate(loadingCanvasPrefab_);
        yield return SceneManager.LoadSceneAsync(nextSceneName_, LoadSceneMode.Additive);

        if (endFrag_)
        {
            Destroy(playerObject_);
        }

        yield return SceneManager.UnloadSceneAsync("LoadTmpScene");

        Destroy(g);
        Loaded();
    }

    private IEnumerator ScanAnim()
    {
        scanPlane_.SetActive(true);

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime/2;
            scanPlane_.transform.localEulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), new Vector3(-90, 0, 0), time);
            yield return null;
        }

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / 3;
            scanPlane_.transform.localEulerAngles = Vector3.Lerp(new Vector3(-90, 0, 0), new Vector3(90, 0, 0), time);
            yield return null;
        }

        scanPlane_.SetActive(false);
        StartCoroutine(LoadScene());
    }

    private void Loaded()
    {
        for (int i = 0; i < emissiveRenders_.Length; i++)
        {
            emissiveRenders_[i].materials[1].SetColor("_EmissionColor", blueColor_ * 1.2f);
            emissiveRenders_[i].materials[1].SetColor("_Color", blueColor_ * 1.2f);
        }

        light_.color = blueLightColor_;
        light2_.color = blueLightColor_;
        reflection_.RenderProbe();

        StartCoroutine(DoorAnim(true, true));
        
        SceneLoadInitializer.Instance.usedArea = gameObject;
    }


    private void Move()
    {
        playerObject_.transform.parent = transform;
        transform.position = startPosition;
        playerObject_.transform.parent = null;
    }
}
