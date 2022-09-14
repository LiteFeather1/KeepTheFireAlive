using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineFromTo : MonoBehaviour
{
    [SerializeField] private Transform _from;
    [SerializeField] private Transform _to;

    private void OnDrawGizmos()
    {
        if (_from == null || _to == null)
            return;
        Gizmos.DrawLine(_from.position, _to.position);
    }
}
