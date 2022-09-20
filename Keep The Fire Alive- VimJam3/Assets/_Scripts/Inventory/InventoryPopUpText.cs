using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPopUpText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Player _player;
    [SerializeField] private Materials _material;

    private void Start()
    {
        _player.InvetoryUi += UpdateText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        GameManager.Instance.Ui.DisplayInventoryFeed(_player.InventoryPopUpText(_material));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.Ui.HideInventoryFeed();
    }
}
