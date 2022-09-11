using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [Header("Crafting")]
    [SerializeField]  private Transform _itemToPlaceLocation;
    [SerializeField] private SpriteRenderer _currentItemToPlace;    
    private GameObject _itemToCraft;
    [Range(0f, 1f)] [SerializeField] private float _spriteTransparance;

    [Header("PlacementBoxes")]
    [SerializeField] private GameObject _fences;
    [SerializeField] private GameObject _torch;
    [SerializeField] private GameObject _shack;

    private System.Action _spawnAction;

    private static CraftingManager Instance => GameManager.Instance.CraftingManager;

    private void Start()
    {
        DeactivatePlacementBoxes();
    }

    private void PlacementBoxesToActivate(PlaceableTypes type)
    {
        switch (type)
        {
            case PlaceableTypes.Fence:
                _fences.SetActive(true);
                break;
            case PlaceableTypes.Torch:
                _torch.SetActive(true);
                break;
            case PlaceableTypes.Shack:
                _shack.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DeactivatePlacementBoxes()
    {
        _fences.SetActive(false);
        _torch.SetActive(false);
        _shack.SetActive(false);
    }

    public void SetSpriteToSpawn(GameObject objectToSpawn, Sprite spriteToUse, System.Action action, PlaceableTypes type)
    {
        _itemToCraft = objectToSpawn;
        _currentItemToPlace.sprite = spriteToUse;
        _currentItemToPlace.color = Color.clear;
        _spawnAction = action;
        PlacementBoxesToActivate(type);
    }

    public void SetSpriteVisiable(Transform position)
    {
        _currentItemToPlace.color = new Color(1, 1, 1, _spriteTransparance);
        _itemToPlaceLocation.position = position.position;
        _itemToPlaceLocation.rotation = position.rotation;
    }

    public void SetSpriteInvisiable()
    {
        _currentItemToPlace.color = Color.clear;
    }

    public void CraftObject()
    {
        Instantiate(_itemToCraft, _itemToPlaceLocation.position, _itemToPlaceLocation.rotation);
        _spawnAction?.Invoke();
        _spawnAction = null;
        DeactivatePlacementBoxes();
        _currentItemToPlace.sprite = null;
    }
}

public enum PlaceableTypes
{
    Fence,
    Torch,
    Shack
}
