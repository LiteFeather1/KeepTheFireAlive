using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private int _hitsToDie;
    private int _hits;
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private float[] _colliderRadious;
    [SerializeField] private LightOuterRadiousAnimation _light;
    [SerializeField] private AudioClip _breakingSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Hand>(out var hand))
        {
            _hits++;
            Destroy(hand.gameObject);
            if (_hits == _hitsToDie)
            {
                AudioManager.Instance.PlaySound(_breakingSound);
                Destroy(gameObject);
            }
            else
            {
                _collider.radius = _colliderRadious[_hits];
                _light.SetStage(_hits);
            }
        }
    }
}
