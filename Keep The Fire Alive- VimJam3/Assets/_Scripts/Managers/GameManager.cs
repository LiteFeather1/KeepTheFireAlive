using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _timeToWin;

    public static GameManager Instance;
    [Header("Managers")]
    [SerializeField] private UiManager _ui;
    [SerializeField] private CraftingManager _craftingManager;
    [SerializeField] private TreeSpawnManager _treeSpawnManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private RainManager _rainManager;
    [SerializeField] private WindManager _windManager;

    public UiManager Ui { get => _ui; set => _ui = value; }
    public CraftingManager CraftingManager { get => _craftingManager; set => _craftingManager = value; }
    public TreeSpawnManager TreeSpawnManager { get => _treeSpawnManager; set => _treeSpawnManager = value; }
    public AudioManager AudioManager { get => _audioManager; set => _audioManager = value; }
    public RainManager RainManager { get => _rainManager; set => _rainManager = value; }
    public WindManager WindManager { get => _windManager; set => _windManager = value; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Ui.TimeToDisplay(_timeToWin);
        _timeToWin -= Time.deltaTime;

        if (_timeToWin <= 0)
            GameWon();
    }

    public void GameWon()
    {
        print("Game Won!");
    }

    public void GameLost(string reason)
    {
        print("Game Lost!");
        Utils.EndGameLostText = reason;
        Utils.TimeLeft = $"Time Left {Mathf.FloorToInt(_timeToWin / 60)} : {Mathf.FloorToInt(_timeToWin % 60): 00}";
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
