using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBox : MonoBehaviour
{
    protected CraftingManager _craftingManager;
    protected GameObject _myObject;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected SpriteRenderer _sr;

    protected void OnEnable()
    {
        if (_myObject == null)
            SetComponents(true);
    }

    protected void Start()
    {
        _craftingManager = GameManager.Instance.CraftingManager;
    }

    protected virtual void OnMouseEnter()
    {
        _craftingManager?.SetSpriteVisiable(transform);
    }

    protected void OnMouseExit()
    {
        _craftingManager?.SetSpriteInvisiable();
    }

    protected void OnMouseDown()
    {
        _myObject = _craftingManager?.CraftObject();
        if (_myObject != null)
            SetComponents(false);
    }

    protected void SetComponents(bool active)
    {
        _collider.enabled = active;
        _sr.enabled = active;
    }
}
