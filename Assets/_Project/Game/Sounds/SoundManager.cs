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
    public bool _initialized = false;

    public IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(2f);
        // play theme in loop
        _theme.loop = true;
        InvokeRepeating(nameof(PlayTheme), 1f, _theme.clip.length + 0.5f);
        _popVolume = _pop.volume;
        _thudVolume = _thud.volume;
        _whooshVolume = _whoosh.volume;
        UpdateVolume(_volume);
        Mute(_mute);
        _initialized = true;
    }

    public void PlayTheme()
    {
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
        AudioSource.PlayClipAtPoint(_pop.clip, transform.position);
    }

    public void PlayThud(GenericEventOpts opts)
    {
        AudioSource.PlayClipAtPoint(_thud.clip, transform.position);
    }

    public void PlayWhoosh(GenericEventOpts opts)
    {
        AudioSource.PlayClipAtPoint(_whoosh.clip, transform.position);
    }

    public void Mute(bool m)
    {
        _theme.mute = m;
        _pop.mute = m;
        _thud.mute = m;
        _whoosh.mute = m;
    }
}