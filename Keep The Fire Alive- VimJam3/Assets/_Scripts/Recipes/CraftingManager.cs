using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [Header("Crafting")]
    [SerializeField]  private Transform _itemToPlaceLocation;
    [SerializeField] private SpriteRenderer _currentItemToPlace; 
    private GameObject _itemToCraft;
    private GameObject _itemToCraftVertical;
    private GameObject _itemToCraftHorizontal;
    private Sprite _verticalSprite;
    private Sprite _horizontalSprite;
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

    public void SetSpriteVisiable(Transform transform)
    {
        _currentItemToPlace.color = new Color(1, 1, 1, _spriteTransparance);
        _itemToPlaceLocation.position = transform.position;
        float sign = Mathf.Sign(transform.localScale.x);
        _itemToPlaceLocation.localScale = new Vector3(1 * sign, 1);
    }

    public void SetSpriteToSpawnVerticalAndHorizontal(GameObject verticalGO, GameObject horizontal, System.Action action, PlaceableTypes type, params Sprite[] verticaHorizontalS)
    {
        _itemToCraftVertical = verticalGO;
        _itemToCraftHorizontal = horizontal;
        _verticalSprite = verticaHorizontalS[0];
        _horizontalSprite = verticaHorizontalS[1];
        _spawnAction = action;
        PlacementBoxesToActivate(type);
    }

    public void SetSpriteVisiableHorizontalVertical(Transform transform)
    {
        _currentItemToPlace.color = new Color(1, 1, 1, _spriteTransparance);
        if (transform.eulerAngles.z == 90 || transform.eulerAngles.z == 270)
        {
            _currentItemToPlace.sprite = _verticalSprite;
            _itemToCraft = _itemToCraftVertical;
        }
        else
        {
            _currentItemToPlace.sprite = _horizontalSprite;
            _itemToCraft = _itemToCraftHorizontal;
        }
        _itemToPlaceLocation.position = transform.position;
    }

    public void SetSpriteInvisiable()
    {
        _currentItemToPlace.color = Color.clear;
    }

    public GameObject CraftObject()
    {
        Instantiate(_itemToCraftHorizontal, _itemToPlaceLocation.position, Quaternion.identity);
        _spawnAction?.Invoke();
        _spawnAction = null;
        DeactivatePlacementBoxes();
        _currentItemToPlace.sprite = null;
        return _itemToCraftHorizontal;
    }
}

public enum PlaceableTypes
{
    Fence,
    Torch,
    Shack
}
