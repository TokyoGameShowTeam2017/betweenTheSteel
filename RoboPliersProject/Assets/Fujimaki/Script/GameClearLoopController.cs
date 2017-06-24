using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearLoopController : SingletonMonoBehaviour<GameClearLoopController>
{

    [SerializeField]
    private Transform goalPoint;

    [SerializeField]
    private DoorTrriger doortrriger;

    private GameObject drone;

	void Start ()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SetIsMoveAndUI(false);
        drone = GameObject.FindGameObjectWithTag("RawCamera");

        StartCoroutine(MoveDroneGoalPoint());
    }

    private IEnumerator MoveDroneGoalPoint()
    {
        Vector3 defaultPosition = drone.transform.position;
        float time = 0;
        float arriveTime = 8.0f;
        while (time < 1)
        {
            time += Time.deltaTime / arriveTime;
            drone.transform.position = Vector3.Lerp(defaultPosition, goalPoint.position, time);
            print(drone.transform.position);
            yield return null;
        }

        doortrriger.Execute(drone);
    }
	
	void Update ()
    {
		
	}

    public void MoveDrone()
    {
    }
}
