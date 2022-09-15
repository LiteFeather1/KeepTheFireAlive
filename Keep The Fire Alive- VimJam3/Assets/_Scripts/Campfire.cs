using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Campfire : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] private float _life = 100;
    [SerializeField] private float _depleteRate = 1f;
    private float _rainingMultiplier = 1;
    private enum FireState { Big = 0, Medium = 1, Small = 2, AlmostDead = 3, Dead = 4}
    private FireState _fireState = FireState.Big;
    private FireState _previousFireState = FireState.Big;

    [Header("Lights")]
    [SerializeField] private Light2D _bigLight;
    [SerializeField] private float[] _bigRadiousPerState;
    [SerializeField] private float[] _minBigRadiousPerState;
    [SerializeField] private Light2D _smallLight;
    [SerializeField] private float[] _smallRadiousPerState;
    [SerializeField] private float[] _minSmallRadiousPerState;
    private float _timePassed;
    private float _rate = 1;

    [Header("Animator")]
    [SerializeField] private FlipBook _animator;
    [SerializeField] private FlipSheet _bigAnimation;
    [SerializeField] private FlipSheet _mediumAnimation;
    [SerializeField] private FlipSheet _smallAnimation;
    [SerializeField] private FlipSheet _almostAnimation;

    public static Campfire Instance { get; private set; }

    private GameManager _gm;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.RainManager.RainStarted += IsRaining;
    }

    private void Start()
    {

    }

    private void Update()
    {
        DepleteFire();
        CheckIfPrevious();

        HandleLights();
    }

    private void OnDisable()
    {
        _gm.RainManager.RainStarted -= IsRaining;
    }

    private void DepleteFire()
    {
        if (_life >= 0)
            _life -= Time.deltaTime * _rainingMultiplier * _depleteRate;
        WhatStateAreWe();
    }

    private void WhatStateAreWe()
    {
        switch (_life)
        {
            case <= 0:
                _fireState = FireState.Dead;
                break;
            case <= 12:
                _fireState = FireState.AlmostDead;
                break;
            case <= 37.5f:
                _fireState = FireState.Small;
                break;
            case <= 75:
                _fireState = FireState.Medium;
                break;
            case <= 100:
                _fireState = FireState.Big;
                break;
        }
    }

    private void CheckIfPrevious()
    {
        if(_previousFireState != _fireState)
        {
            StateChanged();
        }
        _previousFireState = _fireState;
    }

    private void StateChanged()
    {
        switch (_fireState)
        {
            case FireState.Big:
                StateChangedToBig();
                break;
            case FireState.Medium:
                StateChangedToMedium();
                break;
            case FireState.Small:
                StateChangedToSmall();
                break;
            case FireState.AlmostDead:
                StateChangedToAlmostDead();
                break;
            case FireState.Dead:
                StateChangedToDead();
                break;
        }
    }

    private void StateChangedToBig()
    {
        Play(_bigAnimation);
    }

    private void StateChangedToMedium()
    {
        Play(_mediumAnimation);
    }

    private void StateChangedToSmall()
    {
        Play(_smallAnimation);
    }

    private void StateChangedToAlmostDead()
    {
        Play(_almostAnimation);
    }

    private void StateChangedToDead()
    {
        _animator.gameObject.SetActive(false);
        _gm.GameLost();
    }

    private void HandleLights()
    {
        //_bigLight.pointLightOuterRadius = _bigRadiousPerState[(int)_fireState];
        _timePassed += Time.deltaTime * 1f * _rate;
        _bigLight.pointLightOuterRadius = Mathf.Lerp(_minBigRadiousPerState[(int)_fireState], _bigRadiousPerState[(int)_fireState],Mathf.PingPong(_timePassed,1));
        _smallLight.pointLightOuterRadius = Mathf.Lerp(_minSmallRadiousPerState[(int)_fireState], _smallRadiousPerState[(int)_fireState], Mathf.PingPong(_timePassed * 2, 1));
        _rate = Mathf.Sin(Time.time);
    }

    private void Play(FlipSheet _sheetToPlay)
    {
        _animator.Play(_sheetToPlay, true);
    }

    private void IsRaining(float rainStrength)
    {
        _rainingMultiplier = 2;
    }

    public void FeedMe(Materials materialToFeed)
    {
        if(materialToFeed == Materials.Wood)
        {
            _life += 10;
        }
        else if(materialToFeed == Materials.Grass)
        {
            _life += 5f;
        }
    }

    public void StealFire()
    {
        _life -= 10;
    }
}
