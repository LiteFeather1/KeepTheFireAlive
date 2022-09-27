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
        UpdateText(_material);
    }

    private void UpdateText(Materials materials)
    {
        if(IsMoveOverUi())
            GameManager.Instance.Ui.DisplayInventoryFeed(_player.InventoryPopUpText(materials));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.Ui.HideInventoryFeed();
    }

    private bool IsMoveOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
