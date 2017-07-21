using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialEventImageSet : MonoBehaviour
{
    private bool mFlag;
    private float mAlpha;
    private float mTime;

    //コントローラーのテクスチャー
    [SerializeField, Tooltip("上から見たコントローラー")]
    public Sprite m_ControllerTextureUp;
    [SerializeField, Tooltip("正面から見たコントローラー")]
    public Sprite m_ControllerTextureFront;

    private Texture2D mControllerTexture;
    private PlayerTextIvent.EventController mController;
    private PlayerTextIvent.EventController mNowController;

    private Dictionary<PlayerTextIvent.EventController, GameObject> mButtonTexture;
    // Use this for initialization
    void Start()
    {
        mButtonTexture = new Dictionary<PlayerTextIvent.EventController, GameObject>();
        mController = PlayerTextIvent.EventController.NO_BUTTON;
        mNowController = mController;
        mButtonTexture.Add(PlayerTextIvent.EventController.L_STICK, transform.FindChild("LeftAnalog").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.R_STICK, transform.FindChild("RightAnalog").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.L_STICK_TRIGGER, transform.FindChild("LeftAnalogTrigger").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.R_STICK_TRIGGER, transform.FindChild("RightAnalogTrigger").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.RT, transform.FindChild("RT").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.RB, transform.FindChild("RB").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.ARM_BUTTON, transform.FindChild("ArmButton").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.X, transform.FindChild("X").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.Y, transform.FindChild("Y").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.A, transform.FindChild("A").gameObject);
        mButtonTexture.Add(PlayerTextIvent.EventController.B, transform.FindChild("B").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (mController == PlayerTextIvent.EventController.NO_BUTTON ||
            !mFlag)
        {
            mTime = 0.0f;
            if (mNowController != PlayerTextIvent.EventController.NO_BUTTON)
            {
                mButtonTexture[mNowController].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            return;
        }
        mTime += 340.0f * Time.deltaTime;
        GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(0.0f,1.0f,mTime/340.0f));
        mButtonTexture[mController].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f-Mathf.Sin(mTime * Mathf.Deg2Rad));
        mNowController = mController;
    }

    public void SetController(PlayerTextIvent.EventController controller)
    {
        mController = controller;
        mAlpha = 0.0f;
        mTime = 0.0f;
        if (controller == PlayerTextIvent.EventController.RT ||
            controller == PlayerTextIvent.EventController.RB||
            controller==PlayerTextIvent.EventController.ARM_BUTTON)
            GetComponent<Image>().sprite = m_ControllerTextureUp;
        else
            GetComponent<Image>().sprite = m_ControllerTextureFront;
    }
    public void SetFlag(bool flag)
    {
        mFlag = flag;
    }
}
