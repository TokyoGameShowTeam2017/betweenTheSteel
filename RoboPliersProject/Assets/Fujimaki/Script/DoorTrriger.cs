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

        excutedEvent_ = true;

        if (open_)
        {
            sceneLoadDoor_.OpenBackDoor(other.gameObject);
        }
        else
        {
            sceneLoadDoor_.CloseBackDoor(other.gameObject);
        }
    }
}
