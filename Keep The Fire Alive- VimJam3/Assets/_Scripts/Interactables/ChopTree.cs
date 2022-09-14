using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTree : MonoBehaviour
{
    [SerializeField] private int _hp = 12;
    [SerializeField] private int _amountToSpawn = 3;
    [SerializeField] private Rigidbody2D _woodPrefab;

    public void Tackle()
    {
        _hp--;
        if (_hp == 0)
        {
            for (int i = 0; i < _amountToSpawn; i++)
            {
                Rigidbody2D newWood = Instantiate(_woodPrefab, transform.position, transform.rotation);
                float rangePositive = Random.Range(0.5f, 1f);
                float rangeNegative = Random.Range(-0.5f, -1f);
                Vector2 randomDirection = new(Random.value > .5f ? rangePositive : rangeNegative, Random.value > .5f ? rangePositive : rangeNegative);
                newWood.AddForce(randomDirection * 12.5f);
            }
            Destroy(gameObject);
        }
    }
}
