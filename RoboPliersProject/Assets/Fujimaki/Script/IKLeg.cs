using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLeg : MonoBehaviour {

    [SerializeField]
    private GameObject _ikTaregtBone;

    public GameObject ikTargetBone { get { return _ikTaregtBone; } }

    public float defaultYOffset { get; private set; }
    public GameObject defaultObject { get; private set; }

    private Vector3 latePosition;
    private Vector3 slerpPosition;

    private void Start()
    {
        defaultObject = new GameObject();
        defaultObject.transform.position = transform.position;
        defaultObject.transform.parent = transform.parent;

        defaultYOffset = transform.localPosition.y;
    }

    private void Update()
    {
        if (slerpPosition != Vector3.zero)
        {
            transform.position = Vector3.Slerp(transform.position, slerpPosition, 0.2f);
        }
    }

    public void TranslateLeg(Vector3 position)
    {
        if (Vector3.Distance(latePosition, position) > 5)
        {
            slerpPosition = position;
        }
        else
        {
            transform.position = position;
            slerpPosition = Vector3.zero;
        }
    }
}
