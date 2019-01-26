using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishKitchen : MonoBehaviour
{

    public Animator animator;

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ended"))
        {
            SceneManager.LoadScene("TestScene");
        }
    }
}
