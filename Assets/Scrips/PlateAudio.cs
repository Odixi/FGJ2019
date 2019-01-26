 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateAudio : MonoBehaviour
{
    public Animator animator;
    public TimeController timeController;
    AudioSource audioSource;
    public AudioClip landSound;
    public AudioClip hitSound;
    bool playingSound = false;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    void Update()
    {
        print(timeController.CurrentTime);
        if (animator.GetCurrentAnimatorStateInfo(0).speedMultiplier > 0)
        {
            if (!playingSound)
            {
                if (timeController.CurrentTime > 0.05 && timeController.CurrentTime < 0.06)
                {
                    print("play hit");
                    audioSource.clip = hitSound;
                    audioSource.Play();
                }
                else if (timeController.CurrentTime > 0.11 && timeController.CurrentTime < 0.12)
                {
                    print("play land");
                    audioSource.clip = landSound;
                    audioSource.Play();
                }
            }
        }
    }
}
