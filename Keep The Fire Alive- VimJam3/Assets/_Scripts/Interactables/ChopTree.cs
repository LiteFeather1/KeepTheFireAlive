using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTree : MonoBehaviour
{
    [SerializeField] private int _hp = 4;
    [SerializeField] private int _amountToSpawn = 3;
    [SerializeField] private Rigidbody2D _woodPrefab;

    [SerializeField] private Transform _leaves;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ParticleSystem _leavesParticles;
    [Header("Audio")]
    [SerializeField] private AudioClip _spawn;
    [SerializeField] private AudioClip _fall;


    private void Start()
    {
        AudioManager.Instance.PlaySound(_spawn);
    }

    public void Tackle()
    {
        _hp--;
        if (_hp >= 2)
        {
            StopAllCoroutines();
            StartCoroutine(Wiggle(_leaves));
            _leavesParticles.Play();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Wiggle(transform));
        }
        if (_hp == 2)
        {
            SpawnWoods();
            StopAllCoroutines();
            Destroy(_leaves.gameObject);
            AudioManager.Instance.PlaySound(_fall);
        }
        else if (_hp == 0)
        {
            SpawnWoods();
            GameManager.Instance.TreeSpawnManager.AddTree(transform.position);
            StopAllCoroutines();
            Destroy(gameObject);
        }
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
        float rotationGoal;
        Quaternion goal;
        float time ;
        float minRotation = 1.41875f;
        float maxRotation = 2.8375f;
        for (int i = 0; i < 3; i++)
        {
            rotationGoal = Random.Range(minRotation, maxRotation);
            goal = Quaternion.Euler(0, 0, rotationGoal);
            time = 0;
            while (!Compare(transform.rotation, goal, 1))
            {
                time += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, goal, time * _rotationSpeed);
                yield return null;
            }

            time = 0;
            rotationGoal = Random.Range(minRotation, maxRotation) * -1;
            goal = Quaternion.Euler(0, 0, rotationGoal);
            while (!Compare(transform.rotation, goal, 1))
            {

                time += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, goal, time * _rotationSpeed);
                yield return null;
            }
        }
        time = 0;
        goal = Quaternion.Euler(Vector3.zero);
        while (!Compare(transform.rotation, goal, 1))
        {
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

    private bool Compare(Quaternion quatA, Quaternion quatB , float range)
    {
        return Quaternion.Angle(quatA, quatB) < range;
}
}
