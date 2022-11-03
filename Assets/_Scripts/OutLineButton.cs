using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutLineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Outline _outline;

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_button.IsInteractable())
        _outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.IsInteractable())
            _outline.enabled = true;
        if (!_button.IsInteractable())
            _outline.enabled = false;
    }
}
