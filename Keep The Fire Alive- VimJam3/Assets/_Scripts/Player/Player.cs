using System;
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
    private float _startLoseRate;
    [SerializeField] private bool _isProtected;

    [Header("Equipeables")]
    [SerializeField] private InputStates _inputStates;
    private InputStates _previousInputState;
    private System.Action<Materials> _invetoryUi;
    private Materials _currentMaterialsToDisplay;
    [SerializeField] private PlayerAxe _playerAxe;
    [SerializeField] private PlayerHat _playerHat;

    [Header("Interactables")]
    private MaterialItem _materialNear;
    private float _elapsedPickUpTime;
    private readonly float _timeToPickUp = 0.125f;
    private ChopTree _treeNear;
    private bool _axing;
    private OutLineWhenNear _outlineNear;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private CircleCollider2D _triggerCollider;

    [Header("Animations")]
    [SerializeField] private Animator _ac;
    [SerializeField] private Animator _leafAc;
    [SerializeField] private SpriteRenderer _leafSR;
    [SerializeField] private ParticleSystem _feedParticle;
    [SerializeField] private LightOuterRadioustLerpAnimation _myLight;
    [SerializeField] private LightIntensityRadious _light;

    [Header("AudioClips")]
    [SerializeField] private AudioClip _grassWalking;

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
    private bool _warned;

    public float FireLife
    {
        get => _fireLife; 
        set
        {
            _fireLife = Mathf.Clamp(value, 0, 100);
            GameManager.Instance.Ui.FireFireDisplay(FireLife);
            if (_fireLife <= 25 && !_warned)
            {
                UiManager.Instance.WarningText("Baby fire is getting low...", 2f, new Color32(200, 97, 80, 255));
                _warned = true;
            }
            else if (_fireLife > 25 && _warned)
                _warned = false;
        }
    }

    private bool _wetWarned;
    public float Wetness
    {
        get => _wetness; set
        {
            _wetness = Mathf.Clamp(value, 0, 100);
            GameManager.Instance.Ui.FireWetnessDisplay(_wetness);
            if (_wetness >= 75 && !_wetWarned)
            {
                UiManager.Instance.WarningText("You Are getting too wet Come near baby", 2f, new Color32(92, 105, 169, 255));
                _wetWarned = true;
            }
            else if (_wetness < 75 && _wetWarned)
                _wetWarned = false;
        }
    }

    public Action<Materials> InvetoryUi { get => _invetoryUi; set => _invetoryUi = value; }

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
        _gm.RainManager.RainStarted += StartRaining;
    }

    private void Start()
    {
        _inventorySystem = InventorySystem.Instance;
        _campfire = Campfire.Instance;
        _cam = Camera.main;
        _startLoseRate = _loseWetRate;
    }

    private void Update()
    {
        Inputs();
        Deaths();
        DepleteFire();
        Limit();
        DepleteWetness();
        PickUpTime();
        _myLight.LerpAnimation(_fireLife);
        _light.UpdateIntensity(_fireLife);
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) && _axing)
            ShowLeaf();
        EventFromMotherToMeAndVersa();
        PreviousInputState();
    }

    private void FixedUpdate()
    {
        if(!_axing)
            Moviment();
    }

    private void OnDisable()
    {
        _gm.RainManager.RainStarted -= StartRaining;
    }

    private void Moviment()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;

        _rb.velocity = _speed * direction;

        Flip(xInput);
        _ac.SetFloat("Speed", Mathf.Abs(_rb.velocity.magnitude));
        _leafAc.SetFloat("Speed", Mathf.Abs(_rb.velocity.magnitude));
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
        //InteractInput
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            InputAction();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_inputStates == InputStates.NearCampfire)
            {
                FeedCampfireMaterial(Materials.Wood);
            }
            else
            {
                FeedMe(Materials.Wood);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!_displayCantEat)
                StartCoroutine(CantEatRocks());
        }
        else  if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(_inputStates == InputStates.NearCampfire)
            {
                FeedCampfireMaterial(Materials.Grass);
            }
            else
            {
                FeedMe(Materials.Grass);
            }
        }
    }

    //private void Flickin

    private bool _displayCantEat;
    private IEnumerator CantEatRocks()
    {
        _displayCantEat = true;
        UiManager.Instance.DisplayInventoryFeed(InventoryPopUpText(Materials.Stone));
        yield return new WaitForSeconds(1f);
        UiManager.Instance.HideInventoryFeed();
        _displayCantEat = false;
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
                //FeedCampfireInput();
                break;
            case InputStates.NearCraftingBench:
                if(Input.GetKeyDown(KeyCode.Space))
                GameManager.Instance.Ui.SwitchCraftingMenuActive();
                break;
            case InputStates.Nothing:
                //FeedMeInput();
                break;
            default:
                break;
        }
        _triggerCollider.enabled = false;
        _triggerCollider.enabled = true;
    }


    private void PreviousInputState()
    {
        if(_previousInputState != _inputStates)
        {
            //Something
            UpdateLoseWetRate();
        }
        _previousInputState = _inputStates;
    }

    private void EventFromMotherToMeAndVersa()
    {
        if (_inputStates == InputStates.Nothing && _previousInputState == InputStates.NearCampfire)
        {
            _invetoryUi?.Invoke(_currentMaterialsToDisplay);
        }
        else if (_inputStates == InputStates.NearCampfire && _previousInputState == InputStates.Nothing)
        {
            _invetoryUi?.Invoke(_currentMaterialsToDisplay);
        }
    }

    private void CollectMaterial()
    {
        if (_materialNear != null && _elapsedPickUpTime >= _timeToPickUp)
        {
            _elapsedPickUpTime = 0;
            Flip(-(transform.position.x - _materialNear.transform.position.x));
            _materialNear.Collect();
        }
    }

    private void PickUpTime()
    {
        if (_elapsedPickUpTime <= _timeToPickUp)
            _elapsedPickUpTime += Time.deltaTime;
    }

    private void SwingAxe()
    {
        if (PlayerAxe.IsActive)
        {
            if (_treeNear != null && !_axing)
            {
                PlayAnimation();
            }
        }
        else
        {
            GameManager.Instance.Ui.WarningText("You need a Axe For that!", 1);
        }
    }

    /// <summary>
    /// Event for Animtion hit
    /// </summary>
    private void HitTree()
    {
        _playerAxe.SwingAxe(_treeNear);  
    }

    private void PlayAnimation()
    {
        if (_treeNear != null)
            Flip(-(transform.position.x - _treeNear.transform.position.x));
        _axing = true;
        _rb.velocity = Vector2.zero;
        _ac.SetBool("Axe", _axing);
        _leafSR.enabled = false;
        StartCoroutine(ExitAxingAnimation());
    }

    IEnumerator ExitAxingAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        HitTree();
        yield return new WaitForSeconds(0.125f);
        ShowLeaf();
    }

    /// <summary>
    /// AnimationEvent
    /// </summary>
    public void ShowLeaf()
    {
        _axing = false;
        _ac.SetBool("Axe", _axing);
        if (_playerHat.IsActive)
            _leafSR.enabled = true;
    }

    private void FeedCampfireInput()
    {
        if (_inventorySystem.GetItemAmount(Materials.Wood) > 0)
        {
            FeedCampfireMaterial(Materials.Wood);
        }
        else if (_inventorySystem.GetItemAmount(Materials.Grass) > 0)
        {
            FeedCampfireMaterial(Materials.Grass);
        }
        else
        {
            GameManager.Instance.Ui.WarningText("No Fuel to Feed", 1f);
            print("No Fuel to Feed");
        }
    }

    private void FeedCampfireMaterial(Materials materialToFeed)
    {
        if (_inventorySystem.GetItemAmount(materialToFeed) > 0)
        {
            _inventorySystem.RemoveItem(materialToFeed, 1);
            _campfire.FeedMe(materialToFeed);
            Flip(-(transform.position.x - _campfire.transform.position.x));
        }
    }

    public void ButtonFeedCampfireWood()
    {
        if (_inputStates == InputStates.NearCampfire)
            FeedCampfireMaterial(Materials.Wood);
    }

    public void ButtonFeedCampfireStone()
    {
        if (_inputStates == InputStates.NearCampfire)
            FeedCampfireMaterial(Materials.Stone);
    }

    public void ButtonFeedCampfireGrass()
    {
        if (_inputStates == InputStates.NearCampfire)
            FeedCampfireMaterial(Materials.Grass);
    }

    public string InventoryPopUpText(Materials material)
    {
        if(_inputStates == InputStates.NearCampfire)
        {
            if (material == Materials.Wood || material == Materials.Grass)
            {
                if (_inventorySystem.GetItemAmount(material) > 0)
                {
                    _currentMaterialsToDisplay = material;
                    return $"Feed Mom {material}?";
                }
                else
                {
                    _currentMaterialsToDisplay = material;
                    return "";
                }
            }
            else if(material == Materials.Stone)
            {
                if (_inventorySystem.GetItemAmount(material) > 0)
                {
                    _currentMaterialsToDisplay = material;
                    return "I can't feed Mom stones!";
                }
                else
                {
                    _currentMaterialsToDisplay = material;
                    return "";
                }
            }
        }
        else
        {
            if (material == Materials.Wood || material == Materials.Grass)
            {
                if (_inventorySystem.GetItemAmount(material) > 0)
                {
                    _currentMaterialsToDisplay = material;
                    return $"Eat {material}?";
                }
                else
                {
                    _currentMaterialsToDisplay = material;
                    return "";
                }

            }
            else if (material == Materials.Stone)
            {
                if (_inventorySystem.GetItemAmount(material) > 0)
                {
                    _currentMaterialsToDisplay = material;
                    return "I can't eat stones!";
                }
                else
                {
                    _currentMaterialsToDisplay = material;
                    return "";
                }
            }
        }
        return "lol";
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
                Wetness += (Time.deltaTime * rainStrength) / _isolationStrength;
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
        if (Wetness >= 100)
        {
            Die("Wetness", "You got too wet!");
        }
    }

    private void DeathOfLackOfFire()
    {
        if (FireLife <= 0)
        {
            Die("The fire depleted you", "Baby fire went out!");
        }
    }

    private void Die(string diedOf, string reason)
    {
        print($"Player Died Of {diedOf}");
        _gm.GameLost(reason);
        Destroy(gameObject);
    }

    private void DepleteFire()
    {
        float wetnessMultiplier = Mathf.Lerp(1, 2, Wetness / 100);

        float depleteRate = Time.deltaTime * _depleteRate * wetnessMultiplier;

        FireLife -= depleteRate;
    }

    private void UpdateLoseWetRate()
    {
        if (_inputStates == InputStates.NearCampfire)
        {
            _loseWetRate = _startLoseRate;
        }
        else
        {
            _loseWetRate = _startLoseRate / 4;
        }
    }

    private void DepleteWetness()
    { 
        if (Wetness > 0)
        {
            float protectedShelter = _isProtected ? 4 : 1;
            Wetness -= _loseWetRate * Time.deltaTime * protectedShelter;
        }
    }

    private void FeedMe(Materials materialToFeed)
    {
        if (_inventorySystem.GetItemAmount(materialToFeed) > 0)
        {
            _inventorySystem.RemoveItem(materialToFeed, 1);
            if(materialToFeed == Materials.Wood)
            {
                AudioManager.Instance.PlayFeedSound(.25f);
                FireLife += 10;
                SpeedOfParticles(Materials.Wood);
            }
            else if (materialToFeed == Materials.Stone)
            {
                FireLife -= 10;
                UiManager.Instance.WarningText("These grey things taste super bad yuck!", 2f, Color.grey);
            }
            else if(materialToFeed == Materials.Grass)
            {
                FireLife += 5;
                AudioManager.Instance.PlayFeedSound(.125f);
                SpeedOfParticles(Materials.Grass);
            }
        }
    }

    public void ButtonFeedMeWood()
    {
        if (_inputStates != InputStates.NearCampfire)
        {
            FeedMe(Materials.Wood);
        }
    }

    public void ButtonFeedMeStone()
    {
        if (_inputStates != InputStates.NearCampfire)
        {
            FeedMe(Materials.Stone);
        }
    }

    public void ButtonFeedMeGrass()
    {
        if (_inputStates != InputStates.NearCampfire)
        {
            FeedMe(Materials.Grass);
        }
    }

    private void SpeedOfParticles(Materials materialsToPlay)
    {
        float velocity;
        float quantity;
        if (materialsToPlay == Materials.Wood)
        {
            velocity = 1;
            quantity = 75;
        }
        else
        {
            velocity = .5f;
            quantity = 50f;
        }
        var velocityModule = _feedParticle.velocityOverLifetime;
        velocityModule.speedModifier = velocity;
        var emmision = _feedParticle.emission;
        emmision.rateOverTime = quantity;
        _feedParticle.Play();
    }
    // Animation Event
    private void PlayGrassSound()
    {
        AudioManager.Instance.PlaySound(_grassWalking);
    }

    private void Limit()
    {
        float yLimit = _cam.orthographicSize - _triggerCollider.radius;
        float xLimit = _cam.orthographicSize * _cam.aspect - _triggerCollider.radius + _triggerCollider.offset.x;
        if (transform.position.x > xLimit)
            transform.position = new Vector2(xLimit, transform.position.y);
        else if (transform.position.x < -xLimit)
            transform.position = new Vector2(-xLimit, transform.position.y);
        if (transform.position.y > yLimit)
            transform.position = new Vector2(transform.position.x, yLimit);
        else if (transform.position.y < -yLimit)
            transform.position = new Vector2(transform.position.x, -yLimit);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Ice>(out var ice))
        {
            FireLife -= 5;
            Wetness += 20;
            ice.Die();
        }
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
            if (collision.TryGetComponent<MaterialItem>(out var itemNear))
            {
                _materialNear = itemNear;
                _inputStates = InputStates.NearMaterial;
                SetOutlineOfInterectable(collision);
            }
        }
        else if (collision.CompareTag("Tree"))
        {
            if (!collision.TryGetComponent<ChopTree>(out var treeNear))
                return;

            _treeNear = treeNear;
            _inputStates = InputStates.NearATree;
            SetOutlineOfInterectable(collision);
        }
        else if (collision.CompareTag("Campfire"))
        {
            _inputStates = InputStates.NearCampfire;
            SetOutlineOfInterectable(collision);
        }
        else if (collision.CompareTag("CraftingBench"))
        {
            _inputStates = InputStates.NearCraftingBench;
            SetOutlineOfInterectable(collision);
        }
    }

    private void SetOutlineOfInterectable(Collider2D collision)
    {
        _outlineNear?.PlayerExit();
        _outlineNear = collision.GetComponent<OutLineWhenNear>();
        _outlineNear?.PlayerNear();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inputStates = InputStates.Nothing;
        _outlineNear?.PlayerExit();
        _outlineNear = null;
        if (collision.CompareTag("Shack"))
            _isProtected = false;
    }
}
