using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private FlipBook _animator;
    [SerializeField] private FlipSheet _deadAnimation;

    private Vector2 _campFirePos;

    private void Start()
    {
        _campFirePos = Campfire.Instance.transform.position;
        Vector2 direction = (Vector2)transform.position - _campFirePos;
        if (Mathf.Sign(direction.x) > 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, .32f, 0);
    }

    private void FixedUpdate()
    {
        Moviment();
    }

    private void Moviment()
    {
        _rb.position = Vector2.MoveTowards(transform.position, _campFirePos, _speed * Time.deltaTime);
    }

    public void Die()
    {
        _animator.Play(_deadAnimation, false, true);
        StartCoroutine(Dieco());
    }

    private IEnumerator Dieco()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Campfire"))
        {
            Campfire.Instance.StealFire();
            print("Fire Stole " + gameObject);
            Die();
        }
    }
}
