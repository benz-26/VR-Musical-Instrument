using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerFrame : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private string targetTag;

    public bool useVelocity = true;
    public float minVelocity = 0;
    public float maxVelocity = 2;

    public bool randomizePitch = true;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag(targetTag))
        {
            VelocityEstimator estimator = other.GetComponent<VelocityEstimator>();
            if (estimator && useVelocity)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                float volume = Mathf.InverseLerp(minVelocity, maxVelocity, v);

                if(randomizePitch)
                {
                    audioSource.pitch = Random.Range(minPitch, maxPitch);
                }
                audioSource.PlayOneShot(audioClip,volume);
#if UNITY_EDITOR
                Debug.Log("Ketok");
#endif
            }
            else
            {
                if (randomizePitch)
                {
                    audioSource.pitch = Random.Range(minPitch, maxPitch);
                }
                audioSource.PlayOneShot(audioClip);
            }

        }
    }
}
