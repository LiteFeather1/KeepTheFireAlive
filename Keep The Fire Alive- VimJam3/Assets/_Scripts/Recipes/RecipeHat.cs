using UnityEngine;

public class RecipeHat : Recipe
{
    [SerializeField] private PlayerHat _playerHat;
    public override void ButtonSetCraft()
    {
        _playerHat.SetNewHat();
        base.ButtonSetCraft();
    }
}
