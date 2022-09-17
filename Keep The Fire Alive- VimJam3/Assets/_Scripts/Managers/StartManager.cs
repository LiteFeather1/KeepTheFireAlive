using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _gameObjectsToActivate;
    [SerializeField] private GameObject[] _objectsToDeactivate;
    [SerializeField] private FlipBook _startAnimation;
    [SerializeField] private GameObject _smallLights;
    [SerializeField] private Camera _cam;

    [Header("Campfire")]
    [SerializeField] private Campfire _campfire;
    [SerializeField] private ParticleSystem _burst;
    private bool _started = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if(!_started)
            {
                _cam.orthographicSize = 1.9f;
                _started = true;
                _startAnimation.Play();
                _burst.Play();
                _campfire.enabled = true;
                _campfire.StateChangedToBig();
                StartCoroutine(Wait());
            }    
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (var item in _gameObjectsToActivate)
        {
            item.SetActive(true);
        }
        foreach (var item in _objectsToDeactivate)
        {
            item.SetActive(false);
        }
        Destroy(gameObject);
    }
}