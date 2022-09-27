using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightOuterRadiousAnimation : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float[] _maxRadious;
    [SerializeField] private float[] _minRadious;
    private int _stage = 0;
    private float _passingTime;
    private float _rate = 1;

    private void Update()
    {
        _passingTime += Time.deltaTime * _rate;
        _light.pointLightOuterRadius = Mathf.Lerp(_maxRadious[_stage], _minRadious[_stage], Mathf.PingPong(_passingTime, 1));
        _rate = Mathf.Sin(Time.time);
    }
    /// <summary>
    /// From Highest to lowest
    /// </summary>
    /// <param name="stage"></param>
    public void SetStage(int stage)
    {
        _stage = stage;
    }
}
