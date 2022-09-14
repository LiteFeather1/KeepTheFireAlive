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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hand hand = collision.GetComponent<Hand>();
        if(hand != null)
        {
            _hits++;
            _collider.radius = _colliderRadious[_hits];
            _light.SetStage(_hits);
            Destroy(hand.gameObject);
            if (_hits == _hitsToDie)
                Destroy(gameObject);
        }
    }
}
