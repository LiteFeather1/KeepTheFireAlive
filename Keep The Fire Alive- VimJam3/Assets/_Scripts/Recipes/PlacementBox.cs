using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBox : MonoBehaviour
{
    private CraftingManager _craftingManager;
    private GameObject _myObject;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _sr;

    private void OnEnable()
    {
        if (_myObject == null)
            SetComponents(true);
    }

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
        _myObject = _craftingManager?.CraftObject();
        if (_myObject != null)
            SetComponents(false);
    }

    private void SetComponents(bool active)
    {
        _collider.enabled = active;
        _sr.enabled = active;
    }
}
