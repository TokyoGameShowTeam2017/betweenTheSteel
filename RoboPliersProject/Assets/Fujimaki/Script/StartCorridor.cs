using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCorridor : MonoBehaviour {


    private void Awake()
    {
        if (SceneLoadInitializer.Instance.continueScene)
        {
            Destroy(gameObject);
        }else
        {
            SceneLoadInitializer.Instance.usedArea = gameObject;
        }
    }
}
