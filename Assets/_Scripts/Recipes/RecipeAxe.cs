using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeAxe : Recipe
{
    [SerializeField] private PlayerAxe _playerAxe;

    private void OnEnable()
    {
        ReturnAvailability(_playerAxe.IsActive ? 0 : 1);
    }

    public override void ButtonSetCraft()
    {
        base.ButtonSetCraft();
        _playerAxe.SetNewAxe();
        CraftItem();
    }
}
