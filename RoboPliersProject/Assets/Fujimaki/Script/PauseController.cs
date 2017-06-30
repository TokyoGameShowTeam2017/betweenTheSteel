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
        if ((InputManager.GetStick() == StickState.Up) && !input) 
        {
            input = true;
            select--;
            if (select < 0)
            {
                select = texts.Length - 1;
            }

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

            UpdateMenuColor();
        }

        if (InputManager.GetSelectArm().isDown)
        {
            switch (select)
            {
                case 0:
                    StartCoroutine(FadeGroup(false));
                    break;
                case 1:
                                        StartCoroutine(LoadScene("Title"));
                    break;
                case 2:
                    break;
            }
        }

        if ((InputManager.GetStick() != StickState.Up) && (InputManager.GetStick() != StickState.Down))
        {
            input = false;
        }

    }

    private void UpdateMenuColor()
    {
        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].color = i == select ? new Color(0, 1, 1, 1) : new Color(1, 1, 1, 1);
        }
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
            time += Time.deltaTime * 10;

            yield return null;
        }

        if (!display)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(true);
            Destroy(gameObject);
        }
    }
}
