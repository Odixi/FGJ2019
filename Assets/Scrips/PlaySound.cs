 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    AudioSource audioSource;
    public float time;
    public float timeWindow = 0.01f;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (TimeController.Instance.CurrentTime > time - timeWindow && TimeController.Instance.CurrentTime < time + timeWindow)
            {
                audioSource.Play();
            }
        }

    }
}
