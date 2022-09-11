using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventorySystem))]
public class InventorySystemEditor : Editor
{
    Dictionary<Materials, int> _inventory;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        _inventory = new();
        InventorySystem inventorySystem = (InventorySystem)target;
        _inventory = inventorySystem.Inventory;

        if(_inventory != null)
            foreach (KeyValuePair<Materials, int> kvp in _inventory)
            {
                EditorGUILayout.IntField(kvp.Key.ToString(), kvp.Value);
            }

    }
}
