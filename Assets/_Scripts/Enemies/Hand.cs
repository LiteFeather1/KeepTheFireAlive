using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float _speed = .25f;
    private float _startSpeed;

    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _open;
    [SerializeField] private Sprite _closed;
    [SerializeField] private Transform _light; 
    private bool _fireStole;
    private bool _playerAbove;

    private Vector2 _position;
    private Vector2 _directionOffSet;
    private Vector2 _startPos;

    private Vector3 _campfirePos;

    private static int _handCount = 0;

    private void Start()
    {
        if (++_handCount > 0)
            AudioManager.Instance.PlayCreepySound();
        _startSpeed = _speed;
        SetRotation();
        _position = transform.position;
        _directionOffSet = (transform.position - _campfirePos).normalized;
        _position -= _collider.size * _directionOffSet / 2;
        _startPos = transform.position;
    }

    private void Update()
    {
        Moviment();
        CheckIfCanStealFire();
        CheckIfDespawn();
        // magic number bb but it comes from the sprite it self so not so magic?
        _light.position = new Vector2(_position.x + 0.14f, _position.y + 0.12f);
    }

    private void OnDisable()
    {
        _handCount--;
        if (_handCount <= 0)
            AudioManager.Instance?.StopCreepySound();
    }

    private void CheckIfCanStealFire()
    {
        if (Vector2.Distance(_position, _campfirePos) < 0.01f)
        {
            _speed = -3;
            Campfire.Instance.StealFire();
            _fireStole = true;
            _sr.sprite = _closed;
            _light.gameObject.SetActive(true);
            print("Stole Fire");
        }
    }

    private void CheckIfDespawn()
    {
        if(Vector2.Distance(_position, _startPos) < 1.5f)
        {
            if (_playerAbove || _fireStole) 
                Destroy(gameObject);
        }
    }
    private void Moviment()
    {
        float step = _speed * Time.deltaTime;
        float xPosStep = step * _directionOffSet.x; 
        float yPosStep = step * _directionOffSet.y; 
        _position = new Vector2(_position.x - xPosStep, _position.y - yPosStep);
        _sr.size = new Vector2(_sr.size.x, _sr.size.y + step);
        _collider.offset = new Vector2(_collider.offset.x, _collider.offset.y + step);
    }

    private void SetRotation()
    {
        _campfirePos = Campfire.Instance.transform.position;
        float angle = AngleBetweenTwoPoints(transform.position, _campfirePos);
        Quaternion to = Quaternion.Euler(new Vector3(0f, 0f, angle + 90));
        transform.rotation = to;
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_fireStole)
        {
            _speed = -1.25f;
            _playerAbove = true;
            _sr.sprite = _closed;
            StopAllCoroutines();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_fireStole)
        {
            _playerAbove = false;
            StartCoroutine(Backing());
        }
    }

    private IEnumerator Backing()
    {
        if (_playerAbove == false)
        {
            _speed = 0;
            yield return new WaitForSeconds(1f);
            _speed = _startSpeed;
            _sr.sprite = _open;
        }
    }
}
