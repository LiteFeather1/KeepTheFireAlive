using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private int _hp;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _horizontalSprite;
    [SerializeField] private Sprite _verticalSprite;

    private void Start()
    {
        float rotation = transform.rotation.z;
        //if (rotation == 270 || rotation == 90)
        //    _sr.sprite = _verticalSprite;
        //else
        //    _sr.sprite = _horizontalSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ice ice = collision.gameObject.GetComponent<Ice>();
        if(ice != null)
        {
            Destroy(ice.gameObject);
            _hp--;
            if(_hp == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
