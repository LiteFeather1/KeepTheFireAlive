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
    [SerializeField] private ParticleSystem _feedParticle;

    private bool _warned;

    public static Campfire Instance { get; private set; }
    public float Life
    {
        get => _life; set
        {
            _life = Mathf.Clamp(value, 0, 125f);
            if (_life <= 25 && !_warned)
            {
                UiManager.Instance.WarningText("Mom's fire is getting low...", 3f, new Color32(200, 97, 80, 255));
                _warned = true;
            }
            else if (_life > 25 && _warned)
                _warned = false;
        }
    }

    private GameManager _gm;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(CO_OnEnable());
    }

    IEnumerator CO_OnEnable()
    {
        yield return new WaitForSeconds(1.25f);
        _gm = GameManager.Instance;
        _gm.RainManager.RainStarted += IsRaining;
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
        if (Life >= 0)
            Life -= Time.deltaTime * _rainingMultiplier * _depleteRate;
        WhatStateAreWe();
    }

    private void WhatStateAreWe()
    {
        switch (Life)
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

    public void StateChangedToBig()
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
        _gm.GameLost("Yours Moms fire went out");
    }

    private void HandleLights()
    {
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
        _rainingMultiplier = rainStrength;
    }

    public void FeedMe(Materials materialToFeed)
    {
        if(materialToFeed == Materials.Wood)
        {
            Life += 15;
            AudioManager.Instance.PlayFeedSound(.25f);
        }
        if (materialToFeed == Materials.Stone)
        {
            Life -= 10;
            UiManager.Instance.WarningText("That felt bad baby! What is that?!", 2f, Color.grey);
        }
        else if(materialToFeed == Materials.Grass)
        {
            Life += 10f;
            AudioManager.Instance.PlayFeedSound(.125f);
        }
        SpeedOfParticles(materialToFeed);
    }


    private void SpeedOfParticles(Materials materialsToPlay)
    {
        float velocity = _fireState switch
        {
            FireState.Big => 1,
            FireState.Medium => .75f,
            FireState.Small => .5f,
            FireState.AlmostDead => .25f,
            FireState.Dead => .0f,
            _ => .0f,
        };
        var velocityModule = _feedParticle.velocityOverLifetime;
        velocityModule.speedModifier = velocity;

        float quantity;
        if (materialsToPlay == Materials.Wood)
            quantity = 50;
        else
            quantity = 20f;
        var emmision = _feedParticle.emission;
        emmision.rateOverTime = quantity;

        _feedParticle.Play();
    }

    public void StealFire()
    {
        Life -= 10;
    }
}
