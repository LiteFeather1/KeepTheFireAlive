using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Rain")]
    [SerializeField] private float _timeToStartRaining;
    [SerializeField] private float _rainingStrength;
    private bool _isRaining;
    private Action<float> _rainStarted;

    [Header("Wind")]
    [SerializeField] private float _minWindStrength;
    [SerializeField] private float _maxWindStrength;
    [SerializeField] private float _minTimeToBlow;
    [SerializeField] private float _maxTimeToBlow;
    private float _timeToBlow;
    private float _windPassingTime;
    public Action<float,float> _windEvent;

    [Header("SpawnManager")]
    [SerializeField] private GameObject[] _materials;
    [SerializeField] private float _minTimeToSpawn;
    [SerializeField] private float _maxTimeToSpawn;
    private float _timeToSpawn;
    private float _spawnPassingTime;

    public static GameManager Instance;
    [Header("Managers")]
    [SerializeField] private UiManager _ui;
    [SerializeField] private CraftingManager _craftingManager;
    [SerializeField] private Camera _cam;

    public UiManager Ui { get => _ui; set => _ui = value; }
    public CraftingManager CraftingManager { get => _craftingManager; set => _craftingManager = value; }
    public Action<float> RainStarted { get => _rainStarted; set => _rainStarted = value; }
    public float RainingStrength => _rainingStrength;

    public Action<float,float> WindEvent { get => _windEvent; set => _windEvent = value; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(RainCounter());
        _timeToBlow = UnityEngine.Random.Range(_minTimeToBlow, _maxTimeToBlow);
        _timeToSpawn = UnityEngine.Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
    }

    private void Update()
    {
        HandleWind();
        HandleSpawns();
    }

    private IEnumerator RainCounter()
    {
        yield return new WaitForSeconds(_timeToStartRaining);
        _rainStarted?.Invoke(_rainingStrength);
    }

    private void HandleWind()
    {
        _windPassingTime += Time.deltaTime;
        if(_windPassingTime >= _timeToBlow)
        {
            _windPassingTime = 0;
            _timeToBlow = UnityEngine.Random.Range(_minTimeToBlow, _maxTimeToBlow);
            _windEvent?.Invoke(_minWindStrength, _maxWindStrength);
        }
    }

    private void HandleSpawns()
    {
        _spawnPassingTime += Time.deltaTime;
        if(_spawnPassingTime >= _timeToSpawn)
        {
            _spawnPassingTime = 0;
            _timeToSpawn = UnityEngine.Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            int whatToSpawn = UnityEngine.Random.Range(0, 100);
            if (whatToSpawn <= 25)
                whatToSpawn = 2;
            else if (whatToSpawn <= 50)
                whatToSpawn = 1;
            else if (whatToSpawn <= 100)
                whatToSpawn = 0;

            float xPosition = (-_cam.orthographicSize * _cam.aspect) - 2 + UnityEngine.Random.Range(0, 1f);
            float yPosition = UnityEngine.Random.Range(-_cam.orthographicSize +0.2f, _cam.orthographicSize- 0.2f);
            Instantiate(_materials[whatToSpawn], new Vector2(xPosition, yPosition), Quaternion.identity);
        }
    }

    public void GameWon()
    {
        print("Game Won!");
    }

    public void GameLost()
    {
        print("Game Lost!");
    }
}
