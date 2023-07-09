using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMusicHandler : MonoBehaviour
{

    private AudioSource bgMusic;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        bgMusic = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        if (!bgMusic.isPlaying)
        {
            bgMusic.Play();
        }
    }

    public void StopMusicAndDestroy()
    {
        Destroy(transform.gameObject);
    }
}
