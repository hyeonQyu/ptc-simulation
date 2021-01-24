using Nextwin.Client.Util;
using Nextwin.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioClip
{
    Call1,
    Call2,

    DontCall,

    TodayWhy,
    Broken,
    Why,
    Forgot,
    NoOffWork,

    Omg,

    Hey,
    Pardon,
    Busy,
    Ok,

    Which,
    NoRice,
    Sorry,

    SelectAgain,
    Which2,
    
    Happy,
    Finish,

    Order,
    Wtf,

    Good,

    Success,
    Fail,
    Fist,
    Paper
}

public enum EAudioSource
{
    Director,
    GirlFriend,
    Leader1,
    Leader2,
    Member,
    Player,
    Paper1,
    Paper2,
    Paper3
}

[Serializable]
public class AudioSourceDictionary:SerializableDictionary<EAudioSource, AudioSource> { }

public class AudioManager : Singleton<AudioManager>
{
    protected Dictionary<EAudioClip, AudioClip> _audioClips = new Dictionary<EAudioClip, AudioClip>();
    [SerializeField, Header("Key: AudioSource layer / Value: AudioSource object")]
    protected AudioSourceDictionary _audioSources;

    protected override void Awake()
    {
        base.Awake();
        LoadAudioClips();
    }

    private void Start()
    {
        CheckAudioSourcesAssinged();
    }

    /// <summary>
    /// 특정 오디오 소스를 통해 오디오 재생
    /// </summary>
    /// <param name="auidoClipName">재생하려는 오디오 클립 이름</param>
    /// <param name="audioSourceKey">오디오 클립이 재생될 오디오소스 레이어 이름</param>
    public virtual void PlayAudio(EAudioClip auidoClipName, EAudioSource audioSourceKey)
    {
        AudioSource source = _audioSources[audioSourceKey];
        source.clip = _audioClips[auidoClipName];
        source.Play();
    }

    /// <summary>
    /// 모든 오디오 일시정지
    /// </summary>
    public virtual void PauseAll()
    {
        foreach(KeyValuePair<EAudioSource, AudioSource> item in _audioSources)
        {
            item.Value.Pause();
        }
    }

    /// <summary>
    /// 특정 오디오소스 레이어 일시정지
    /// </summary>
    /// <param name="audioSourceKey"></param>
    public virtual void Pause(EAudioSource audioSourceKey)
    {
        _audioSources[audioSourceKey].Pause();
    }

    /// <summary>
    /// 모든 오디오 재생
    /// </summary>
    public virtual void ResumeAll()
    {
        foreach(KeyValuePair<EAudioSource, AudioSource> item in _audioSources)
        {
            item.Value.UnPause();
        }
    }

    /// <summary>
    /// 모든 오디오소스 레이어 재생
    /// </summary>
    /// <param name="audioSourceKey"></param>
    public virtual void Resume(EAudioSource audioSourceKey)
    {
        _audioSources[audioSourceKey].UnPause();
    }

    public virtual float GetAudioClipLength(EAudioClip audioClipKey)
    {
        return _audioClips[audioClipKey].length;
    }

    public virtual float GetAudioSourcePitch(EAudioSource audioSourceKey)
    {
        return _audioSources[audioSourceKey].pitch;
    }

    protected virtual void CheckAudioSourcesAssinged()
    {
        if(_audioSources.Count == 0)
        {
            Debug.LogError("Assign AudioSource.");
        }

        foreach(KeyValuePair<EAudioSource, AudioSource> item in _audioSources)
        {
            if(item.Value == null)
            {
                Debug.LogError($"[AudioManager Error] Add AudioSource corresponding to {item.Key}.");
            }
        }
    }

    protected virtual void LoadAudioClips()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("");
        foreach(AudioClip clip in clips)
        {
            _audioClips.Add(EnumConverter.ToEnum<EAudioClip>(clip.name), clip);
        }
    }
}
