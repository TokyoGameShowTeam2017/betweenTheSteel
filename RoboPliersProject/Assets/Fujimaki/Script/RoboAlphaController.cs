using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboAlphaController : MonoBehaviour {

    [SerializeField]
    private Renderer[] renderer;

    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private float alphaPower;

    private void Awake()
    {

    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        float alpha = Vector3.Distance(camera.transform.position, transform.position);
        alpha = Mathf.Pow(alpha / maxDistance, alphaPower);

        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].materials[0].SetFloat("_Distance",alpha);
            renderer[i].materials[1].SetFloat("_Distance", alpha);
        }
	}
}
