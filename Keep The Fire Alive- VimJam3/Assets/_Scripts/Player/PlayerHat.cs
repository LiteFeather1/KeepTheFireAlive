using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHat : MonoBehaviour
{
    [SerializeField] private float _durability;
    private float _isolationStrength = .25f;
    private float _depleatStrenght = .5f;
    private bool _isActive;

    [SerializeField] private InventoryItemUi _inventoryItemUi;

    [SerializeField] private Player _player;
    public bool IsActive => _isActive;
    public float IsolationStrength  => _isolationStrength;

    private void Start()
    {
        _inventoryItemUi.SetMe(_durability, 100);
        enabled = false;
    }

    private void Update()
    {
        DepleatDurabity();
        _inventoryItemUi.SetMe(_durability, 100);
    }

    public void SetNewHat()
    {
        _durability = 100;
        _isActive = true;
        enabled = _isActive;
        _player.SetIsolation(_isolationStrength);
    }

    private void DepleatDurabity()
    {
        if (_durability > 0)
            _durability -= Time.deltaTime * _depleatStrenght;
        if (_durability <= 0)
            BreakHat();
    }

    private void BreakHat()
    {
        print("HatBroke");
        _isActive = false;
        enabled = _isActive;
        _player.SetIsolation(0);
    }
}
