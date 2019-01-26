using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    public List<Sprite> images;
    public List<float> times;

    private Image im;

    private int index = 0;
    private float time = 0;

    private void Start()
    {
        im = GetComponentInChildren<Image>();
        im.sprite = images[index];
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > times[index])
        {
            index++;
            if (index >= times.Count)
            {
                SceneManager.LoadScene("Lobby");
                return;
            }
            im.sprite = images[index];
        }
    }

}
