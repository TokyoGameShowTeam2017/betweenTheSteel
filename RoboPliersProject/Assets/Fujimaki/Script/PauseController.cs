using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private RawImage[] texts;

    [SerializeField]
    private GameObject manualPrefab;
    [SerializeField]
    private GameObject selectStagePrefab;
    //[SerializeField]
    //private Canvas selectCanvas;

    private int select;

    private Coroutine fade;
    private bool input;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        fade = StartCoroutine(FadeGroup(true));
        select = 0;
        UpdateMenuColor();

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("XBOXStart"))
        {
            StartCoroutine(FadeGroup(false));
        }

        if (GameObject.Find("PausePerformanceCanvas(Clone)").GetComponent<PausePerformance>().GetPauseState() == PausePerformance.PauseState.DecisionWait)
        {
            PauseInput();

            if (Input.GetButtonDown("XBOXArm3"))
            {
                select = 4;
                UpdateMenuColor();
                StartCoroutine(FadeGroup(false));
                DestroyPausePerformancePrefab();
            }

            if (InputWrap())
            {
                switch (select)
                {
                    case 0:
                        StartCoroutine(FadeGroup(false));
                        DestroyPausePerformancePrefab();
                        break;

                    case 1:
                        Instantiate(selectStagePrefab);
                        //GameObject.Find("RotationOrigin").GetComponent<PauseStageSelectMap>().enabled = true;
                        break;

                    case 2:
                        Instantiate(manualPrefab);
                        break;

                    case 3:
                        StartCoroutine(LoadScene("Title"));
                        DestroyPausePerformancePrefab();
                        break;

                    case 4:
                        StartCoroutine(FadeGroup(false));
                        DestroyPausePerformancePrefab();
                        break;
                }
            }
        }
    }

    private void PauseInput()
    {
        if ((InputManager.GetStick() == StickState.Up) && !input)
        {
            input = true;
            select--;
            if (select < 0)
            {
                select = texts.Length - 1;
            }
            SoundManager.Instance.PlaySe("select");

            UpdateMenuColor();
        }

        if ((InputManager.GetStick() == StickState.Down) && !input)
        {
            input = true;
            select++;
            if (select == texts.Length)
            {
                select = 0;
            }
            SoundManager.Instance.PlaySe("select");

            UpdateMenuColor();
        }


        if ((InputManager.GetStick() != StickState.Up) && (InputManager.GetStick() != StickState.Down))
        {
            input = false;
        }
    }

    private void UpdateMenuColor()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = i == select ? new Color(0, 1, 1, 1) : new Color(1, 1, 1, 1);
        }
    }

    private void DestroyPausePerformancePrefab()
    {
        Destroy(GameObject.Find("PausePerformanceCanvas(Clone)"));
    }

    public int GetSelectNum()
    {
        return select;
    }

    public void DestroyCanvas()
    {
        StopCoroutine(fade);
        StartCoroutine(FadeGroup(false));
    }

    private IEnumerator LoadScene(string name)
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 2;
            fadeImage.color = new Color(0, 0, 0, time);

            yield return null;
        }

        foreach (var g in SceneLoadInitializer.Instance.usedAreas)
        {
            Destroy(g);
        }

        Destroy(GameObject.FindGameObjectWithTag("Player"));

        SceneManager.LoadScene(name);
    }

    private IEnumerator FadeGroup(bool display)
    {
        float time = 0;
        while (time < 1)
        {

            if (display)
            {
                canvasGroup.alpha = time;
            }
            else
            {
                canvasGroup.alpha = 1 - time;
            }
            time += Time.deltaTime * 3;

            yield return null;
        }

        if (!display)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(true);
            Destroy(gameObject);
        }
    }

    //ボタンの押されたとき
    private bool InputWrap()
    {
        int id = 0;

        //if (Input.GetButtonDown("XBOXArm1"))
        //    id = 1;
        if (Input.GetButtonDown("XBOXArm2"))
            id = 2;
        //if (Input.GetButtonDown("XBOXArm3"))
        //    id = 3;
        //if (Input.GetButtonDown("XBOXArm4"))
        //    id = 4;

        if (id != 0)
            return true;

        return false;
    }

}
