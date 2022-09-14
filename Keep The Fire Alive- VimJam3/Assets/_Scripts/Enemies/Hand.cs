using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float _speed = 1;

    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _sr;
    private bool _stoleFire;
    private bool _playerAbove;

    private Vector2 _position;
    private Vector2 _directionOffSet;
    private Vector2 _startPos;

    private Vector3 _campfirePos;

    private void Start()
    {
        SetRotation();
        _position = transform.position;
        _directionOffSet = (transform.position - _campfirePos).normalized;
        _position -= _collider.size * _directionOffSet;
        _startPos = transform.position;
    }

    private void Update()
    {
        Moviment();
        CheckIfCanStealFire();
        CheckIfDespawn();
    }

    private void CheckIfCanStealFire()
    {
        if (Vector2.Distance(_position, _campfirePos) < 0.1f)
        {
            _speed = -2;
            Campfire.Instance.StealFire();
            _stoleFire = true;
            print("Stole Fire");
        }
    }

    private void CheckIfDespawn()
    {
        if(Vector2.Distance(_position, _startPos) < 0.5f)
        {
            if (_playerAbove)
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
        if (collision.CompareTag("Player") && !_stoleFire)
        {
            _speed *= -1;
            _playerAbove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_stoleFire)
        {
            _speed *= -1;
            _playerAbove = false;
        }
    }
}
