using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausepPocessor : MonoBehaviour {

    [SerializeField]
    private GameObject pausePrefab;
    [SerializeField]
    private GameObject pauseFramePrefab;

    private PauseController pauseController;

	void Update ()
    {
        //今リザルト画面中か調べる処理
        if (SceneManager.GetActiveScene().name != "cave" && !SceneLoadInitializer.Instance.pauseNot)
        {
            //スタートボタンを押したら
            if (Input.GetButtonDown("XBOXStart"))
            {
                //SE
                SoundManager.Instance.PlaySe("enter");

                if (SceneLoadInitializer.Instance.nonPauseScene)
                {
                    return;
                }
                if (pauseController == null)
                {
                    GameObject g = Instantiate(pausePrefab, Vector3.zero, Quaternion.identity);
                    pauseController = g.GetComponent<PauseController>();
                    Instantiate(pauseFramePrefab);
                }
            }
        }
	}


}
