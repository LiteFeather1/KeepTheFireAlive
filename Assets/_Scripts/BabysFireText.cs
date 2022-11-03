using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BabysFireText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.SetActive(false);
    }
}