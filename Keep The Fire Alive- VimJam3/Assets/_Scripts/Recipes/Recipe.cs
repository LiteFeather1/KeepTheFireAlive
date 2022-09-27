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
    [SerializeField] protected Image _icon;
    [SerializeField] protected int _avaibleSpots;
    protected int _avaibleSpotsForCheck;

    protected InventorySystem InvetorySystem => InventorySystem.Instance;
    protected CraftingManager CraftingManager => GameManager.Instance.CraftingManager;
    protected UiManager UiManager => GameManager.Instance.Ui;

    protected virtual void OnEnable()
    {
        //CheckIfCanCraft();
    }

    protected bool CheckIfCanCraft()
    {
        int checksNeeded = _items.Length;
        for (int i = 0; i < _items.Length; i++)
        {
            if (InvetorySystem.GetItemAmount(_items[i]) >= _amountNecessary[i])
                checksNeeded--;
        }

        if (checksNeeded == 0)
        {
            return true;
        }
        else
        {
            //Debug.Log("Insuficient Materials " + gameObject.name);
            return false;
        }
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
        Time.timeScale = 1;
        AudioManager.Instance.PlayCraftedSound();
    }

    /// <summary>
    /// Ui Button Event
    /// </summary>
    public virtual void ButtonSetCraft()
    {
        UiManager.SwitchCraftingMenuActive();
        UiManager.DeactivatePopUpWindow();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UiManager.ActivatePopUpWindow(_button.GetComponent<RectTransform>(), _items, _amountNecessary, _description);
    }

    public void OnMouseEnter()
    {
        print("enter");
        UiManager.ActivatePopUpWindow(_button.GetComponent<RectTransform>(), _items, _amountNecessary, _description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.DeactivatePopUpWindow();
    }

    public void OnMouseExit()
    {
        UiManager.DeactivatePopUpWindow();
    }

    public void ReturnAvailability(int avaibles)
    {
        _avaibleSpotsForCheck = _avaibleSpots;
        _avaibleSpotsForCheck -= avaibles;
        if (_avaibleSpotsForCheck < _avaibleSpots && CheckIfCanCraft())
        {
            _button.interactable = true;
            _icon.color = Color.white;
        }
        else
        {
            _button.interactable = false;
            _icon.color = new Color(1, 1, 1, 0.5f);
        }
    }
}
