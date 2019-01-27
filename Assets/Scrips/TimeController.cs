using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    public float CurrentTime = 0;
    public float MaxTime = 1;
    public float MinTime = 0;
    public float Speed = 0;
    public float PbSpeed = 1;

    public List<float> keys;

    public List<Animator> animators;

    public Slider timeSlider;
    public List<Slider> keySliders;
    public Text timeValueText;

    public AudioSource musicAmbiendSource;
    public AudioSource musicTimeSource;

    private AudioSource[] audioSources;

    public float volume = 0.4f;
    public float fadeStep = 0.01f;

    public GameObject timeTravelSoundObject;

    private void Start()
    {
        if (Instance != null)
        {
            print("TimeController already exist!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        foreach(var ks in keySliders)
        {
            ks.gameObject.SetActive(false);
        }
        keys = new List<float> {-1,-1,-1,-1};
        audioSources = FindObjectsOfType<AudioSource>();
        SetSoundSpeeds();
    }

    private void Update()
    {
        CurrentTime = animators[0].GetCurrentAnimatorStateInfo(0).normalizedTime;
        var curSpeed = animators[0].GetCurrentAnimatorStateInfo(0).speedMultiplier;

        if ((CurrentTime >= MaxTime && Speed > 0) || (CurrentTime <= MinTime && Speed < 0))
        {
            Speed = 0;
            foreach (var a in animators)
            {
                Speed = 0;
                a.SetFloat("speed", 0);
            }
            SetSoundSpeeds();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f)
        {
            PbSpeed += 0.1f;
            timeValueText.text = PbSpeed.ToString("0.00");
            if (curSpeed != 0)
            {
                var s = curSpeed > 0 ? 1 : -1;
                TimeController.Instance.Play(s * PbSpeed);
                Speed = s * PbSpeed;
                SetSoundSpeeds();
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") < -0.015f)
        {
            PbSpeed -= PbSpeed <= 0.15f ? 0 : 0.1f;
            timeValueText.text = PbSpeed.ToString("0.00");
            if (curSpeed != 0)
            {
                var s = curSpeed > 0 ? 1 : -1;
                TimeController.Instance.Play(s * PbSpeed);
                Speed = s * PbSpeed;
                SetSoundSpeeds();
            }
        }

        // Time Inputs
        if (Input.GetKeyDown(KeyCode.E))
        {
            var ts = TimeController.Instance.Speed > 0 ? 0 :PbSpeed;
            TimeController.Instance.Play(ts);
            SetSoundSpeeds();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var ts = TimeController.Instance.Speed < 0 ? 0 : -PbSpeed;
            TimeController.Instance.Play(ts);
            SetSoundSpeeds();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[0] = CurrentTime;
            keySliders[0].gameObject.SetActive(true);
            keySliders[0].value = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !Input.GetKey(KeyCode.LeftControl) && keys[0] >= 0)
        {
            SetTime(keys[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[1] = CurrentTime;
            keySliders[1].gameObject.SetActive(true);
            keySliders[1].value = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKey(KeyCode.LeftControl) && keys[1] >= 0)
        {
            SetTime(keys[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[2] = CurrentTime;
            keySliders[2].gameObject.SetActive(true);
            keySliders[2].value = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !Input.GetKey(KeyCode.LeftControl) && keys[2] >= 0)
        {
            SetTime(keys[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[3] = CurrentTime;
            keySliders[3].gameObject.SetActive(true);
            keySliders[3].value = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !Input.GetKey(KeyCode.LeftControl) && keys[3] >= 0)
        {
            SetTime(keys[3]);
        }

        timeSlider.value = CurrentTime;
    }

    private void SetSoundSpeeds()
    {
        foreach(var a in audioSources)
        {
            a.pitch = Speed;
        }
        if (Speed == 0)
        {
            StartCoroutine(fadeMusic(musicAmbiendSource, musicTimeSource));
            musicAmbiendSource.pitch = 1;
        }
        else
        {
            StartCoroutine(fadeMusic(musicTimeSource, musicAmbiendSource));
        }
    }

    private IEnumerator fadeMusic(AudioSource fadeIn, AudioSource fadeOut)
    {
        while (fadeIn.volume < volume || fadeOut.volume > 0)
        {
            fadeIn.volume += fadeStep;
            fadeOut.volume -= fadeStep;
            yield return new WaitForEndOfFrame();
        }


    }

    // can be negative
    public void Play(float playbackspeed)
    {
        if ((CurrentTime >= MaxTime && playbackspeed > 0) || (CurrentTime <= MinTime && playbackspeed < 0))
        {
            return;
        }
        foreach(var a in animators)
        {
            a.SetFloat("speed", playbackspeed);
        }
        Speed = playbackspeed;
    }

    // time [0,1]
    public void SetTime(float time)
    {
        if (time > 1 || time < 0)
        {
            return;
        }
        foreach(var a in animators)
        {
            //a.SetFloat("time", time);
            a.Play(0, 0, time);
        }
        PlayTimetravelSound();
    }

    void PlayTimetravelSound()
    {
        GameObject soundObject = Instantiate(timeTravelSoundObject);
        Destroy(soundObject, 2F);
    }
}
