using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineWhenNear : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _sr.material.SetFloat("OutlineThinckess", 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _sr.material.SetFloat("OutlineThinckess", 0);
    }
}
