using UnityEngine;

public class RecipeHat : Recipe
{
    [SerializeField] private PlayerHat _playerHat;
    public override void SetCraft()
    {
        _playerHat.SetNewHat();
        base.SetCraft();
    }
}
