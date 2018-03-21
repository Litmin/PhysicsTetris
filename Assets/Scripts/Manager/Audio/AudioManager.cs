using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;


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
            return m_MusicVolume;
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
            return m_SfxVolume;
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
            return m_MusicMuted;
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
            return m_SfxMuted;
        }
        set
        {
            this._SetSfxMuted(value);
        }
    }

    //播放音效
    public AudioController PlaySfx(string audioName,float volume = 1f,float delay = 0f,float pitch = 1f,bool loop = false,bool dispatchEvent = false)
    {
        if(this.m_SfxAudioNamePlayedThisFrame.Contains(audioName))
        {
            return null;
        }
        this.m_SfxAudioNamePlayedThisFrame.Add(audioName);
        AudioController audioController = this._CreateAudioControlelr(audioName, loop);
        audioController.complete += this._HandleAudioControllerCompleted;
        this.m_SfxControllers.Add(audioController);
        audioController.MasterVolume = this.sfxVolume;
        audioController.Muted = this.sfxMuted;
        if(!this.m_SfxControllerByName.ContainsKey(audioName))
        {
            this.m_SfxControllerByName.Add(audioName, new List<AudioController>());
        }
        this.m_SfxControllerByName[audioName].Add(audioController);
        return audioController;
    }

    public void StopAllSfx()
    {
        foreach(var audioController in this.m_SfxControllers)
        {
            if(audioController != null)
            {
                audioController.complete -= this._HandleAudioControllerCompleted;
                audioController.Stop();
            }
        }
        this.m_SfxControllers.Clear();
    }

    public void PauseAllSfx()
    {
        foreach(var audioController in this.m_SfxControllers)
        {
            if(audioController != null)
            {
                audioController.Pause();
            }
        }
    }

    public void ResumeAllSfx()
    {
        foreach(var audioController in this.m_SfxControllers)
        {
            if(audioController != null)
            {
                audioController.Resume();
            }
        }
    }

    public void PlayMusic(string audioName,float volume = 1f,float delay = 0f,bool loop = true)
    {
        if(this.m_MusicControllers.ContainsKey(audioName))
        {

        }
    }
    
    //控制音乐播放音量的Tweener
    public void TweenMusicVolume(float targetVolume, float time)
    {
        this.KillMusicVolumeTween();
        this.m_MusicVolumeTweener = DOTween.To(delegate() { return this.m_MusicVolume; }, delegate (float x)
        {
            this.musicVolume = x;
        }, targetVolume, time).SetUpdate(true);
    }

    public void KillMusicVolumeTween()
    {
        if (this.m_MusicVolumeTweener != null && this.m_MusicVolumeTweener.IsActive())
        {
            this.m_MusicVolumeTweener.Kill(false);
        }
        this.m_MusicVolumeTweener = null;
    }

    public void PauseMusicVolumeTween()
    {
        if (this.m_MusicVolumeTweener != null)
        {
            this.m_MusicVolumeTweener.Pause<Tweener>();
        }
    }

    public void UnpauseMusicVolumeTween()
    {
        if (this.m_MusicVolumeTweener != null)
        {
            this.m_MusicVolumeTweener.Play<Tweener>();
        }
    }

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
        this.m_MusicMuted = value;

    }

    //设置音效静音或不静音
    private void _SetSfxMuted(bool value)
    {
        this.m_SfxMuted = value;
    }

    private AudioController _CreateAudioControlelr(string audioName,bool looping = false)
    {
        GameObject gameObject = new GameObject();
        if(audioName != null)
        {
            gameObject.name = audioName;
        }
        gameObject.transform.parent = base.gameObject.transform;
        return new AudioController(audioName, gameObject, looping);
    }

    private void _HandleAudioControllerCompleted(AudioController audioController)
    {
        audioController.complete -= this._HandleAudioControllerCompleted;
        if(this.m_SfxControllers.Contains(audioController))
        {
            this.m_SfxControllers.Remove(audioController);
        }
        if(this.m_SfxControllerByName.ContainsKey(audioController.audioName))
        {
            this.m_SfxControllerByName[audioController.audioName].Remove(audioController);
        }
    }

    private void _PlayClip(AudioController audioController,string audioName,float volume = 1f,float delay = 0f,float pitch = 1f)
    {
        AudioClip clip = (AudioClip)Singleton<ResourceManager>.instance.GetRawObjectByName(audioName);
        audioController.PlayClip(clip, volume, delay, pitch);
    }

    private float m_MusicVolume = 1f;

    private float m_SfxVolume = 1f;

    private bool m_MusicMuted;

    private bool m_SfxMuted;

    private Tweener m_MusicVolumeTweener;

    private List<AudioController> m_SfxControllers = new List<AudioController>();

    private Dictionary<string, List<AudioController>> m_SfxControllerByName = new Dictionary<string, List<AudioController>>();

    private List<AudioController> m_MusicControllers = new List<AudioController>();

    private Dictionary<string, List<AudioController>> m_MusicControllerByName = new Dictionary<string, List<AudioController>>();

    private List<string> m_SfxAudioNamePlayedThisFrame = new List<string>();

    private List<string> m_MusicAudioNamePlayerThisFrame = new List<string>();
}
