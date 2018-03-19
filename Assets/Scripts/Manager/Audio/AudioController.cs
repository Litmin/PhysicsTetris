using System;
using UnityEngine;

public class AudioController : IUpdatable
{   
    public AudioController(string audioNameIn,GameObject audioObject,bool looping = false)
    {
        this.audioName = audioNameIn;
        this.m_audioSource = audioObject.AddComponent<AudioSource>();
        this.m_audioSource.loop = looping;
        this.m_audioSource.playOnAwake = false;
    }

    public void Update()
    {

    }

    public void PlayClip(AudioClip clip,float volume = 1f, float delay = 0f, float pitch = 1f)
    {
        this.m_audioSource.clip = clip;
        this.m_audioSource.pitch = pitch;
        this.m_audioSource.PlayDelayed(delay);
        this.m_volume = volume;
        this._ChangeVolume();
    }

    public void Resume()
    {
        if(this.m_paused)
        {
            this.m_paused = false;
            this.m_audioSource.UnPause();
        }
    }

    public void Pause()
    {
        if(!this.m_paused)
        {
            this.m_paused = true;
            this.m_audioSource.Pause();
        }
    }

    public void Stop()
    {
        if(this.m_audioSource != null)
        {
            this.m_audioSource.Stop();
        }
        this._Complete();
    }
    
    private void _Complete()
    {
        this._OnComplete();
        UnityEngine.Object.Destroy(this.m_audioSource.gameObject);
    }

    private void _OnComplete()
    {

    }

    private void _SetVolume(float value)
    {
        this.m_volume = value;
        this._ChangeVolume();
    }

    private void _SetMuted(bool value)
    {
        this.m_muted = value ? 0 : 1;
        this._ChangeVolume();
    }

    private void _ChangeVolume()
    {
        this.m_audioSource.volume = this.m_volume * (float)this.m_muted;
    }


    public event Action<AudioController> complete;

    public string audioName { get; set; }

    public float Volume
    {
        get
        {
            return this.m_volume;
        }
        set
        {
            this._SetVolume(value);
        }
    }

    public bool Muted
    {
        get
        {
            return this.m_muted == 0;
        }
        set
        {
            this._SetMuted(value);
        }
    }

    private bool m_paused;

    private AudioSource m_audioSource;

    private float m_volume = 1f;

    private int m_muted = 1;


}
