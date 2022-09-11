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
    [SerializeField] private ActionInputState _actionInputState;
    [SerializeField] private PlayerAxe _playerAxe;
    [SerializeField] private PlayerHat _playerHat;

    [SerializeField] private Rigidbody2D _rb;

    private enum ActionInputState
    {
        NearMaterial,
        NearATree,
        NearCampfire,
        NearCraftingTable
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
        switch (_actionInputState)
        {

            case ActionInputState.NearATree:
                SwingAxe();
                break;
            case ActionInputState.NearCampfire:
                break;
            case ActionInputState.NearCraftingTable:
                break;
            default:
                break;
        }
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
}
