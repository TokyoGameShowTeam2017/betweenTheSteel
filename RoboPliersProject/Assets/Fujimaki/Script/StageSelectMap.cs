using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectMap : MonoBehaviour
{

    private Camera playerCamera;

    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private RawImage thumbnailImage;

    [SerializeField]
    private Texture[] thumbnails;

    [SerializeField]
    private RectMask2D thumbnailMask;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private GameObject titleDroneCamera;
    private int selectNum;

    private bool exit;
    private bool stated;
    private Coroutine loadSceneAnim;

    private void Start()
    {
        exit = true;
        StartCoroutine(BackGroundLoad());
    }

    private IEnumerator BackGroundLoad()
    {
        yield return SceneManager.LoadSceneAsync("Stage01", LoadSceneMode.Additive);

        if (!SceneLoadInitializer.Instance.continueScene)
        {
            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, 1 - time);
                yield return null;
            }
        }else
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }

        GameObject.FindGameObjectWithTag("RawCamera").GetComponent<Camera>().enabled = false;
        GameObject.FindGameObjectWithTag("MinimapManager").GetComponent<MiniMap>().m_DrawMiniMap = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(false);
    }

    public void LoadScene(int num)
    {
        if (loadSceneAnim != null)
        {
            StopCoroutine(loadSceneAnim);
        }

        if (num == -1)
        {
            thumbnailImage.enabled = false;
            loadSceneAnim = StartCoroutine(InLoadSceneAnim(num));
            exit = true;
            return;
        }
        else
        {

            if (exit)
            {
                loadSceneAnim = StartCoroutine(InLoadSceneAnim(num));
            }
            else
            {
                loadSceneAnim = StartCoroutine(OutLoadSceneAnim(num));
            }

            exit = false;
            thumbnailImage.enabled = true;
        }
    }

    //サムネ表示アニメーション
    private IEnumerator InLoadSceneAnim(int num)
    {
        float time = 0;
        thumbnailImage.texture = thumbnails[num - 1];

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 3;
            thumbnailImage.transform.localPosition = new Vector3(Mathf.Lerp(50, 0, time), 0, 0);
            thumbnailImage.color = new Color(thumbnailImage.color.r, thumbnailImage.color.g, thumbnailImage.color.b, time);

            yield return null;
        }
    }

    //サムネ日表示アニメーション
    private IEnumerator OutLoadSceneAnim(int num)
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 3;
            thumbnailImage.transform.localPosition = new Vector3(Mathf.Lerp(0, -50, time), 0, 0);
            thumbnailImage.color = new Color(thumbnailImage.color.r, thumbnailImage.color.g, thumbnailImage.color.b, 1 - time);

            yield return null;
        }

        if (num > 0)
        {
            StartCoroutine(InLoadSceneAnim(num));
        }
    }

    //テスト用
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            selectNum++;
            if (selectNum > 3)
            {
                selectNum = 1;
            }

            LoadScene(selectNum);
        }

        if ((Input.GetKeyDown(KeyCode.F) && (!stated)))
        {

            StartCoroutine(StartOtherScene(selectNum));
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartFirstScene();
        }*/

    }

    //ステージ１を開始
    public void StartFirstScene()
    {
        if (stated)
        {
            return;
        }

        stated = true;
        GameObject.FindGameObjectWithTag("RawCamera").GetComponent<Camera>().enabled = true;
        StartCoroutine(StartScene(1, titleDroneCamera));
    }

    private IEnumerator StartFirstSceneAnim()
    {
        yield return null;
    }

    public void StartOtherScene(int num)
    {
        StartCoroutine(StartOtherSceneAnim(num));
    }

    //ステージ１以外のステージを開始
    private IEnumerator StartOtherSceneAnim(int num)
    {

        if (stated)
        {
            yield break;
        }

        stated = true;
        float time = 0;

        //ステージサムネイルを画面いっぱいに
        RectTransform r = thumbnailMask.GetComponent<RectTransform>();
        while (time < 1)
        {
            time += Time.deltaTime;
            r.sizeDelta = new Vector2(Mathf.Lerp(670, 1280, time), Mathf.Lerp(390, 720, time));

            yield return null;
        }

        Destroy(GameObject.FindGameObjectWithTag("Player"));

        string loadedScene = "Stage0" + num;


        SceneManager.UnloadSceneAsync("Stage01");
        yield return SceneManager.LoadSceneAsync(loadedScene, LoadSceneMode.Additive);

        StartCoroutine(StartScene(num, GameObject.FindGameObjectWithTag("ThumbnailCamera")));
    }

    //カメラをプレイヤーに繊維アニメション
    private IEnumerator StartScene(int num, GameObject startObj)
    {
        float time = 0;

        //カメラアニメーション準備
        GameObject target = GameObject.FindGameObjectWithTag("RawCamera");

        Vector3 defaultPosition = startObj.transform.position;
        Vector3 targetPosition = target.transform.position;

        Quaternion defaultRotation = startObj.transform.rotation;
        Quaternion targetRotation = target.transform.rotation;

        sceneCamera.enabled = false;

        //フェードアウト
        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            thumbnailImage.color = new Color(thumbnailImage.color.r, thumbnailImage.color.g, thumbnailImage.color.b, 1 - time);
            target.transform.position = defaultPosition;
            target.transform.rotation = defaultRotation;

            yield return null;
        }

        GameObject.FindGameObjectWithTag("MinimapManager").GetComponent<MiniMap>().m_DrawMiniMap = true;

        //カメラアニメーション
        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime / 2.0f;
            target.transform.position = Vector3.Lerp(defaultPosition, targetPosition, time);
            target.transform.rotation = Quaternion.Lerp(defaultRotation, targetRotation, time);

            yield return null;
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(true);
        target.GetComponent<Camera>().enabled = true;
        camera.enabled = false;

        SceneManager.UnloadSceneAsync("title");
    }

    private void AllCompleat()
    {

    }
}
