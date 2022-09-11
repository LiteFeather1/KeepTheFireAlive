using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Crafting")]
    [SerializeField] protected Materials[] _items;
    [SerializeField] protected int[] _amountNecessary;
    [SerializeReference] [TextArea] protected string _description;

    [Header("Components")]
    [SerializeField] protected Button _button;
    protected bool _isCraftable;

    protected InventorySystem InvetorySystem => InventorySystem.Instance;
    protected CraftingManager CraftingManager => GameManager.Instance.CraftingManager;
    protected UiManager UiManager => GameManager.Instance.Ui;

    protected void OnEnable()
    {
        CheckIfCanCraft();
    }

    protected void OnMouseEnter()
    {
        print("Mouse");
    }

    protected void OnMouseExit()
    {
        UiManager.DeactivatePopUpWindow();
    }

    protected void CheckIfCanCraft()
    {
        int checksNeeded = _items.Length;
        for (int i = 0; i < _items.Length; i++)
        {
            if (InvetorySystem.GetItemAmount(_items[i]) >= _amountNecessary[i])
                checksNeeded--;
        }

        if (checksNeeded == 0)
        {
            _isCraftable = true;
        }
        else
        {
            _isCraftable = false;
            Debug.Log("Insuficient Materials");
        }

        _button.interactable = _isCraftable;
    }

    protected void RemoveMaterialsOfInventory(Materials item, int amountToRemove)
    {
        InvetorySystem.RemoveItem(item, amountToRemove);
    }

    protected void CraftItem()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            RemoveMaterialsOfInventory(_items[i], _amountNecessary[i]);
        }
        Debug.Log("Crafted");
    }

    /// <summary>
    /// Ui Button Event
    /// </summary>
    public virtual void SetCraft()
    {
        UiManager.SwitchCraftingMenuActive();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UiManager.ActivatePopUpWindow(_button.GetComponent<RectTransform>(), _items, _amountNecessary, _description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.DeactivatePopUpWindow();
    }
}
