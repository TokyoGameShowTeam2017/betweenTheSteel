using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSteam : MonoBehaviour
{
    private float mTime;
   [SerializeField, Tooltip("どのぐらいの頻度")]



    private ParticleSystem mParticle;
    // Use this for initialization
    void Start()
    {
        mParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
