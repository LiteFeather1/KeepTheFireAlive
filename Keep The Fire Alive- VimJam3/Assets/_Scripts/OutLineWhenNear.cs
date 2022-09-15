using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineWhenNear : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;

    public void PlayerNear()
    {
        _sr.material.SetFloat("OutlineThinckess", 1.16f);
    }

    public void PlayerExit()
    {
        _sr.material.SetFloat("OutlineThinckess", 0);
    }
}
