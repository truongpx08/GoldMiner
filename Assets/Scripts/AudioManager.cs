using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EAudioClipName
{
    Block,
    Box,
    Hook,
    PullLine,
    Time,
}

public class AudioManager : TruongSingleton<AudioManager>
{
    [TitleGroup("Audio Sources")]
    [SerializeField] private AudioSource pullLineSource;
    [SerializeField] private AudioSource soundEffectSource;

    [TitleGroup("Audio Settings")]
    [SerializeField] private bool musicOn;
    [SerializeField] private bool soundEffectsOn;

    [TitleGroup("Audio Clips")]
    [SerializeField] private List<AudioClip> audioClipList;

    public void PlayPullLineClip()
    {
        var clip = audioClipList.Find(item => item.name == EAudioClipName.PullLine.ToString());
        if (clip != null) PlayPullLineClip(clip);
        else Debug.LogError($"{EAudioClipName.PullLine} not found");
    }

    public void PlaySoundEffect(EAudioClipName clipName)
    {
        var clip = audioClipList.Find(item => item.name == clipName.ToString());
        if (clip != null) PlayAudioClip(clip);
        else Debug.LogError($"{clipName} not found");
    }

    private void PlayPullLineClip(AudioClip clip)
    {
        if (musicOn)
        {
            pullLineSource.clip = clip;
            pullLineSource.loop = true;
            pullLineSource.Play();
        }
    }

    public void StopPullLine()
    {
        if (pullLineSource.isPlaying)
        {
            pullLineSource.Stop();
        }
    }


    private void PlayAudioClip(AudioClip clip)
    {
        if (soundEffectsOn)
        {
            if (soundEffectSource.isPlaying) soundEffectSource.Stop();
            soundEffectSource.PlayOneShot(clip);
        }
    }

    public void SetStatus(bool isOn)
    {
        this.musicOn = isOn;
        this.soundEffectsOn = isOn;
    }
}