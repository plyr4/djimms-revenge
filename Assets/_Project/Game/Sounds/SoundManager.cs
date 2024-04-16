using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            // attempt to locate the singleton
            if (_instance == null)
            {
                _instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
            }

            // create a new singleton
            if (_instance == null)
            {
                _instance = (new GameObject("SoundManager")).AddComponent<SoundManager>();
            }

            // return singleton
            return _instance;
        }
    }

    public AudioSource _theme;
    public float _themeVolume = 0.5f;

    public AudioSource _pop;
    public float _popVolume = 0.5f;


    public AudioSource _thud;
    public float _thudVolume = 0.5f;

    public AudioSource _whoosh;
    public float _whooshVolume = 0.5f;

    public float _volume = 0.2f;
    public bool _mute = false;

    public void Start()
    {
        // play theme in loop
        _theme.loop = true;
        _theme.PlayScheduled(0);
        _popVolume = _pop.volume;
        _thudVolume = _thud.volume;
        _whooshVolume = _whoosh.volume;
        UpdateVolume(_volume);
        Mute(_mute);
    }

    void Update()
    {
        // Play the audio for this image
        if (_theme.isPlaying != true)
            AudioSource.PlayClipAtPoint(_theme.clip, transform.position);
    }

    public void UpdateVolume(float volume)
    {
        _volume = volume;

        _theme.volume = volume * _themeVolume;
        _pop.volume = volume * _popVolume;
        _thud.volume = volume * _thudVolume;
        _whoosh.volume = volume * _whooshVolume;
    }

    public void PlayPop(GenericEventOpts opts)
    {
        _pop.PlayOneShot(_pop.clip);
    }

    public void PlayThud(GenericEventOpts opts)
    {
        _thud.PlayOneShot(_thud.clip);
    }

    public void PlayWhoosh(GenericEventOpts opts)
    {
        _whoosh.PlayOneShot(_whoosh.clip);
    }

    public void Mute(bool m)
    {
        _theme.mute = m;
        _pop.mute = m;
        _thud.mute = m;
        _whoosh.mute = m;
    }
}