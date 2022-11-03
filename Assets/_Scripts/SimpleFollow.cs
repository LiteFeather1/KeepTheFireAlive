using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private void Update()
    {
        transform.position = _parent.position;
    }
}
