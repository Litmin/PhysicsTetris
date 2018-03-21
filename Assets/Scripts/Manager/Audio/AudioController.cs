using System;
using UnityEngine;

public class AudioController : IUpdatable
{   
    public AudioController(string audioNameIn,GameObject audioObject,bool looping = false)
    {
        this.audioName = audioNameIn;
        this.m_AudioSource = audioObject.AddComponent<AudioSource>();
        this.m_AudioSource.loop = looping;
        this.m_AudioSource.playOnAwake = false;
    }

    public void Update()
    {
        if(!this.m_AudioSource.isPlaying && !this.m_Paused)
        {
            this._Complete();
        }
    }

    public void PlayClip(AudioClip clip,float volume = 1f, float delay = 0f, float pitch = 1f)
    {
        this.m_AudioSource.clip = clip;
        this.m_AudioSource.pitch = pitch;
        this.m_AudioSource.PlayDelayed(delay);
        this.m_Volume = volume;
        this._ChangeVolume();
    }

    public void Resume()
    {
        if(this.m_Paused)
        {
            this.m_Paused = false;
            this.m_AudioSource.UnPause();
        }
    }

    public void Pause()
    {
        if(!this.m_Paused)
        {
            this.m_Paused = true;
            this.m_AudioSource.Pause();
        }
    }

    public void Stop()
    {
        if(this.m_AudioSource != null)
        {
            this.m_AudioSource.Stop();
        }
        this._Complete();
    }
    
    private void _Complete()
    {
        this._OnComplete();
        UnityEngine.Object.Destroy(this.m_AudioSource.gameObject);
    }

    private void _OnComplete()
    {
        if(this.complete != null)
        {
            this.complete(this);
        }
    }

    private void _SetVolume(float value)
    {
        this.m_Volume = value;
        this._ChangeVolume();
    }

    private void _SetMasterVolume(float value)
    {
        this.m_MasterVolume = value;
        this._ChangeVolume();
    }

    private void _SetMuted(bool value)
    {
        this.m_Muted = value ? 0 : 1;
        this._ChangeVolume();
    }

    private void _ChangeVolume()
    {
        this.m_AudioSource.volume = this.m_Volume * this.m_MasterVolume * (float)this.m_Muted;
    }


    public event Action<AudioController> complete;

    public string audioName { get; set; }

    public float Volume
    {
        get
        {
            return this.m_Volume;
        }
        set
        {
            this._SetVolume(value);
        }
    }

    public float MasterVolume
    {
        get
        {
            return this.m_MasterVolume;
        }
        set
        {
            this._SetMasterVolume(value);
        }
    }

    public bool Muted
    {
        get
        {
            return this.m_Muted == 0;
        }
        set
        {
            this._SetMuted(value);
        }
    }

    private bool m_Paused;

    private AudioSource m_AudioSource;

    private float m_Volume = 1f;

    private float m_MasterVolume = 1f;

    private int m_Muted = 1;
}
