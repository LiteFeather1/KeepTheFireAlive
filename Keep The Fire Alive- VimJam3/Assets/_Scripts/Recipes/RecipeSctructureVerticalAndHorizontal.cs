using UnityEngine;

public class RecipeSctructureVerticalAndHorizontal : Recipe
{
    [SerializeField] private GameObject _itemToCraftVertical;
    [SerializeField] private Sprite _itemToCraftVerticalSprite;
    [SerializeField] private GameObject _itemToCraftHorizontal;
    [SerializeField] private Sprite _itemToCraftHorizontalSprite;
    [SerializeField] private PlaceableTypes _typeOfPlaceable = PlaceableTypes.Fence;

    public override void ButtonSetCraft()
    {
        CraftingManager?.SetSpriteToSpawnVerticalAndHorizontal(_itemToCraftVertical, _itemToCraftHorizontal, CraftItem, _typeOfPlaceable, _itemToCraftVerticalSprite, _itemToCraftHorizontalSprite);
        base.ButtonSetCraft();
    }
}
