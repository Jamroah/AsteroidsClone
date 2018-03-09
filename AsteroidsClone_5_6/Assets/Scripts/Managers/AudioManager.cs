// Copyright (C) 2015 Ben Beagley //

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum TRACK_TYPE
{
    BGM,
    AFX,
    SFX
}

public enum TRANSITION
{
    INSTANT,
    FADE
}

[System.Serializable]
public class AudioTrack
{
    public string key;
    public AudioClip clip;
    public TRACK_TYPE trackType;
    //public float fadeDuration;

    public AudioTrack() { }

    public AudioTrack(string name, AudioClip audioclip, TRACK_TYPE type)
    {
        key = name;
        clip = audioclip;
        trackType = type;
    }
}

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioTrack> trackListBGM = new List<AudioTrack>();
    public List<AudioTrack> trackListSFX = new List<AudioTrack>();

    private AudioSource m_BGM, m_tmpBGM;
    private AudioSource m_AFX;
    private AudioSource m_SFX;

    private static AudioClip m_fadeBGM;
    private static AudioClip m_fadeAFX;

    private float m_masterVolume = 1f;
    private float m_bgmVolume = 1f;
    private float m_sfxVolume = 0.5f;

    private float fadeDuration = 1f;

    private Dictionary<string, AudioTrack> m_trackListBGM = new Dictionary<string, AudioTrack>();
    private Dictionary<string, AudioTrack> m_trackListSFX = new Dictionary<string, AudioTrack>();

    void Start()
    {
        m_BGM = new GameObject("BGM").AddOrGetComponent<AudioSource>();
        m_AFX = new GameObject("AFX").AddOrGetComponent<AudioSource>();
        m_SFX = new GameObject("SFX").AddOrGetComponent<AudioSource>();

        m_BGM.loop = true;
        m_AFX.loop = true;

        FillDictionaries();
    }

    /// <summary>
    /// Sets the master volume for bgm, afx and sfx.
    /// </summary>
    /// <param name="volume">float value between 0 - 1</param>
    public static void SetMasterVolume(float volume)
    {
        if(volume > 1)
        {
            volume = 1;
        }

        if (volume < 0)
        {
            volume = 0;
        }

        Instance.m_masterVolume = volume;
        Instance.m_BGM.volume = Instance.m_bgmVolume * volume;
        Instance.m_AFX.volume = Instance.m_sfxVolume * volume;
        Instance.m_SFX.volume = Instance.m_sfxVolume * volume;
    }

    /// <summary>
    /// Sets the volume for bgm.
    /// </summary>
    /// <param name="volume">float value between 0 - 1</param>
    public static void SetBGMVolume(float volume)
    {
        if (volume > 1)
        {
            volume = 1;
        }

        if (volume < 0)
        {
            volume = 0;
        }

        Instance.m_bgmVolume = volume;
        Instance.m_BGM.volume = volume * Instance.m_masterVolume;
    }

    /// <summary>
    /// Sets the volume sfx.
    /// </summary>
    /// <param name="volume">float value between 0 - 1</param>
    public static void SetSFXVolume(float volume)
    {
        if (volume > 1)
        {
            volume = 1;
        }

        if (volume < 0)
        {
            volume = 0;
        }

        Instance.m_sfxVolume = volume;
        Instance.m_SFX.volume = volume * Instance.m_masterVolume;
        Instance.m_AFX.volume = volume * Instance.m_masterVolume;
    }

    public static void SetMute(bool mute)
    {
        Instance.m_AFX.mute = mute;
        Instance.m_BGM.mute = mute;
        Instance.m_SFX.mute = mute;
    }

    public static void PlayBGM(string track, TRANSITION transition)
    {
        //Check if the audio track actually exists
        if (!Instance.m_trackListBGM.ContainsKey(track))
        {
            Debug.LogWarning("Audio Track does not exist.");
            return;
        }

        if (Instance.m_BGM.clip == Instance.m_trackListBGM[track].clip)
        {
            Debug.LogWarning("BGM is already playing.");
            return;
        }

        Instance.StopCoroutine("FadeInBGM");
        Instance.StopCoroutine("FadeOutBGM");
        Instance.StopCoroutine("CrossFadeBGM");

        switch (transition)
        {
            case TRANSITION.INSTANT:
                Instance.m_BGM.PlayClip(Instance.m_trackListBGM[track].clip);
                break;
            case TRANSITION.FADE:
                m_fadeBGM = Instance.m_trackListBGM[track].clip;

                Instance.m_tmpBGM = new GameObject("BGM").AddOrGetComponent<AudioSource>();
                Instance.m_tmpBGM.PlayClip(Instance.m_trackListBGM[track].clip);
                
                Instance.StartCoroutine("CrossFadeBGM");
                break;
            default:
                break;
        }
    }

    public static void PlayAFX(string track, TRANSITION transition)
    {
        //Check if the audio track actually exists
        if (!Instance.m_trackListBGM.ContainsKey(track))
        {
            Debug.LogWarning("Audio Track does not exist.");
            return;
        }

        //if (Instance.m_AFX.clip == Instance.m_trackListBGM[track].clip)
        //{
        //    Debug.LogWarning("AFX is already playing.");
        //    return;
        //}

        Instance.StopCoroutine("FadeInAFX");
        Instance.StopCoroutine("FadeOutAFX");

        switch (transition)
        {
            case TRANSITION.INSTANT:
                Instance.m_AFX.PlayClip(Instance.m_trackListBGM[track].clip);
                break;
            case TRANSITION.FADE:
                m_fadeAFX = Instance.m_trackListBGM[track].clip;
                Instance.m_AFX.PlayClip(Instance.m_trackListBGM[track].clip);
                Instance.StartCoroutine("FadeInAFX");
                break;
            default:
                break;
        }

        Instance.m_AFX.timeSamples = UnityEngine.Random.Range(0, Instance.m_trackListBGM[track].clip.samples);
    }

    public static void StopBGM(TRANSITION transition)
    {
        Instance.StopCoroutine("FadeInBGM");
        Instance.StopCoroutine("FadeOutBGM");
        Instance.StopCoroutine("CrossFadeBGM");

        switch (transition)
        {
            case TRANSITION.INSTANT:
                Instance.m_BGM.Stop();
                Instance.m_BGM.clip = null;
                break;
            case TRANSITION.FADE:
                Instance.StartCoroutine("FadeOutBGM");
                break;
            default:
                break;
        }
    }

    public static void StopAFX(TRANSITION transition)
    {
        Instance.StopCoroutine("FadeInAFX");
        Instance.StopCoroutine("FadeOutAFX");

        switch (transition)
        {
            case TRANSITION.INSTANT:
                Instance.m_AFX.Stop();
                Instance.m_AFX.clip = null;
                break;
            case TRANSITION.FADE:
                Instance.StartCoroutine("FadeOutAFX");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Plays an SFX
    /// </summary>
    /// <param name="key">The key set in the inspector</param>
    public static void PlaySFX(string key)
    {
        if (Instance != null && !Instance.m_trackListSFX.ContainsKey(key))
        {
            Debug.LogWarning("Audio Track does not exist.");
            return;
        }

        if (Instance != null)
        {
            Instance.m_SFX.pitch = 1;
            Instance.m_SFX.PlayOneShot(Instance.m_trackListSFX[key].clip, Instance.m_sfxVolume * Instance.m_masterVolume);
            Debug.Log("Played " + key);
        }
    }

    /// <summary>
    /// Plays an SFX with variance in pitch
    /// </summary>
    /// <param name="key">The key set in the inspector</param>
    public static void PlaySFX(string key, float minPitch, float maxPitch)
    {
        if (Instance != null && !Instance.m_trackListSFX.ContainsKey(key))
        {
            Debug.LogWarning("Audio Track does not exist.");
            return;
        }

        if(minPitch > maxPitch)
        {
            Debug.Log("Minimum pitch must be less than maximum pitch.");
            maxPitch = minPitch;
        }

        if (Instance != null)
        {
            Instance.m_SFX.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            Instance.m_SFX.PlayOneShot(Instance.m_trackListSFX[key].clip, Instance.m_sfxVolume * Instance.m_masterVolume);
            Debug.Log("Played " + key);
        }
    }

    public static void StopAll(TRANSITION transition)
    {
        StopBGM(transition);
        StopAFX(transition);
    }

    private IEnumerator CrossFadeBGM()
    {
        //Fade in
        StartCoroutine(FadeInBGM(m_tmpBGM));
        yield return null;
        //Fade out
        yield return StartCoroutine(FadeOutBGM());

        yield return null;

        m_BGM = m_tmpBGM;
        m_tmpBGM = null;
    }


    private IEnumerator FadeInBGM()
    {
        yield return StartCoroutine(FadeInBGM(m_BGM));
    }

    private IEnumerator FadeInBGM(AudioSource source)
    {
        float targetVolume = (1 * m_bgmVolume * m_masterVolume);

        if (fadeDuration <= 0)
        {
            source.volume = targetVolume;
        }
        else
        {
            source.volume = 0;
        }

        while (source.volume < targetVolume)
        {
            source.volume += (Time.deltaTime / fadeDuration) * m_bgmVolume * m_masterVolume;
            yield return null;
        }

        source.volume = targetVolume;
    }

    private IEnumerator FadeOutBGM()
    {
        while (m_BGM.volume > 0)
        {
            m_BGM.volume -= (Time.deltaTime / fadeDuration) * m_bgmVolume * m_masterVolume;
            yield return null;
        }

        m_BGM.clip = null;
    }

    private IEnumerator FadeInAFX()
    {
        float targetVolume = (1 * m_sfxVolume * m_masterVolume);

        if (fadeDuration <= 0)
        {
            m_AFX.volume = targetVolume;
        }
        else
        {
            m_AFX.volume = 0;
        }

        while (m_AFX.volume < targetVolume)
        {
            m_AFX.volume += (Time.deltaTime / fadeDuration) * m_sfxVolume * m_masterVolume;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator FadeOutAFX()
    {
        while (m_AFX.volume > 0)
        {
            m_AFX.volume -= (Time.deltaTime / fadeDuration) * m_sfxVolume * m_masterVolume;
            yield return null;
        }

        m_AFX.clip = null;
    }

    private void FillDictionaries()
    {
        foreach (AudioTrack track in trackListBGM)
        {
            if (track.key == null || track.key == "" || track.clip == null)
            {
                Debug.Log("BGM track is invalid. Skipping.");
                continue;
            }

            if(track.trackType == TRACK_TYPE.SFX)
            {
                Debug.Log(track.key + " wrongfully assigned as SFX. Defaulting to BGM.");
                track.trackType = TRACK_TYPE.BGM;
            }

            try
            {
                m_trackListBGM.Add(track.key, track);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        foreach (AudioTrack track in trackListSFX)
        {
            if (track.key == null || track.key == "" || track.clip == null)
            {
                Debug.Log("SFX track is invalid. Skipping.");
                continue;
            }

            try
            {
                m_trackListSFX.Add(track.key, track);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    #region old
    /// <summary>
    /// Plays a BGM
    /// </summary>
    /// <param name="key">The key set in the inspector</param>
    //public static void PlayBGM(string key)
    //{
    //    //Check if the audio track actually exists
    //    if (!Instance.m_trackListBGM.ContainsKey(key))
    //    {
    //        Debug.LogWarning("Audio Track does not exist.");
    //        return;
    //    }

    //    if (Instance.m_BGM.clip == Instance.m_trackListBGM[key].clip)
    //    {
    //        Debug.LogWarning("BGM is already playing.");
    //        return;
    //    }

    //    //If the BGM is playing something already we fade it out
    //    if (Instance.m_BGM.isPlaying)
    //    {
    //        Instance.StartCoroutine(Instance.FadeOut(Instance.m_BGM, () => { PlayBGM(key); }));
    //        return;
    //    }

    //    Instance.StartCoroutine(Instance.FadeIn(Instance.m_trackListBGM[key]));

    //    Debug.Log("Played " + key);
    //}

    /// <summary>
    /// plays an AFX
    /// </summary>
    /// <param name="key">The key set in the inspector</param>
    //public static void PlayAFX(string key)
    //{
    //    //Check if the audio track actually exists
    //    if (!Instance.m_trackListBGM.ContainsKey(key))
    //    {
    //        Debug.LogWarning("Audio Track does not exist.");
    //        return;
    //    }

    //    if (Instance.m_AFX.clip == Instance.m_trackListBGM[key].clip)
    //    {
    //        Debug.LogWarning("AFX is already playing.");
    //        return;
    //    }

    //    //If the AFX source is playing something already we fade it out
    //    if (Instance.m_AFX.isPlaying)
    //    {
    //        Instance.StartCoroutine(Instance.FadeOut(Instance.m_AFX, () => { PlayAFX(key); }));
    //        return;
    //    }

    //    Instance.StartCoroutine(Instance.FadeIn(Instance.m_trackListBGM[key]));

    //    Debug.Log("Played " + key);
    //}

    /// <summary>
    /// Stops the current BGM.
    /// </summary>
    /// <param name="immediate">If false fades out</param>
    //public static void StopBGM(bool immediate = false)
    //{
    //    if (Instance.m_BGM.clip != null)
    //    {
    //        if (immediate)
    //        {
    //            //AudioManager.Instance.m_BGM.Stop();
    //            //AudioManager.Instance.m_BGM.clip = null;
    //        }
    //        else
    //        {
    //            Instance.StartCoroutine(Instance.FadeOut(Instance.m_BGM));
    //        }
    //    } 
    //}

    /// <summary>
    /// Stops the current AFX.
    /// </summary>
    /// <param name="immediate">If false fades out</param>
    //public static void StopAFX(bool immediate = false)
    //{
    //    if (Instance.m_AFX.isPlaying)
    //    {
    //        if (immediate)
    //        {
    //            Instance.m_AFX.Stop();
    //            //AudioManager.Instance.m_AFX.clip = null;
    //        }
    //        else
    //        {
    //            Instance.StartCoroutine(Instance.FadeOut(Instance.m_AFX));
    //        }
    //    } 
    //}

    //public static void StopAll(bool immediate = false)
    //{
    //    if(immediate)
    //    {
    //        Instance.m_BGM.Stop();
    //        Instance.m_AFX.Stop();
    //        return;
    //    }

    //    Instance.StartCoroutine(Instance.FadeOut(Instance.m_BGM));
    //    Instance.StartCoroutine(Instance.FadeOut(Instance.m_AFX));
    //}

    ///// <summary>
    ///// Stops the current SFX.
    ///// </summary>
    //public static void StopSFX()
    //{
    //    if (AudioManager.Instance.m_SFX.isPlaying)
    //    {
    //        AudioManager.Instance.m_SFX.Stop();
    //    }

    //    AudioManager.Instance.m_SFX.clip = null;
    //}

    //private IEnumerator FadeIn(AudioTrack track)
    //{     
    //    AudioSource source = null;
    //    float targetVolume = 0.0f;

    //    if (track.trackType == TRACK_TYPE.SFX)
    //    {
    //        Debug.Log("Wrong AudioSource. Breaking out.");
    //        yield break;
    //    }

    //    switch (track.trackType)
    //    {
    //        case TRACK_TYPE.BGM:
    //            source = m_BGM;
    //            targetVolume = (1 * m_bgmVolume * m_masterVolume);
    //            break;
    //        case TRACK_TYPE.AFX:
    //            source = m_AFX;
    //            targetVolume = (1 * m_sfxVolume * m_masterVolume);
    //            break;
    //    }

    //    Debug.Log("Started Fade In of " + track.key);

    //    source.PlayClip(track.clip);

    //    if (fadeDuration <= 0)
    //    {
    //        source.volume = targetVolume;
    //    }
    //    else
    //    {
    //        source.volume = 0;
    //    }

    //    while (source.volume < targetVolume)
    //    {
    //        source.volume += (Time.deltaTime / fadeDuration) * m_bgmVolume * m_masterVolume;
    //        yield return null;
    //    }

    //    source.volume = targetVolume;

    //    Debug.Log("Completed Fade In of " + track.key + ". Source Volume is " + source.volume);
    //}

    //private IEnumerator FadeOut(AudioSource source, Action OnComplete = null)
    //{
    //    if(source == m_SFX)
    //    {
    //        Debug.Log("Wrong AudioSource. Breaking out.");
    //        yield break;
    //    }

    //    Debug.Log("Started Fade Out of " + source.name);

    //    float targetVolume = 0.0f;

    //    if (source.name == "AFX")
    //    {
    //        targetVolume = m_sfxVolume;
    //    }
    //    else
    //    {
    //        targetVolume = m_bgmVolume;
    //    }

    //    while (source.volume > 0)
    //    {
    //        source.volume -= (Time.deltaTime / fadeDuration) * targetVolume * m_masterVolume;
    //        yield return null;
    //    }

    //    source.clip = null;
    //    source.volume = 1;
    //    Debug.Log("Completed Fade Out of " + source.name);

    //    if(OnComplete != null)
    //    {
    //        OnComplete();
    //    }
    //}
    #endregion
}
