using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishLobby : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.SetFloat("Speed", 1.0F);
    }
}
