using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineWhenNear : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;

    private void Start()
    {
        _sr.material.SetColor("OutlineColor", Color.white);
    }

    public void PlayerNear()
    {
        _sr.material.SetFloat("OutlineThinckess", 1.16f);
    }

    public void PlayerExit()
    {
        _sr.material.SetFloat("OutlineThinckess", 0);
    }
}
