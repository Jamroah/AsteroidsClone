using UnityEngine;
using System.Collections;

public static class AudioSourceExtensions
{
    /// <summary>
    /// Plays an audio clip
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clip"></param>
    public static void PlayClip(this AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Plays a random clip from an array of clips
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clips"></param>
    public static void PlayRandomClip(this AudioSource audioSource, AudioClip[] clips)
    {
        int clipIndex = Random.Range(0, clips.Length);
        audioSource.PlayClip(clips[clipIndex]);
    }
}
