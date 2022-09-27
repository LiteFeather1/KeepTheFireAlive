using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightIntensityRadious : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float _maxLight, _minLight; 

    public void UpdateIntensity(float health, float maxHp = 100)
    {
        _light.intensity = Mathf.Lerp(_minLight, _maxLight, health / maxHp);
    }
}
