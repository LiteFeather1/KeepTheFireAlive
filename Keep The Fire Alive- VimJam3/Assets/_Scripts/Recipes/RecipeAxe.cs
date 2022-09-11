using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeAxe : Recipe
{
    [SerializeField] private PlayerAxe _playerAxe;
    public override void SetCraft()
    {
        _playerAxe.SetNewAxe();
        base.SetCraft();
    }
}
