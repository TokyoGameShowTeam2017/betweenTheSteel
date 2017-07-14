using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSteam : MonoBehaviour
{
    private float mTime;
    //ランダムタイム
    private float mRandomTime;
    [SerializeField, Tooltip("どのぐらいの頻度で")]
    public float m_PushhTime;
    [SerializeField, Tooltip("どのくらいの頻度でランダム値")]
    public float m_PushhRandmTime;

    private ParticleSystem mParticle;
    // Use this for initialization
    void Start()
    {
        mParticle = GetComponent<ParticleSystem>();
        mTime = 0.0f;
        mRandomTime = Random.Range(-mRandomTime, mRandomTime);
    }

    // Update is called once per frame
    void Update()
    {
        mTime += Time.deltaTime;

        if (mTime >= m_PushhTime + m_PushhRandmTime)
        {
            mTime = 0.0f;
            mRandomTime = Random.Range(-mRandomTime, mRandomTime);
            SoundManager.Instance.PlaySe("steam");
            mParticle.Play();
        }
    }
}
