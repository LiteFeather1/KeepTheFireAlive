using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Rain")]
    [SerializeField] private float _timeToStartRaining;
    [SerializeField] private float _rainingStrength;
    private bool _isRaining;
    private Action _rainStarted;

    public static GameManager Instance;
    [Header("Managers")]
    [SerializeField] private UiManager _ui;
    [SerializeField] private CraftingManager _craftingManager;

    public UiManager Ui { get => _ui; set => _ui = value; }
    public CraftingManager CraftingManager { get => _craftingManager; set => _craftingManager = value; }
    public Action RainStarted { get => _rainStarted; set => _rainStarted = value; }
    public float RainingStrength => _rainingStrength;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(RainCounter());
    }

    private void Update()
    {

    }

    private IEnumerator RainCounter()
    {
        yield return new WaitForSeconds(_timeToStartRaining);
        _rainStarted?.Invoke();
    }
}
