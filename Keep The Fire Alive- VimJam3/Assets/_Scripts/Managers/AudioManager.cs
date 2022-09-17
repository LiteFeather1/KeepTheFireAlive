using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private bool _muted;
    private float _volume = 1;
    [SerializeField] private AudioSource _effectSound;
    [SerializeField] private AudioSource _creepySound;
    [SerializeField] private AudioSource _rainSound;
    [Header("Audios")]
    [SerializeField] private AudioClip _craftSound;
    [SerializeField] private AudioClip _feedSound;

    public static AudioManager Instance => GameManager.Instance.AudioManager;

    public float Volume { get => _volume; set => _volume = Mathf.Clamp(value, 0, 1); }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            _muted = !_muted;
            if (_muted)
                AudioListener.volume = 0;
            else
                AudioListener.volume = Volume;
        }

        if(Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _volume += 0.1f;
            if (!_muted)
                AudioListener.volume = _volume;
        }
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _volume -= 0.1f;
            if (!_muted)
                AudioListener.volume = _volume;
        }
    }
    public void PlaySound(AudioClip clip)
    {
        _effectSound.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        _effectSound.PlayOneShot(clip, volume);
    }

    public void PlayCreepySound()
    {
        _creepySound.Play();
    }

    public void StopCreepySound()
    {
        _creepySound?.Stop();
    }

    public void PlayRainSound()
    {
        _rainSound.Play();
    }

    public void PlayCraftedSound()
    {
        PlaySound(_craftSound);
    }

    public void PlayFeedSound()
    {
        PlaySound(_feedSound);
    }
}
