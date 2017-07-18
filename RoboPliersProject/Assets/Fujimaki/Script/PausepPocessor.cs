using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausepPocessor : MonoBehaviour {

    [SerializeField]
    private GameObject pausePrefab;
    [SerializeField]
    private GameObject pauseFramePrefab;

    private PauseController pauseController;

	void Update ()
    {
        if (Input.GetButtonDown("XBOXStart"))
        {
            SoundManager.Instance.PlaySe("enter");

            if (SceneLoadInitializer.Instance.nonPauseScene)
            {
                return;
            }

            if (pauseController==null)
            {
                GameObject g = Instantiate(pausePrefab, Vector3.zero, Quaternion.identity);
                pauseController = g.GetComponent<PauseController>();
                Instantiate(pauseFramePrefab);
            }
        }
	}
}
