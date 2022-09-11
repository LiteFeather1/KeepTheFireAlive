using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Moviment")]
    [SerializeField] private float _speed;

    [Header("Fire")]
    [SerializeField] private float _fireLife;
    [SerializeField] private float _depleteRate = .5f;

    [Header("Weather")]
    [SerializeField] private float _wetness;
    [SerializeField] private float _isolationStrength;
    [SerializeField] private bool _isProtected;

    [Header("Equipeables")]
    [SerializeField] private InputStates _inputStates;
    [SerializeField] private PlayerAxe _playerAxe;
    [SerializeField] private PlayerHat _playerHat;

    [Header("Interactables")]
    private MaterialItem _materialNear;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _triggerCollider;
    public static Player Instance { get; private set; }

    private enum InputStates
    {
        NearMaterial,
        NearATree,
        NearCampfire,
        NearCraftingBench,
        Nothing
    }

    private GameManager _gm;
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

    }

    private void Update()
    {
        Inputs();
        Deaths();
        DepleteFire();
    }

    private void FixedUpdate()
    {
        Moviment();
    }

    private void Moviment()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;

        _rb.velocity = _speed  * direction;
    }

    private void Inputs()
    {
        if(Input.GetKeyDown(KeyCode.Space))
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
                break;
            case InputStates.NearCraftingBench:
                GameManager.Instance.Ui.SwitchCraftingMenuActive();
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
            _playerAxe.SwingAxe();
        else
            print("No Axe");
    }

    public void SetIsolation(float isolation)
    {
        _isolationStrength = isolation;
    }

    private void StartRaining()
    {
        StartCoroutine(RainingCo(_gm.RainingStrength));
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
        Destroy(gameObject);
        print($"Player Died Of {diedOf}");
    }

    private void DepleteFire()
    {
        float wetnessMultiplier = Mathf.Lerp(0, 2, _wetness / 100);

        _fireLife -= Time.deltaTime / _depleteRate * wetnessMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MaterialItem itemNear = collision.GetComponent<MaterialItem>();
        if (itemNear)
        {
            _materialNear = itemNear;
            _inputStates = InputStates.NearMaterial;
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
    }
}
