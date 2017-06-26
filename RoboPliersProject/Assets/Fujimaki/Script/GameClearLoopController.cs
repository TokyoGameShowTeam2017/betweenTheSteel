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

    private GameObject drone;

	void Start ()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(false);
        drone = GameObject.FindGameObjectWithTag("RawCamera");

        if (safeMode)
        {
            StartCoroutine(SafeMode());
        }
        else
        {
            StartCoroutine(MoveDroneGoalPoint());
        }
    }

    private IEnumerator SafeMode()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        yield return SceneManager.LoadSceneAsync("Title 1");
    }

    private IEnumerator MoveDroneGoalPoint()
    {
        Vector3 defaultPosition = drone.transform.position;

        float time = 0;
        float arriveTime = 8.0f;

        drone.transform.parent = null;
        while (time < 1)
        {
            time += Time.deltaTime / arriveTime;
            drone.transform.position = Vector3.Lerp(defaultPosition, goalPoint.position, time);
            print(drone.transform.position);
            yield return null;
        }

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        doortrriger.Execute(drone);
    }
	
	void Update ()
    {
		
	}

    public void MoveDrone()
    {
    }
}
