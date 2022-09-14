using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxe : MonoBehaviour
{
    [SerializeField] private int _durability;
    [SerializeField] private InventoryItemUi _inventoryItemUi;
    private bool _isActive;
    public bool IsActive => _isActive;

    private void Start()
    {
        _inventoryItemUi.SetMe(_durability, 16);
    }

    public void SetNewAxe()
    {
        _isActive = true;
        _durability = 16;
        _inventoryItemUi.SetMe(_durability, 16);
    }

    public void SwingAxe(ChopTree tree)
    {
        if (_durability > 0)
        {
            _durability--;
            tree.Tackle();
            _inventoryItemUi.SetMe(_durability, 16);
            if (_durability == 0)
            {
                BreakAxe();
            }
            print("AxeSwong");
        }
    }

    private void BreakAxe()
    {
        print("AxeBroke");
        _isActive = false;
    }
}
