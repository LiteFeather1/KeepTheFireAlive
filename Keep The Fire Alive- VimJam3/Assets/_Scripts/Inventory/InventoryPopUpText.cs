using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPopUpText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Player _player;
    [SerializeField] private Materials _material;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.Ui.DisplayInventoryFeed(_player.InventoryPopUpText(_material));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.Ui.HideInventoryFeed();
    }
}
