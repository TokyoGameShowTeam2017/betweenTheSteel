using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadInitializer
{

    private static SceneLoadInitializer instance_ = new SceneLoadInitializer();
    public static SceneLoadInitializer Instance { get { return instance_; } }

    public List<GameObject> usedAreas = new List<GameObject>();

    public GameObject usedArea;
    public bool continueScene;
    public bool gameClear;

    private SceneLoadInitializer()
    {
    }

    public void StartLoadScene(string name)
    {
    }
}
