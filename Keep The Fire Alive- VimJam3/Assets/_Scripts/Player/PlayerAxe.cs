using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxe : MonoBehaviour
{
    [SerializeField] private int _durability;
    private readonly int _maxDurability = 24;
    [SerializeField] private InventoryItemUi _inventoryItemUi;
    [SerializeField] private AudioClip _woodChop;
    [SerializeField] private AudioClip _breakingSound;
    private bool _isActive;
    public bool IsActive => _isActive;

    private void Start()
    {
        _inventoryItemUi.SetMe(_durability, _maxDurability);    
    }

    public void SetNewAxe()
    {
        _isActive = true;
        _durability = _maxDurability;
        _inventoryItemUi.SetMe(_durability, _maxDurability);
    }

    public void SwingAxe(ChopTree tree)
    {
        if (_durability > 0)
        {
            _durability--;
            tree.Tackle();
            _inventoryItemUi.SetMe(_durability, _maxDurability);
            AudioManager.Instance.PlaySound(_woodChop);
            if (_durability == 0)
            {
                BreakAxe();
            }
        }
    }

    private void BreakAxe()
    {
        AudioManager.Instance.PlaySound(_breakingSound);
        print("AxeBroke");
        _isActive = false;
    }
}
