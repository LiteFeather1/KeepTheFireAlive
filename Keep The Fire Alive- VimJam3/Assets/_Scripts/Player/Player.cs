using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Moviment")]
    [SerializeField] private float _speed;
    private bool _facingRight = true;
    private IEnumerator _flipCo;

    [Header("Fire")]
    [SerializeField] private float _fireLife;
    [SerializeField] private float _depleteRate = .5f;

    [Header("Weather")]
    [SerializeField] private float _wetness;
    [SerializeField] private float _isolationStrength;
    [SerializeField] private float _loseWetRate;
    [SerializeField] private bool _isProtected;

    [Header("Equipeables")]
    [SerializeField] private InputStates _inputStates;
    [SerializeField] private PlayerAxe _playerAxe;
    [SerializeField] private PlayerHat _playerHat;

    [Header("Interactables")]
    private MaterialItem _materialNear;
    private ChopTree _treeNear;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _triggerCollider;

    private InventorySystem _inventorySystem;
    private Campfire _campfire;
    public static Player Instance { get; private set; }

    private enum InputStates
    {
        NearMaterial,
        NearCampfire,
        NearATree,
        NearCraftingBench,
        Nothing
    }

    private GameManager _gm;
    private Camera _cam;
    public PlayerAxe PlayerAxe { get => _playerAxe; set => _playerAxe = value; }
    public float FireLife
    {
        get => _fireLife; set
        {
            _fireLife = Mathf.Clamp(value, 0, 100);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.RainStarted += StartRaining;
    }

    private void Start()
    {
        _inventorySystem = InventorySystem.Instance;
        _campfire = Campfire.Instance;
        _cam = Camera.main;
    }

    private void Update()
    {
        Inputs();
        Deaths();
        DepleteFire();
        Limit();
        DepleteWetness();
    }

    private void FixedUpdate()
    {
        Moviment();
    }

    private void OnDisable()
    {
        _gm.RainStarted -= StartRaining;
    }

    private void Moviment()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;

        _rb.velocity = _speed * direction;

        Flip(xInput);
    }

    private void Flip(float xInput)
    {
        if ((xInput < 0 && _facingRight) || (xInput > 0 && !_facingRight))
        {
            _facingRight = !_facingRight;
            if (_flipCo != null)
                StopCoroutine(_flipCo);
            _flipCo = FlipCo();
            StartCoroutine(_flipCo);
        }
    }

    IEnumerator FlipCo()
    {
        if (!_facingRight)
        {
            while (transform.eulerAngles.y < 170)
            {
                Quaternion quaternion = Quaternion.Euler(0, 170, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, 10 * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            while (transform.eulerAngles.y > 10)
            {
                Quaternion quaternion = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, 10 * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InputAction();
        }
    }

    private void InputAction()
    {
        switch (_inputStates)
        {
            case InputStates.NearMaterial:
                CollectMaterial();
                break;
            case InputStates.NearATree:
                SwingAxe();
                break;
            case InputStates.NearCampfire:
                FeedCampfireInput();
                break;
            case InputStates.NearCraftingBench:
                GameManager.Instance.Ui.SwitchCraftingMenuActive();
                break;
            case InputStates.Nothing:
                FeedMeInput();
                break;
            default:
                break;
        }
        _triggerCollider.enabled = false;
        _triggerCollider.enabled = true;
    }

    private void CollectMaterial()
    {
        if (_materialNear != null)
            _materialNear.Collect();
    }

    private void SwingAxe()
    {
        if (PlayerAxe.IsActive)
            if (_treeNear != null)
                _playerAxe.SwingAxe(_treeNear);
            else
                print("No Axe");
    }

    private void FeedCampfireInput()
    {
        if (_inventorySystem.GetItemAmount(Materials.Wood) > 0)
        {
            FeedCampfireMaterial(Materials.Wood);
        }
        else if (_inventorySystem.GetItemAmount(Materials.Wood) > 0)
        {
            FeedCampfireMaterial(Materials.Grass);
        }
        else
        {
            print("No Fuel to Feed");
        }
    }

    private void FeedCampfireMaterial(Materials materialToFeed)
    {
        if (_inventorySystem.GetItemAmount(materialToFeed) > 0)
        {
            _inventorySystem.RemoveItem(materialToFeed, 1);
            _campfire.FeedMe(materialToFeed);
        }
    }

    public void ButtonFeedCampfireWood()
    {
        if (_inputStates == InputStates.NearCampfire)
            FeedCampfireMaterial(Materials.Wood);
    }

    public void ButtonFeedCampfireGrass()
    {
        if (_inputStates == InputStates.NearCampfire)
            FeedCampfireMaterial(Materials.Grass);
    }

    public void SetIsolation(float isolation)
    {
        _isolationStrength = isolation;
    }

    private void StartRaining(float rainStrenghth)
    {
        StartCoroutine(RainingCo(rainStrenghth));
    }

    private IEnumerator RainingCo(float rainStrength)
    {
        while (true)
        {
            if (!_isProtected)
                _wetness += (Time.deltaTime * rainStrength) / _isolationStrength;
            yield return new WaitForFixedUpdate();
        }
    }

    private void Deaths()
    {
        DeathOfLackOfFire();
        DeathOfWetness();
    }

    private void DeathOfWetness()
    {
        if (_wetness >= 100)
        {
            Die("Wetness");
        }
    }

    private void DeathOfLackOfFire()
    {
        if (FireLife <= 0)
        {
            Die("The fire depleted you");
        }
    }

    private void Die(string diedOf)
    {
        print($"Player Died Of {diedOf}");
        _gm.GameLost();
        Destroy(gameObject);
    }

    private void DepleteFire()
    {
        float wetnessMultiplier = Mathf.Lerp(1, 2, _wetness / 100);

        float depleteRate = Time.deltaTime / _depleteRate * wetnessMultiplier;

        _fireLife -= depleteRate;
    }

    private void DepleteWetness()
    {
        if (_isProtected)
        {
            if (_wetness > 0)
                _wetness -= _loseWetRate * Time.deltaTime;
        }
    }

    private void FeedMe(Materials materialToFeed)
    {
        if (_inventorySystem.GetItemAmount(materialToFeed) > 0)
        {
            _inventorySystem.RemoveItem(materialToFeed, 1);
            if(materialToFeed == Materials.Wood)
            {
                _fireLife += 10;
            }
            else if(materialToFeed == Materials.Grass)
            {
                _fireLife += 5;
            }
            
        }
    }

    private void FeedMeInput()
    {
        if (_inventorySystem.GetItemAmount(Materials.Wood) > 0)
        {
            FeedMe(Materials.Wood);
        }
        else if (_inventorySystem.GetItemAmount(Materials.Wood) > 0)
        {
            FeedMe(Materials.Grass);
        }
        else
        {
            print("No Fuel to Feed");
        }
    }

    private void Limit()
    {
        float yLimit = _cam.orthographicSize - transform.localScale.y / 2;
        float xLimit = _cam.orthographicSize * _cam.aspect - transform.localScale.x / 2;
        if (transform.position.x > xLimit)
            transform.position = new Vector2(xLimit, transform.position.y);
        else if (transform.position.x < -xLimit)
            transform.position = new Vector2(-xLimit, transform.position.y);
        if (transform.position.y > yLimit)
            transform.position = new Vector2(transform.position.x, yLimit);
        else if (transform.position.y < -yLimit)
            transform.position = new Vector2(transform.position.x, -yLimit);
    }

    public void ButtonFeedMeWood()
    {
        if (_inputStates == InputStates.Nothing)
            FeedMe(Materials.Wood);
    }

    public void ButtonFeedMeGrass()
    {
        if (_inputStates == InputStates.Nothing)
            FeedMe(Materials.Grass);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StateByTrigger(collision);

        if (collision.CompareTag("Shack"))
            _isProtected = true;
    }

    private void StateByTrigger(Collider2D collision)
    {
        if (collision.CompareTag("Material"))
        {
            MaterialItem itemNear = collision.GetComponent<MaterialItem>();
            if (itemNear != null)
            {
                _materialNear = itemNear;
                _inputStates = InputStates.NearMaterial;
            }
        }
        else if (collision.CompareTag("Tree"))
        {
            ChopTree treeNear = collision.GetComponent<ChopTree>();
            if (treeNear == null)
                return;
            _treeNear = treeNear;
            _inputStates = InputStates.NearATree;
        }
        else if (collision.CompareTag("Campfire"))
        {
            _inputStates = InputStates.NearCampfire;
        }
        else if (collision.CompareTag("CraftingBench"))
        {
            _inputStates = InputStates.NearCraftingBench;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inputStates = InputStates.Nothing;
        _materialNear = null;
        if (collision.CompareTag("Shack"))
            _isProtected = false;
    }
}
