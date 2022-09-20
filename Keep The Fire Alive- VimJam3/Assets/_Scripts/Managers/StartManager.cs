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
    [SerializeField] private AudioClip _flameBurst;

    [Header("Campfire")]
    [SerializeField] private Campfire _campfire;
    [SerializeField] private ParticleSystem _burst;
    private bool _started = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if(!_started)
            {
                AudioManager.Instance.PlaySound(_flameBurst, .5f);
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
        yield return new WaitForSeconds(1f);
        foreach (var item in _objectsToDeactivate)
        {
            item.SetActive(false);
        }
        foreach (var item in _gameObjectsToActivate)
        {
            item.SetActive(true);
        }
        UiManager.Instance.WarningText("The night is dark and full of terrors" + System.Environment.NewLine + "So a little was born to proctect his Mom", 3f);
        StartCoroutine(ZoomCamereOut());
    }

    private IEnumerator ZoomCamereOut()
    {
        float time = 0;
        Vector3 pos = new(0.25f, 0, -10);
        while (_cam.orthographicSize != 1.8f && _cam.transform.position.y != 0)
        {
            time += Time.deltaTime;
            pos.y = Mathf.Lerp(0.25f, 0, time);
            _cam.transform.position = pos;
            _cam.orthographicSize = Mathf.Lerp(0.9f, 1.8f, time);
            yield return null;
        }
        _cam.transform.position = new Vector3(0, 0, -10);
        Destroy(gameObject);
    }
}