using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmColorController : MonoBehaviour {

    [SerializeField]
    private Renderer armA;

    [SerializeField]
    private Color armAcolor;

    [Space(10)]

    [SerializeField]
    private Renderer armB;

    [SerializeField]
    private Color armBcolor;

    [Space(10)]

    [SerializeField]
    private Renderer armX;

    [SerializeField]
    private Color armXcolor;

    [Space(10)]

    [SerializeField]
    private Renderer armY;

    [SerializeField]
    private Color armYcolor;

    private TutorialSetting player;

    //player.tutorialsetting.getisactivearm();

    private void Start()
    {
        player = GetComponent<TutorialSetting>();
        SetColor(armA, armAcolor);
        SetColor(armB, armBcolor);
        SetColor(armX, armXcolor);
        SetColor(armY, armYcolor);
    }

    private void SetColor(Renderer renderer,Color col)
    {
        renderer.materials[1].SetColor("_Color", col);
        renderer.materials[1].SetColor("_EmissionColor", col);
    }

    void Update ()
    {

        Color noEnableColor = new Color(0.5f, 0.5f, 0.5f);
        SetColor(armA, player.GetIsActiveArm(0) ? armAcolor : noEnableColor);
        SetColor(armB, player.GetIsActiveArm(1) ? armBcolor : noEnableColor);
        SetColor(armX, player.GetIsActiveArm(2) ? armXcolor : noEnableColor);
        SetColor(armY, player.GetIsActiveArm(3) ? armYcolor : noEnableColor);
    }
}
