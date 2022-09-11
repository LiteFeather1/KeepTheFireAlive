using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBox : MonoBehaviour
{
    private CraftingManager _craftingManager;

    private void Start()
    {
        _craftingManager = GameManager.Instance.CraftingManager;
    }

    private void OnMouseEnter()
    {
        _craftingManager?.SetSpriteVisiable(transform);
    }

    private void OnMouseExit()
    {
        _craftingManager?.SetSpriteInvisiable();
    }

    private void OnMouseDown()
    {
        _craftingManager?.CraftObject();
    }
}
