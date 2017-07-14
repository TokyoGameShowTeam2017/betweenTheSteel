using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSetParticle : MonoBehaviour
{
    private ParticleSystem mParticle;
    // Use this for initialization
    void Start()
    {
        mParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mParticle.isPlaying)
            Destroy(gameObject);
    }
}
