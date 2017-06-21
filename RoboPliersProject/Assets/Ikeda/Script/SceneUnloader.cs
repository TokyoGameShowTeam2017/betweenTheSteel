using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField]
    private GameObject UnloadObject;

    private bool isUnload = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        /* シーンをアンロードするやり方 */
        //if (isUnload)
        //{
        //    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(UnloadSceneName);
        //}

        /* プレハブを非アクティブ化するやり方 */
        if (isUnload)
        {
            UnloadObject.gameObject.SetActive(false);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isUnload = true;
        }
    }
}
