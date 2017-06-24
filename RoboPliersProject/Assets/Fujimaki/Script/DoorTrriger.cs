using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrriger : MonoBehaviour {

    [SerializeField, Tooltip("ドアスクリプト")]
    private SceneLoadDoor sceneLoadDoor_;

    [SerializeField, Tooltip("閉まるか閉じるか")]
    private bool open_;

    private bool excutedEvent_;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag != "Player") || (excutedEvent_)) 
        {
            return;
        }

        Execute(other.gameObject);
    }

    public void Execute(GameObject g)
    {
        excutedEvent_ = true;

        if (open_)
        {
            sceneLoadDoor_.OpenBackDoor(g);
        }
        else
        {
            sceneLoadDoor_.CloseBackDoor(g);
        }
    }
}
