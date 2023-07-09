using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip trashSound;

    /// <summary>
    /// Plays the provided AudioClip on the audioPlayer
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        audioPlayer.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays the provided AudioClip on the audioPlayer with the specified volume
    /// </summary>
    public void PlaySound(AudioClip clip, float volumeScale)
    {
        audioPlayer.PlayOneShot(clip, volumeScale);
    }
}
