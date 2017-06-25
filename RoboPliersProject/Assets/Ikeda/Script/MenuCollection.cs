using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuCollection : MonoBehaviour
{

    enum SceneState
    {
        MenuEnterState,
        MenuSelectState,

        None
    }

    enum MenuState
    {
        GameStart,
        StageSelect,
        Manual,
        Exit,
        Back,

        None
    }

    [SerializeField, Tooltip("選択されていないα値を下げるスピードを設定")]
    private float m_LowerSpeed = 0.025f;
    [SerializeField, Tooltip("選択されたα値を下げるスピードを設定")]
    private float m_ChoiceLowerSpeed = 0.04f;
    [SerializeField, Tooltip("決定した選択を何倍するか")]
    private float m_ScaleDouble = 1.0f;

    private float m_LowerAlpha = 1.0f;
    private float m_ChoiceLowerAlpha = 1.0f;
    private MenuState m_State;
    private Vector3 m_Scale;
    private bool m_IsSceneEnd = false;
    //選択されている番号
    private int m_MenuNum;
    private int m_BeflorNum;

    private SceneState m_MenuState;

    private StickState m_StickState;
    private bool m_Once;
    // Use this for initialization
    void Start()
    {
        m_StickState = StickState.None;
        m_Once = false;

        m_MenuNum = 0;
        m_BeflorNum = 0;
        m_IsSceneEnd = false;
        m_MenuState = SceneState.MenuEnterState;
        m_State = MenuState.None;
        GameObject.Find("sideFrame").GetComponent<MenuFrame>().InitializeBackRate();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_MenuState)
        {
            case SceneState.MenuEnterState:
                //ノイズを表示
                GameObject.Find("Noises").GetComponent<Noise>().NoiseEnter();

                //フレームを表示
                GameObject.Find("sideFrame").GetComponent<MenuFrame>().FrameEnter();

                //メニューを表示
                transform.FindChild("selectMenu").GetComponent<SelectMenuEnter>().MenusEnter();

                break;

            case SceneState.MenuSelectState:
                //メニュー選択
                MenuSelect();
                StickState l_state = InputManager.GetStick();
                print(l_state);

                //選択されているメニューの色を変える
                MenuColoring();

                //選択されているメニューで決定
                MenuDecision();
                break;
        }
    }

    /// <summary>
    /// メニュー選択
    /// </summary>
    private void MenuSelect()
    {
        m_StickState = InputManager.GetStick();

        switch (m_StickState)
        {
            case StickState.Up:
                if (!m_Once)
                {
                    m_Once = true;
                    m_MenuNum = m_MenuNum - 1;
                    if (m_MenuNum - 1 < -1)
                    {
                        m_MenuNum = 0;
                    }
                }
                break;

            case StickState.Down:
                if (!m_Once)
                {
                    m_Once = true;
                    if (m_MenuNum == 4) return;
                    m_MenuNum = m_MenuNum + 1;
                    if (m_MenuNum + 1 > 4)
                    {
                        m_MenuNum = 3;
                    }
                }
                break;

            case StickState.Right:
                if (!m_Once)
                {
                    if (m_MenuNum != 4) return;
                    m_MenuNum = m_BeflorNum;
                }
                break;

            case StickState.Left:
                if (!m_Once)
                {
                    if (m_MenuNum == 4) return;
                    m_BeflorNum = m_MenuNum;
                    m_MenuNum = 4;
                }
                break;

            default:
                m_Once = false;
                break;
        }
    }

    private void MenuColoring()
    {
        switch (m_MenuNum)
        {
            case 0:
                //大きさを変える
                GameObject.Find("startgame").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                //色を変える
                GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(0, 1, 1);
                GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                break;

            case 1:
                //大きさを変える
                GameObject.Find("selectstage").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                //色を変える
                GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(0, 1, 1);
                GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                break;

            case 2:
                //大きさを変える
                GameObject.Find("manual").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                //色を変える
                GameObject.Find("manual").GetComponent<RawImage>().color = new Color(0, 1, 1);
                GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                break;

            case 3:
                //大きさを変える
                GameObject.Find("exit").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("backMenu").transform.localScale = new Vector3(1f, 1f, 1f);

                //色を変える
                GameObject.Find("exit").GetComponent<RawImage>().color = new Color(0, 1, 1);
                GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(1, 1, 1);
                break;

            case 4:
                //大きさを変える
                GameObject.Find("backMenu").transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
                GameObject.Find("startgame").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("selectstage").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("manual").transform.localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("exit").transform.localScale = new Vector3(1f, 1f, 1f);

                //色を変える
                GameObject.Find("backMenu").GetComponent<RawImage>().color = new Color(0, 1, 1);
                GameObject.Find("startgame").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("selectstage").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("manual").GetComponent<RawImage>().color = new Color(1, 1, 1);
                GameObject.Find("exit").GetComponent<RawImage>().color = new Color(1, 1, 1);
                break;
        }
    }


    private void MenuDecision()
    {
        switch (m_MenuNum)
        {
            case 0:
                if (InputManager.GetSelectArm().isDown ||Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.GameStart;
                    m_Scale = GameObject.Find("startgame").transform.localScale;
                }
                if (m_State == MenuState.GameStart)
                {
                    if (m_LowerAlpha >= 0) m_LowerAlpha -= m_LowerSpeed;

                    if (m_ChoiceLowerAlpha >= 0)
                    {
                        m_ChoiceLowerAlpha -= m_ChoiceLowerSpeed;
                        m_Scale += (m_Scale * m_ScaleDouble) / 60;
                    }

                    GameObject.Find("startgame").transform.localScale = new Vector3(m_Scale.x, m_Scale.y, m_Scale.z);
                    GameObject.Find("Canvas menu(Clone)").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;

                    GameObject.Find("RotationOrigin").GetComponent<StageSelectMap>().StartFirstScene();
                }
                break;

            case 1:
                if (InputManager.GetSelectArm().isDown || Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.StageSelect;
                    m_Scale = GameObject.Find("selectstage").transform.localScale;
                }
                if (m_State == MenuState.StageSelect)
                {
                    if (m_LowerAlpha >= 0) m_LowerAlpha -= m_LowerSpeed;

                    if (m_ChoiceLowerAlpha >= 0)
                    {
                        m_ChoiceLowerAlpha -= m_ChoiceLowerSpeed;
                        m_Scale += (m_Scale * m_ScaleDouble) / 60;
                    }

                    GameObject.Find("selectstage").transform.localScale = new Vector3(m_Scale.x, m_Scale.y, m_Scale.z);
                    GameObject.Find("menuselectback1").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;
                    GameObject.Find("menuselectback2").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;
                    GameObject.Find("menuselectback3").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;
                    GameObject.Find("menuselectback4").GetComponent<CanvasGroup>().alpha = m_ChoiceLowerAlpha;

                    GameObject.Find("sideFrame").GetComponent<MenuFrame>().SpreadFrame();
                }
                break;

            case 2:
                if (InputManager.GetSelectArm().isDown || Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.Manual;
                }
                if (m_State == MenuState.Manual)
                {

                }
                break;

            case 3:
                if (InputManager.GetSelectArm().isDown || Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.Exit;
                }
                if (m_State == MenuState.Exit)
                {
                    Application.Quit();
                }
                break;

            case 4:
                if (InputManager.GetSelectArm().isDown || Input.GetKeyDown(KeyCode.Space))
                {
                    m_State = MenuState.Back;
                }
                if (m_State == MenuState.Back)
                {
                    GameObject.Find("SceneCollection").GetComponent<SceneCollection>().SetNextScene(0);
                    GameObject.Find("SceneCollection").GetComponent<SceneCollection>().IsEndScene(true);
                }
                break;
        }
    }



    public void SetSceneState(int sceneState)
    {
        m_MenuState = (SceneState)sceneState;
    }

    public int GetMenuState()
    {
        return (int)m_State;
    }
}
