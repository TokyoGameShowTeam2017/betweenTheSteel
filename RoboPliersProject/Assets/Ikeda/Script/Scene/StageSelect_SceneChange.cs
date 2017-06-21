using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect_SceneChange : MonoBehaviour {

    public int StageNum = 0;

    RectTransform image;

    public GameObject m_Stage1;
    public GameObject m_Stage2;
    public GameObject m_Stage3;
    public GameObject m_Stage4;
    public GameObject m_Stage5;

    public GameObject m_Player;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StageNum = StageNum - 1;
            if (StageNum - 1 < -1)
            {
                StageNum = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StageNum = StageNum + 1;
            if (StageNum + 1 > 5)
            {
                StageNum = 4;
            }
        }

        switch (StageNum)
        {
            case 0:
                image = GameObject.Find("Image").GetComponent<RectTransform>();
                image.localPosition = new Vector3(-50.0f, 95.0f, 0.0f);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //UnityEngine.SceneManagement.SceneManager.LoadScene("test 1");
                    m_Stage1.gameObject.SetActive(true);
                    m_Player.transform.position = new Vector3(197, 125, -735);
                    m_Player.gameObject.SetActive(true);
                    this.transform.parent.gameObject.SetActive(false);
                }
                break;

            case 1:
                image = GameObject.Find("Image").GetComponent<RectTransform>();
                image.localPosition = new Vector3(-50.0f, 50.0f, 0.0f);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Stage2.gameObject.SetActive(true);
                    m_Player.transform.position = new Vector3(197, 125, -620);
                    m_Player.gameObject.SetActive(true);
                    this.transform.parent.gameObject.SetActive(false);
                }
                break;

            case 2:
                image = GameObject.Find("Image").GetComponent<RectTransform>();
                image.localPosition = new Vector3(-50.0f, 7.0f, 0.0f);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Stage3.gameObject.SetActive(true);
                    m_Player.transform.position = new Vector3(197, 125, -500);
                    m_Player.gameObject.SetActive(true);
                    this.transform.parent.gameObject.SetActive(false);
                }
                break;

            case 3:
                image = GameObject.Find("Image").GetComponent<RectTransform>();
                image.localPosition = new Vector3(-50.0f, -34.0f, 0.0f);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("stage4");
                }
                break;

            case 4:
                image = GameObject.Find("Image").GetComponent<RectTransform>();
                image.localPosition = new Vector3(-50.0f, -79.0f, 0.0f);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("stage5");
                }
                break;
        }
    }
}
