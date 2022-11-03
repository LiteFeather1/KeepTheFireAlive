using UnityEngine;

public class RecipeHat : Recipe
{
    [SerializeField] private PlayerHat _playerHat;

    private void OnEnable()
    {
        ReturnAvailability(_playerHat.IsActive ? 0 : 1);
    }

    public override void ButtonSetCraft()
    {
        _playerHat.SetNewHat();
        base.ButtonSetCraft();
        CraftItem();
    }
}
