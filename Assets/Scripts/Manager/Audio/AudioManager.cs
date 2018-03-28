using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;


public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
        base.gameObject.AddComponent<AudioListener>();
        base.gameObject.name = "a_AudioManager";

        //初始化
        m_MusicVolume = 1f;

        m_SfxVolume = 1f;

        m_SfxControllers = new List<AudioController>();

        m_SfxControllerByName = new Dictionary<string, List<AudioController>>();

        m_MusicControllers = new List<AudioController>();

        m_MusicControllerByName = new Dictionary<string, List<AudioController>>();

        m_SfxAudioNamePlayedThisFrame = new List<string>();

        m_MusicAudioNamePlayerThisFrame = new List<string>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        for(int i = 0;i< m_SfxControllers.Count;i++)
        {
            if(m_SfxControllers[i] != null)
            {
                m_SfxControllers[i].Update();
            }
        }
        for(int i = 0;i < m_MusicControllers.Count;i++)
        {
            if(m_MusicControllers[i] != null)
            {
                m_MusicControllers[i].Update();
            }
        }
    }

    private void LateUpdate()
    {
        m_MusicAudioNamePlayerThisFrame.Clear();
        m_SfxAudioNamePlayedThisFrame.Clear();
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
        if (this.m_SfxAudioNamePlayedThisFrame.Contains(audioName))
        {
            return null;
        }
        this.m_SfxAudioNamePlayedThisFrame.Add(audioName);
        AudioController audioController = this._CreateAudioControlelr(audioName, loop);
        audioController.complete += this._HandleSfxAudioControllerCompleted;
        this.m_SfxControllers.Add(audioController);
        audioController.MasterVolume = this.sfxVolume;
        audioController.Muted = this.sfxMuted;
        this._PlayClip(audioController, audioName, volume, delay, pitch);
        if (!this.m_SfxControllerByName.ContainsKey(audioName))
        {
            this.m_SfxControllerByName.Add(audioName, new List<AudioController>());
        }
        this.m_SfxControllerByName[audioName].Add(audioController);
        return audioController;
    }

    public void StopSfx(string audioName = null)
    {
        if(audioName != null)
        {
            if(this.m_SfxControllerByName.ContainsKey(audioName))
            {
                foreach(var audioControlelr in this.m_SfxControllerByName[audioName])
                {
                    if(audioControlelr != null)
                    {
                        audioControlelr.complete -= this._HandleSfxAudioControllerCompleted;
                        audioControlelr.Stop();
                        this.m_SfxControllers.Remove(audioControlelr);
                    }
                }
                this.m_SfxControllerByName[audioName].Clear();
            }
        }
        else
        {
            StopAllSfx();
        }
    }

    public void StopAllSfx()
    {
        foreach(var audioController in this.m_SfxControllers)
        {
            if(audioController != null)
            {
                audioController.complete -= this._HandleSfxAudioControllerCompleted;
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

    public AudioController PlayMusic(string audioName, float volume = 1f, float delay = 0f, bool loop = true)
    {
        if (this.m_MusicAudioNamePlayerThisFrame.Contains(audioName))
        {
            return null;
        }
        this.m_MusicAudioNamePlayerThisFrame.Add(audioName);
        AudioController audioController = this._CreateAudioControlelr(audioName, loop);
        audioController.complete += this._HandleMusicAudioControllerCompleted;
        this.m_MusicControllers.Add(audioController);
        audioController.MasterVolume = this.musicVolume;
        audioController.Muted = this.musicMuted;
        this._PlayClip(audioController, audioName, volume, delay, 1f);
        if (!this.m_MusicControllerByName.ContainsKey(audioName))
        {
            this.m_MusicControllerByName.Add(audioName, new List<AudioController>());
        }
        this.m_MusicControllerByName[audioName].Add(audioController);
        return audioController;
    }

    public void StopMusic(string audioName = null)
    {
        if(audioName != null)
        {
            if(this.m_MusicControllerByName.ContainsKey(audioName))
            {
                foreach(var audioController in this.m_MusicControllerByName[audioName])
                {
                    if(audioController != null)
                    {
                        audioController.complete -= this._HandleMusicAudioControllerCompleted;
                        audioController.Stop();
                        this.m_MusicControllers.Remove(audioController);
                    }
                }
            }
            this.m_MusicControllerByName[audioName].Clear();
        }
        else
        {
            StopAllMusic();
        }
    }

    public void StopAllMusic()
    {
        foreach (var audioController in this.m_MusicControllers)
        {
            if (audioController != null)
            {
                audioController.complete -= this._HandleMusicAudioControllerCompleted;
                audioController.Stop();
            }
        }
        this.m_MusicControllers.Clear();
    }

    public void PauseAllMusic()
    {
        foreach(var audioController in this.m_MusicControllers)
        {
            if(audioController != null)
            {
                audioController.Pause();
            }
        }
    }

    public void ResumeAllMusic()
    {
        foreach(var audioController in this.m_MusicControllers)
        {
            if(audioController != null)
            {
                audioController.Resume();
            }
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
        this.m_MusicVolume = value;
        if (m_MusicControllers.Count == 0)
            return;
        foreach(var audioController in this.m_MusicControllers)
        {
            audioController.MasterVolume = this.m_MusicVolume;
        }
    }

    //设置音效音量
    private void _SetSfxVolume(float value)
    {
        this.m_SfxVolume = value;
        foreach(var audioControlelr in this.m_SfxControllers)
        {
            audioControlelr.MasterVolume = this.m_SfxVolume;
        }
    }

    //设置音乐静音或不静音
    private void _SetMusicMuted(bool value)
    {
        this.m_MusicMuted = value;
        foreach(var audioController in this.m_MusicControllers)
        {
            audioController.Muted = this.m_MusicMuted;
        }

    }

    //设置音效静音或不静音
    private void _SetSfxMuted(bool value)
    {
        this.m_SfxMuted = value;
        foreach(var audioController in this.m_SfxControllers)
        {
            audioController.Muted = this.m_SfxMuted;
        }
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

    private void _HandleSfxAudioControllerCompleted(AudioController audioController)
    {
        audioController.complete -= this._HandleSfxAudioControllerCompleted;
        if(this.m_SfxControllers.Contains(audioController))
        {
            this.m_SfxControllers.Remove(audioController);
        }
        if(this.m_SfxControllerByName.ContainsKey(audioController.audioName))
        {
            this.m_SfxControllerByName[audioController.audioName].Remove(audioController);
        }
    }

    private void _HandleMusicAudioControllerCompleted(AudioController audioController)
    {
        audioController.complete -= this._HandleMusicAudioControllerCompleted;
        if (this.m_MusicControllers.Contains(audioController))
        {
            this.m_MusicControllers.Remove(audioController);
        }
        if (this.m_MusicControllerByName.ContainsKey(audioController.audioName))
        {
            this.m_MusicControllerByName[audioController.audioName].Remove(audioController);
        }
    }

    private void _PlayClip(AudioController audioController,string audioName,float volume = 1f,float delay = 0f,float pitch = 1f)
    {
        AudioClip clip = (AudioClip)Singleton<ResourceManager>.instance.GetRawObjectByName(audioName);
        audioController.PlayClip(clip, volume, delay, pitch);
    }

    private float m_MusicVolume;

    private float m_SfxVolume;

    private bool m_MusicMuted;

    private bool m_SfxMuted;

    private Tweener m_MusicVolumeTweener;

    private List<AudioController> m_SfxControllers;

    private Dictionary<string, List<AudioController>> m_SfxControllerByName;

    private List<AudioController> m_MusicControllers;

    private Dictionary<string, List<AudioController>> m_MusicControllerByName;

    private List<string> m_SfxAudioNamePlayedThisFrame;

    private List<string> m_MusicAudioNamePlayerThisFrame;
}
