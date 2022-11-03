using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightOuterRadioustLerpAnimation : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float _maxRadious;
    [SerializeField] private float _maxMaxRadious;
    [SerializeField] private float _maxMinRadious;
    [SerializeField] private float _minRadious;
    [SerializeField] private float _minMaxRadious;
    [SerializeField] private float _minMinRadious;
    private float _passingTime;
    private float _rate = 1;

    public void LerpAnimation(float health, float maxHp = 100)
    {
        _passingTime += Time.deltaTime * _rate;
        _maxRadious = Mathf.Lerp(_maxMinRadious, _maxMaxRadious, health / maxHp);
        _minRadious = Mathf.Lerp(_minMinRadious, _minMaxRadious, health / maxHp);
        _light.pointLightOuterRadius = Mathf.Lerp(_maxRadious, _minRadious, Mathf.PingPong(_passingTime * _rate, 1));
        _rate = Mathf.Sin(Time.time);
    }
}
