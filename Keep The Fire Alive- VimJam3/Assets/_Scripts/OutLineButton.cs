using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutLineButton : MonoBehaviour
{

    [SerializeField] private Outline _outline;

    private void OnMouseEnter()
    {
        _outline.enabled = true;
    }

    public void OnMouseExit()
    {
        _outline.enabled = false;
    }
}
