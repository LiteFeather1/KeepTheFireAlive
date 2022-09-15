using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _effectSound;

    private static AudioManager Instance => GameManager.Instance.AudioManager;

    public void PlaySound(AudioClip clip)
    {
        _effectSound.PlayOneShot(clip);
    }
}
