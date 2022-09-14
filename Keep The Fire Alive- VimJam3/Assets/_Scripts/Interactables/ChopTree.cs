using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTree : MonoBehaviour
{
    [SerializeField] private int _hp = 12;
    [SerializeField] private int _amountToSpawn = 3;
    [SerializeField] private Rigidbody2D _woodPrefab;

    [SerializeField] private Transform _leaves;
    [SerializeField] private float _rotationSpeed;
    private IEnumerator _wiggle;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {

        }
    }
    public void Tackle()
    {
        if(_hp > 3)
        {
            _wiggle = Wiggle(_leaves);
            if (_wiggle != null)
                StopCoroutine(_wiggle);
            StartCoroutine(_wiggle);
        }
        else
        {
            _wiggle = Wiggle(transform);
            if (_wiggle != null)
                StopCoroutine(_wiggle);
            StartCoroutine(_wiggle);
        }
        if (_hp == 3)
        {
            SpawnWoods();
            Destroy(_leaves);
        }
        else if (_hp == 0)
        {
            Destroy(gameObject);
        }
        _hp--;
    }

    private void SpawnWoods()
    {
        for (int i = 0; i < _amountToSpawn; i++)
        {
            Rigidbody2D newWood = Instantiate(_woodPrefab, transform.position, transform.rotation);
            float rangePositive = Random.Range(0.5f, 1f);
            float rangeNegative = Random.Range(-0.5f, -1f);
            Vector2 randomDirection = new(Random.value > .5f ? rangePositive : rangeNegative, Random.value > .5f ? rangePositive : rangeNegative);
            newWood.AddForce(randomDirection * 12.5f);
        }
    }

    private IEnumerator Wiggle(Transform transform)
    {
        float rotationGoal = 0;
        Quaternion goal = Quaternion.Euler(0, 0, rotationGoal);
        float time = 0;
        float minRotation = 1.41875f;
        float maxRotation = 2.8375f;
        for (int i = 0; i < 3; i++)
        {
            rotationGoal = Random.Range(minRotation, maxRotation);
            goal = Quaternion.Euler(0, 0, rotationGoal);
            time = 0;
            while (!compare(_leaves.rotation, goal, 1))
            {
                time += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, goal, time * _rotationSpeed);
                print(compare(_leaves.rotation, goal, 5));
                yield return null;
            }

            time = 0;
            rotationGoal = Random.Range(minRotation, maxRotation) * -1;
            goal = Quaternion.Euler(0, 0, rotationGoal);
            while (!compare(_leaves.rotation, goal, 1))
            {
                print("Plus");
                time += Time.deltaTime;
                _leaves.rotation = Quaternion.Lerp(transform.rotation, goal, time * _rotationSpeed);
                yield return null;
            }
        }
        time = 0;
        goal = Quaternion.Euler(Vector3.zero);
        while (!compare(_leaves.rotation, goal, 1))
        {
            print("Plus");
            time += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, goal, time * _rotationSpeed);
            yield return null;
        }
        transform.rotation = goal;
    }

    public bool Approximately(Quaternion quatA, Quaternion value, float acceptableRange)
    {
        return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
    }

    private bool compare(Quaternion quatA, Quaternion quatB , float range)
    {
        return Quaternion.Angle(quatA, quatB) < range;
}
}
