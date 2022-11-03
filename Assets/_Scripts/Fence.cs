using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private AudioClip _breakingSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ice>(out var ice))
        {
            ice.Die();
            _hp--;
            if(_hp == 0)
            {
                AudioManager.Instance.PlaySound(_breakingSound);
                Destroy(gameObject);
            }
        }
    }
}
