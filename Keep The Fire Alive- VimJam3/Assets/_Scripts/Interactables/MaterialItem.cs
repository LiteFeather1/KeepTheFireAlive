
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialItem : MonoBehaviour
{
    [SerializeField] private Materials _myType;
    [SerializeField] private int _howMuchToAdd = 1;
    [SerializeField] private AudioClip _collect;

    [Header("Wind")]
    [SerializeField] private Rigidbody2D _rb;

    private GameManager _gm;

    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.WindManager.WindEvent += Blow;
    }
    private void OnDisable()
    {
        _gm.WindManager.WindEvent -= Blow;
    }
    public void Collect()
    {
        InventorySystem.Instance.AddItem(_myType, _howMuchToAdd);
        Destroy(gameObject);
        AudioManager.Instance.PlaySound(_collect);
    }

    private void Blow(float min, float max)
    {
        float strength = Random.Range(min, max);
        _rb.AddForce(Vector2.right * strength);
    }
}
