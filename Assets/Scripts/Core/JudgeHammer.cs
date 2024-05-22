using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeHammer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Coaster"))
        {
            audioSource.clip = audioClip; // Assign the AudioClip to the AudioSource
            audioSource.Play();
            Debug.Log("Kena");
        }
    }
}
