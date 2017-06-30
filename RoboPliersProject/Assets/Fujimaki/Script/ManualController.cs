using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualController : MonoBehaviour {

    [SerializeField]
    private Texture[] images;

    [SerializeField]
    private RawImage renderImage;

    private Coroutine fadeAnimation;
    private int selectNum;

    private bool input;
    private bool enableInput;

    private void Awake()
    {
        renderImage.color = new Color(renderImage.color.r, renderImage.color.g, renderImage.color.b, 0);
    }

    // Use this for initialization
    void Start ()
    {
        selectNum = 0;
        renderImage.texture = images[selectNum];
        StartCoroutine(Fade(true));
    }
	
	// Update is called once per frame
	void Update ()
    {
        

        if ((InputManager.GetStick() != StickState.Left) && (InputManager.GetStick() != StickState.Right))
        {
            input = false;
        }

        if (input)
        {
            return;
        }

        if (InputManager.GetStick() == StickState.Right)
        {
            input = true;
            selectNum++;
            if (selectNum > images.Length-1)
            {
                selectNum = 0;
            }

            renderImage.texture = images[selectNum];
        }

        if (InputManager.GetStick() == StickState.Left)
        {
            input = true;
            selectNum--;
            if (selectNum <0)
            {
                selectNum = images.Length - 1;
            }

            renderImage.texture = images[selectNum];
        }
    }

    public void CanvasDestroy()
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool display)
    {

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 3;
            float alpha = display ? time : 1 - time;
            renderImage.color = new Color(renderImage.color.r, renderImage.color.g, renderImage.color.b, alpha);
            yield return null;
        }

        if (!display)
        {
            Destroy(gameObject);
        }
    }
}
