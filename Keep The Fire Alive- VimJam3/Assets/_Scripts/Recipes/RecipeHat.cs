using UnityEngine;

public class RecipeHat : Recipe
{
    [SerializeField] private PlayerHat _playerHat;

    protected override void OnEnable()
    {
        base.OnEnable();
        ReturnAvailability(_playerHat.IsActive ? 0 : 1);
    }

    public override void ButtonSetCraft()
    {
        _playerHat.SetNewHat();
        base.ButtonSetCraft();
        CraftItem();
    }
}
