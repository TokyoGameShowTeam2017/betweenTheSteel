using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader
{

    private static SceneLoader _singleInstance = new SceneLoader();

    public static SceneLoader GetInstance()
    {
        return _singleInstance;
    }

    private SceneLoader()
    {
    }

    public PlayerMove _playerMove;
}
