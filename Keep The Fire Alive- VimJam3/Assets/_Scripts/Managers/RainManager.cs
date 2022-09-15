using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    [Header("Rain")]
    [SerializeField] private float _timeToStartRaining = 180;
    [SerializeField] private float _rainingStrength = 1;
    private bool _isRaining;
    private Action<float> _rainStarted;
    [SerializeField] private ParticleSystem _rainParticle;

    public Action<float> RainStarted { get => _rainStarted; set => _rainStarted = value; }

    private static RainManager Instance => GameManager.Instance.RainManager;

    private void Start()
    {
        StartCoroutine(RainCounter());
    }

    private IEnumerator RainCounter()
    {
        yield return new WaitForSeconds(_timeToStartRaining);
        _rainStarted?.Invoke(_rainingStrength);
        _rainParticle.Play();
        GameManager.Instance.Ui.WarningText("Rain started careful not to get too Wet!", 5, new Color32(92, 105, 169, 255));
    }
}