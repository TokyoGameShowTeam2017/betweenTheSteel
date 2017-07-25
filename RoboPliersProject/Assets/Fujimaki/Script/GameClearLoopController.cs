using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearLoopController : SingletonMonoBehaviour<GameClearLoopController>
{

    [SerializeField]
    private bool safeMode;

    [SerializeField]
    private Transform goalPoint;

    [SerializeField]
    private DoorTrriger doortrriger;

    [SerializeField]
    private GameObject sceneLoadDoor;

    [SerializeField]
    private GameObject caveResultCanvas;

    private GameObject drone;

	void Start ()
    {
        SceneLoadInitializer.Instance.nonPauseScene = true;
    }

    private IEnumerator SafeMode()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        yield return SceneManager.LoadSceneAsync("title");
    }

    private IEnumerator MoveDroneGoalPoint()
    {
        //UIを画面外に移動させる
        GameObject.FindGameObjectWithTag("ArmManager").GetComponent<ArmManager>().EndUIMove();

        Vector3 defaultPosition = drone.transform.position;
        Quaternion defaultRotation = drone.transform.rotation;

        float time = 0;
        float arriveTime = 8.0f;

        drone.transform.parent = null;
        while (time < 1)
        {
            time += Time.deltaTime / arriveTime;
            drone.transform.position = Vector3.Lerp(defaultPosition, goalPoint.position, time);
            drone.transform.rotation = Quaternion.Lerp(defaultRotation, goalPoint.rotation, time);
            yield return null;
        }

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        doortrriger.Execute(drone);
    }

    private IEnumerator CanvasCount()
    {
        yield return new WaitForSeconds(2);
        Instantiate(caveResultCanvas, Vector3.zero, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        SceneLoadInitializer.Instance.gameClear = true;
        drone = GameObject.FindGameObjectWithTag("RawCamera");

        goalPoint.transform.parent = sceneLoadDoor.transform;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(false);

        if (safeMode)
        {
            StartCoroutine(SafeMode());
        }
        else
        {
            StartCoroutine(MoveDroneGoalPoint());
            StartCoroutine(CanvasCount());
        }
    }


}
