using UnityEngine;

/// <summary>
/// Rotate things
/// </summary>
public class PlatformRotator : MonoBehaviour
{
    public Vector3 rotationAxis;
    public float timeToFullrotation = 5f;

    void FixedUpdate()
    {
        timeToFullrotation = Mathf.Clamp(timeToFullrotation, 0.1f, timeToFullrotation);

        float _uniteRev = 1f / timeToFullrotation;
        float _angularVelocity = 2f * Mathf.PI * _uniteRev;

        transform.Rotate(rotationAxis, _angularVelocity, Space.Self);
    }	
}
