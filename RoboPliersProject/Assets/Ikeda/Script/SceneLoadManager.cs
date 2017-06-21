using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
{

    public delegate void LoadCompleatCallback();

    //新しくシーンを読み込む
    public void LoadNextScene(string nextSceneName, LoadCompleatCallback callback, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadScene(nextSceneName, callback, loadMode));
    }

    //シーンを破棄する
    public void UnLoadNextScene(string nextSceneName, LoadCompleatCallback callback)
    {
        StartCoroutine(UnloadScene(nextSceneName, callback));
    }

    private IEnumerator LoadScene(string name, LoadCompleatCallback callback, LoadSceneMode loadMode)
    {
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        callback();
    }

    private IEnumerator UnloadScene(string name, LoadCompleatCallback callback)
    {
        yield return SceneManager.UnloadSceneAsync(name);
        callback();
    }
}
