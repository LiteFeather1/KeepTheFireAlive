using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    [SerializeField] private Collider2D _collisionCollider;
    private float _timeToDisable = .4f;
    private float _elapsedTime;

    void Start()
    {
        RandomPostion();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _timeToDisable)
        {
            if(_collisionCollider != null)
                _collisionCollider.enabled = true;
            Destroy(this);
        }
    }

    public void RandomPostion()
    {
        float height = Utils.MainCamera.orthographicSize;
        float width = height / 9 * 16;

        transform.position = new Vector2(Random.Range(-width, width - (Random.Range(-.5f, -.25f))), Random.Range(-height, height));
        _timeToDisable += 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RandomPostion();
    }
}