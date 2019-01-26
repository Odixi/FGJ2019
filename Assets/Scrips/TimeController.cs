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
    public Text timeValueText;

    private void Start()
    {
        if (Instance != null)
        {
            print("TimeController already exist!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        keys = new List<float> {-1,-1,-1,-1};
    }

    private void Update()
    {
        CurrentTime = animators[0].GetCurrentAnimatorStateInfo(0).normalizedTime;

        if ((CurrentTime >= MaxTime && Speed > 0) || (CurrentTime <= MinTime && Speed < 0))
        {
            Speed = 0;
            foreach (var a in animators)
            {
                Speed = 0;
                a.SetFloat("speed", 0);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f)
        {
            PbSpeed += 0.1f;
            timeValueText.text = PbSpeed.ToString();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < -0.01f)
        {
            PbSpeed -= PbSpeed <= 0.1f ? 0 : 0.1f;
            timeValueText.text = PbSpeed.ToString();
        }

        // Time Inputs
        if (Input.GetKeyDown(KeyCode.E))
        {
            var ts = TimeController.Instance.Speed > 0 ? 0 :PbSpeed;
            TimeController.Instance.Play(ts);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var ts = TimeController.Instance.Speed < 0 ? 0 : -PbSpeed;
            TimeController.Instance.Play(ts);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[0] = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKey(KeyCode.LeftControl) && keys[0] >= 0)
        {
            SetTime(keys[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[1] = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !Input.GetKey(KeyCode.LeftControl) && keys[1] >= 0)
        {
            SetTime(keys[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[2] = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !Input.GetKey(KeyCode.LeftControl) && keys[2] >= 0)
        {
            SetTime(keys[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKey(KeyCode.LeftControl))
        {
            keys[3] = CurrentTime;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !Input.GetKey(KeyCode.LeftControl) && keys[3] >= 0)
        {
            SetTime(keys[3]);
        }

        timeSlider.value = CurrentTime;
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
        foreach(var a in animators)
        {
            //a.SetFloat("time", time);
            a.Play(0, 0, time);
        }
    }

}
