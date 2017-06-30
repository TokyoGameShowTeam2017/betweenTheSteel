using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausepPocessor : MonoBehaviour {

    [SerializeField]
    private GameObject pausePrefab;

    private PauseController pauseController;

	void Update ()
    {
        if (Input.GetButtonDown("XBOXStart"))
        {
            if (pauseController==null)
            {
                GameObject g = Instantiate(pausePrefab, Vector3.zero, Quaternion.identity);
                pauseController = g.GetComponent<PauseController>();
            }
        }
	}
}
