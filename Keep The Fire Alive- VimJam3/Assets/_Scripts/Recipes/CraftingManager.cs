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
    [SerializeField] private PlacementBox[] _fences;
    [SerializeField] protected RecipeSctructureVerticalAndHorizontal _returnAvailabilityForFence;
    [SerializeField] private PlacementBox[] _torch;
    [SerializeField] protected RecipeStructure _returnAvailabilityForTorch;
    [SerializeField] private PlacementBox[] _shack;
    [SerializeField] protected RecipeStructure _returnAvailabilityForShack;

    private System.Action _spawnAction;

    public static CraftingManager Instance => GameManager.Instance.CraftingManager;

    private void Start()
    {
        DeactivatePlacementBoxes();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            DeactivatePlacementBoxes();
    }

    private void PlacementBoxesToActivate(PlaceableTypes type)
    {
        switch (type)
        {
            case PlaceableTypes.Fence:
                int avaibleSpots = 0;
                foreach (var item in _fences)
                {
                    avaibleSpots += item.SetActive(true);
                }
                _returnAvailabilityForFence.ReturnAvailability(avaibleSpots);
                break;

            case PlaceableTypes.Torch:
                int avaibleSpotsTorch = 0;
                foreach (var item in _torch)
                {
                    avaibleSpotsTorch += item.SetActive(true);
                }
                _returnAvailabilityForTorch.ReturnAvailability(avaibleSpotsTorch);
                break;

            case PlaceableTypes.Shack:
                int avaibleSpotShack = 0;
                foreach (var item in _shack)
                {
                    avaibleSpotShack += item.SetActive(true);
                }
                _returnAvailabilityForShack.ReturnAvailability(avaibleSpotShack);
                break;
        }
    }

    public void Check()
    {
        PlacementBoxesToActivate(PlaceableTypes.Fence);
        PlacementBoxesToActivate(PlaceableTypes.Torch);
        PlacementBoxesToActivate(PlaceableTypes.Shack);
        DeactivatePlacementBoxes();
    }

    public void DeactivatePlacementBoxes()
    {
        foreach (var item in _fences)
        {
            item.SetActive(false);
        }
        foreach (var item in _torch)
        {
            item.SetActive(false);
        }
        foreach (var item in _shack)
        {
            item.SetActive(false);
        }
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
        GameObject newGameObject = Instantiate(_itemToCraft, _itemToPlaceLocation.position, Quaternion.identity);
        newGameObject.transform.localScale = _itemToPlaceLocation.localScale;
        _spawnAction?.Invoke();
        _spawnAction = null;
        DeactivatePlacementBoxes();
        _currentItemToPlace.sprite = null;
        return newGameObject;
    }
}

public enum PlaceableTypes
{
    Fence,
    Torch,
    Shack
}
