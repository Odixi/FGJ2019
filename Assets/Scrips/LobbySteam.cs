using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySteam : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private LobbySteam forceScript;

    private void Start()
    {
        forceScript = gameObject.GetComponent<LobbySteam>();
    }

    void Update()
    {
        if (Time.time > 2)
        {
            particleSystem.emissionRate = 0;
            forceScript.enabled = false;
        } else
        {
            particleSystem.emissionRate = 5;
            forceScript.enabled = true;
        }
    }
}
