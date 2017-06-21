using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlock : MonoBehaviour
{
    private List<GameObject> cubes;
    // Use this for initialization
    void Start()
    {
        cubes = new List<GameObject>();

        Transform[] mTransforms;
        mTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (Transform trans in mTransforms)
        {
            if(trans.gameObject!=gameObject)
            cubes.Add(trans.gameObject);
        }
        //pre = Instantiate(obj, transform);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (var i in cubes)
            {
                i.transform.parent = null;
            }
            for (int i = 0; i <=cubes.Count-1; i++)
            {
                if (i != cubes.Count-1)
                    cubes[i].transform.parent = cubes[i +1].transform;
            }
            cubes[cubes.Count-1].transform.parent=transform;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (var i in cubes)
            {
                i.transform.parent = null;
            }
            for (int i = cubes.Count-1; i >= 0; i--)
            {
                if (i != 0)
                    cubes[i].transform.parent = cubes[i-1].transform;
            }
            cubes[0].transform.parent = transform;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
    }

}
