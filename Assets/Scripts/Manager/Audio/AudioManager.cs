using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
        base.gameObject.AddComponent<AudioListener>();
    }
    private void Start()
    {

    }

    private void Update()
    {

    }

    public float musicVolume
    {
        get
        {
            return m_musicVolume;
        }
        set
        {
            this._SetMusicVolume(value);
        }
    }

    public float sfxVolume
    {
        get
        {
            return m_sfxVolume;
        }
        set
        {
            this._SetSfxVolume(value);
        }
    }
    //是否在播放音乐
    public bool musicPlaying
    {
        get
        {
            return _GetMusicPlaying();
        }
    }
    //音乐是否静音
    public bool musicMuted
    {
        get
        {
            return m_musicMuted;
        }
        set
        {
            this._SetMusicMuted(value);
        }
    }

    //音效是否静音
    public bool sfxMuted
    {
        get
        {
            return m_sfxMuted;
        }
        set
        {
            this._SetSfxMuted(value);
        }
    }

    //播放音效


    //设置背景音乐音量
    private void _SetMusicVolume(float value)
    {

    }
    //设置音效音量
    private void _SetSfxVolume(float value)
    {

    }
    //是否在播放音乐
    private bool _GetMusicPlaying()
    {
        return true;
    }
    //设置音乐静音或不静音
    private void _SetMusicMuted(bool value)
    {
        this.m_musicMuted = value;

    }

    //设置音效静音或不静音
    private void _SetSfxMuted(bool value)
    {
        this.m_sfxMuted = value;
    }

    private float m_musicVolume = 1f;

    private float m_sfxVolume = 1f;

    private bool m_musicMuted;

    private bool m_sfxMuted;
}
