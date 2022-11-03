using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(InventorySystem))]
public class InventorySystemEditor : Editor
{
    Dictionary<Materials, int> _inventory;
    //"Serializing" the dictionary to be seen on the inspector
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
