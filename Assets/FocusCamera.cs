using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FocusCamera : MonoBehaviour
{
    DepthOfField dof;
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        dof = GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
    }

    // Update is called once per frame
    void Update()
    {
        dof.focusDistance.value = Vector3.Distance(transform.position, player.transform.position);
    }
}
