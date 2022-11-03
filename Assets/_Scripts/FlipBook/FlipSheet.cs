using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Flip Sheet"))]
public class FlipSheet : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] 
    private float _fps;
    public Sprite[] Sprites => _sprites;
    public int Length => _sprites.Length;
    public float FPS => _fps;
}
