using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicScript : MonoBehaviour
{
    public AudioSource BackgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        if (!BackgroundMusic.isPlaying)
        {
            BackgroundMusic.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
