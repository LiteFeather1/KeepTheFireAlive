using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private int _startItens;
    private Dictionary<Materials, int> _inventory; 

    [SerializeField] private InventoryMaterialUi[] _inventoryMaterialUi;
    private readonly Dictionary<Materials, InventoryMaterialUi>  _materialToInventoryUi = new(); 
    public static InventorySystem Instance;

    public Dictionary<Materials, int> Inventory { get => _inventory; set => _inventory = value; }

    private void Awake()
    {
        Instance = this;
        InitInventory();
    }

    private void InitInventory()
    {
        _inventory = new();
        int enumSize = Enum.GetNames(typeof(Materials)).Length;

        for (int i = 0; i < enumSize; i++)
        {
            _inventory.Add((Materials)i, _startItens);
            _materialToInventoryUi.Add((Materials)i, _inventoryMaterialUi[i]);
            _materialToInventoryUi[(Materials)i].SetMe(_inventory[(Materials)i]);
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
            _materialToInventoryUi[itemToAdd].SetMe(newAmount);
        }
    }

    public void RemoveItem(Materials itemToRemove, int amountToRemove)
    {
        int currentAmount = GetItemAmount(itemToRemove);
        if(currentAmount == 0)
            return;
        int newAmount = currentAmount - amountToRemove;
        _inventory[itemToRemove] = newAmount;
        _materialToInventoryUi[itemToRemove].SetMe(newAmount);
    }
}
