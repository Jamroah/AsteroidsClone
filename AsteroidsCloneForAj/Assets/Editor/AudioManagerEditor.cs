using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    private AudioManager m_audioManager;

    void OnEnable()
    {
        m_audioManager = target as AudioManager;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("Add BGM", EditorStyles.toolbarButton))
        {
            AddBGM();
        }
        if (GUILayout.Button("Remove BGM", EditorStyles.toolbarButton))
        {
            RemoveBGM();
        }
        EditorGUILayout.EndHorizontal();

        DrawAudioTracks(m_audioManager.trackListBGM);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("Add SFX", EditorStyles.toolbarButton))
        {
            AddSFX();
        }
        if (GUILayout.Button("Remove SFX", EditorStyles.toolbarButton))
        {
            RemoveSFX();
        }
        EditorGUILayout.EndHorizontal();

        DrawAudioTracks(m_audioManager.trackListSFX);

        EditorGUILayout.Space();

        EditorUtility.SetDirty(m_audioManager);
    }

    private void DrawAudioTracks(List<AudioTrack> tracks)
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            bool remove = false;
            EditorGUILayout.BeginVertical(EditorStyles.largeLabel);
            EditorGUILayout.BeginHorizontal();
            tracks[i].key = EditorGUILayout.TextField("Key", tracks[i].key);
            if (GUILayout.Button("Remove", EditorStyles.miniButton))
            {
                remove = true;
            }
            EditorGUILayout.EndHorizontal();          
            tracks[i].clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", tracks[i].clip, typeof(AudioClip), false);
            if (tracks == m_audioManager.trackListSFX)
            {
                tracks[i].trackType = TRACK_TYPE.SFX;
            }
            else
            {
                tracks[i].trackType = (TRACK_TYPE)EditorGUILayout.EnumPopup("Track Type", tracks[i].trackType);
            }
            EditorGUILayout.EndVertical();

            if (remove)
            {
                tracks.RemoveAt(i);
            }
        }   
    }

    private void AddBGM()
    {
        m_audioManager.trackListBGM.Add(new AudioTrack());
    }
    private void RemoveBGM()
    {
        try
        {
            m_audioManager.trackListBGM.RemoveAt(m_audioManager.trackListBGM.Count - 1);
        }
        catch
        {

        }
    }

    private void AddSFX()
    {
        m_audioManager.trackListSFX.Add(new AudioTrack());
    }
    private void RemoveSFX()
    {
        try
        {
            m_audioManager.trackListSFX.RemoveAt(m_audioManager.trackListSFX.Count - 1);
        }
        catch
        {

        }
    }
}
