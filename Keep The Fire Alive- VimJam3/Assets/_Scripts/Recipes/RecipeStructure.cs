using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeStructure : Recipe
{
    [SerializeField] private GameObject _itemToCraft;
    [SerializeField] private Sprite _itemToCraftSprite;
    [SerializeField] private PlaceableTypes _typeOfPlaceable;

    public override void SetCraft()
    {
        CraftingManager.SetSpriteToSpawn(_itemToCraft, _itemToCraftSprite, CraftItem, _typeOfPlaceable);
        base.SetCraft();
    }
}
