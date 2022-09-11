using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<Materials, int> _inventory; 

    private Action _itemAddedEvent;

    public static InventorySystem Instance;

    public Dictionary<Materials, int> Inventory
    {
        get => _inventory; set => _inventory = value;
    }
    public Action ItemAddedEvent { get => _itemAddedEvent; set => _itemAddedEvent = value; }

    private void Awake()
    {
        Instance = this;
        InitInventory(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddItem(Materials.Wood, 1);
    }

    private void InitInventory()
    {
        _inventory = new();
        int enumSize = Enum.GetNames(typeof(Materials)).Length;

        for (int i = 0; i < enumSize; i++)
        {
            _inventory.Add((Materials)i, 10);
        }
    }

    private int ItemMaximumStack(Materials item)
    {
        return item switch
        {
            Materials.Wood => 16,
            Materials.Stone => 16,
            Materials.Grass => 32,
            _ => 16,
        };
    }

    public int GetItemAmount(Materials item)
    {
        return _inventory[item];
    }

    public void AddItem(Materials itemToAdd, int amountToAdd)
    {
        int currentAmount = GetItemAmount(itemToAdd);
        if (currentAmount == ItemMaximumStack(itemToAdd))
        {
            print($"Maximum stack for {itemToAdd}");
        }
        else
        {
            int newAmount = currentAmount + amountToAdd;
            _inventory[itemToAdd] = newAmount;
            _itemAddedEvent?.Invoke();
        }
    }

    public void RemoveItem(Materials itemToRemove, int amountToRemove)
    {
        int currentAmount = GetItemAmount(itemToRemove);
        if(currentAmount == 0)
        {
            print("Can't remove item");
        }
        else
        {
            int newAmount = currentAmount - amountToRemove;
            _inventory[itemToRemove] = newAmount;
        }
    }
}
