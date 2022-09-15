using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private int _hp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ice ice = collision.gameObject.GetComponent<Ice>();
        if(ice != null)
        {
            ice.Die();
            _hp--;
            if(_hp == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
