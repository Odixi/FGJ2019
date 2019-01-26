using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishLobby : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ended"))
        {
            SceneManager.LoadScene("TestScene");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.SetFloat("Speed", 1.0F);
    }
}
