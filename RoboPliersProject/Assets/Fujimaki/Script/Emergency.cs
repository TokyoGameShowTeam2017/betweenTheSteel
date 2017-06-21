using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Emergency : MonoBehaviour {

	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadSceneAsync("Stage01");
        }
	}
}
