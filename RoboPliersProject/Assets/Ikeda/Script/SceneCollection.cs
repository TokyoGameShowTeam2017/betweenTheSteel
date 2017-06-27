using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneCollection : MonoBehaviour
{

    enum SceneState
    {
        TitleScene,
        MenuScene,
        StageSelect,
        ManualScene,

        None
    }

    private Dictionary<SceneState, GameObject> m_SceneCollections = new Dictionary<SceneState, GameObject>();

    [SerializeField]
    private GameObject m_TitlePrefab;
    [SerializeField]
    private GameObject m_MenuPrefab;
    [SerializeField]
    private GameObject m_StageSelect;
    //[SerializeField]
    //private GameObject m_ManualPrefab;

    private bool m_IsSceneEnd;

    //現在のシーン
    private SceneState m_CurrentScene = SceneState.None;
    //次のシーン
    private SceneState m_NextScene = SceneState.None;


    private IEnumerator MyNameSceneCheck()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "title")
            {
                break;
            }
            yield return null;
        }
        //シーンを生成
        NextSceneInstantiate();
        
        m_CurrentScene = m_NextScene;
    }

    // Use this for initialization
    void Start()
    {
        m_IsSceneEnd = false;

        AddScene(SceneState.TitleScene, m_TitlePrefab);
        AddScene(SceneState.MenuScene, m_MenuPrefab);
        AddScene(SceneState.StageSelect, m_StageSelect);
        //AddScene(SceneState.ManualScene, m_ManualPrefab);

        m_CurrentScene = SceneState.None;
        m_NextScene = SceneState.TitleScene;


        StartCoroutine(MyNameSceneCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsSceneEnd)
        {
            //現在のシーンを消去
            CurrentSceneDelete();

            //次のシーンを実体化
            NextSceneInstantiate();

            m_IsSceneEnd = false;
        }
    }

    /***********************************************************************/

    //シーンの追加
    private void AddScene(SceneState sceneState, GameObject gameObject)
    {
        m_SceneCollections[sceneState] = gameObject;
    }

    //現在のシーンを消去
    private void CurrentSceneDelete()
    {
        Destroy(GameObject.Find(m_SceneCollections[m_CurrentScene].name + "(Clone)"));
        m_CurrentScene = m_NextScene;
    }

    //次のシーンを実体化
    private void NextSceneInstantiate()
    {
        Instantiate(m_SceneCollections[m_NextScene]);
    }

    //次のシーンを指定
    public void SetNextScene(int sceneState)
    {
        m_NextScene = (SceneState)sceneState;
    }

    //シーンが終わったか?
    public void IsEndScene(bool isEndScene)
    {
        m_IsSceneEnd = isEndScene;
    }

    /// <summary>
    /// 現在のシーンを返す
    /// </summary>
    /// <returns></returns>
    public int GetSceneState()
    {
        return (int)m_CurrentScene;
    }
}
