using System;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    [Header("Wind")]
    [SerializeField] private float _minWindStrength = 12.5f;
    [SerializeField] private float _maxWindStrength = 25f;
    [SerializeField] private float _minTimeToBlow = 1;
    [SerializeField] private float _maxTimeToBlow = 2;
    private float _timeToBlow;
    private float _windPassingTime;
    public Action<float, float> _windEvent;
    [SerializeField] private ParticleSystem _windParticle;
    [SerializeField] private AudioClip _windGust;

    public static WindManager Instance => GameManager.Instance.WindManager;

    public Action<float, float> WindEvent { get => _windEvent; set => _windEvent = value; }

    private void Start()
    {
        _timeToBlow = UnityEngine.Random.Range(_minTimeToBlow, _maxTimeToBlow);
    }

    private void Update()
    {
        HandleWind();
    }

    private void HandleWind()
    {
        _windPassingTime += Time.deltaTime;
        if (_windPassingTime >= _timeToBlow)
        {
            _windPassingTime = 0;
            _timeToBlow = UnityEngine.Random.Range(_minTimeToBlow, _maxTimeToBlow);
            _windEvent?.Invoke(_minWindStrength, _maxWindStrength);
            _windParticle.Play();
            AudioManager.Instance.PlaySound(_windGust);
        }
    }
}