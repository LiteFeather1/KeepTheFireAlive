using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHat : MonoBehaviour
{
    [SerializeField] private float _durability;
    private readonly float _maxDurability = 100;
    [SerializeField] private float _isolationStrength = 2f;
    private readonly float _depleteRate = 2f;
    private bool _isActive;

    [SerializeField] private InventoryItemUi _inventoryItemUi;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private AudioClip _breakingSound;

    [SerializeField] private Player _player;
    public bool IsActive => _isActive;

    private void Start()
    {
        _inventoryItemUi.SetMe(_durability, _maxDurability);
        enabled = false;
    }

    private void Update()
    {
        DepleatDurabity();
        _inventoryItemUi.SetMe(_durability, _maxDurability);
    }

    public void SetNewHat()
    {
        _durability = 100;
        _isActive = true;
        enabled = _isActive;
        _sr.enabled = _isActive;
        _player.SetIsolation(_isolationStrength);
    }

    private void DepleatDurabity()
    {
        if (_durability > 0)
            _durability -= Time.deltaTime * _depleteRate;
        if (_durability <= 0)
            BreakHat();
    }

    private void BreakHat()
    {
        _inventoryItemUi.SetMe(0, _maxDurability);
        AudioManager.Instance.PlaySound(_breakingSound);
        print("HatBroke");
        _isActive = false;
        _sr.enabled = _isActive;
        _player.SetIsolation(1);
        enabled = _isActive;
    }
}
