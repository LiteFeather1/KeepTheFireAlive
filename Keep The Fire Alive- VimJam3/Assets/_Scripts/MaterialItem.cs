using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialItem : MonoBehaviour
{
    [SerializeField] private Materials _myType;
    [SerializeField] private int _howMuchToAdd = 1;

    public void Collect()
    {
        InventorySystem.Instance.AddItem(_myType, _howMuchToAdd);
        Destroy(gameObject);
    }
}
