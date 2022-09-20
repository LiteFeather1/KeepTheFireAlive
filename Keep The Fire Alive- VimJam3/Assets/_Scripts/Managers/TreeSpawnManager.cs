using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform _tree;
    [SerializeField] private float _minTimeToSpawn;
    [SerializeField] private float _maxTimeToSpawn;
    private float _timeToSpawn;
    private float _passingTime = 0;
    [SerializeField] private Transform[] _possiblePositions;
    private readonly List<Vector3> _positions = new();
    private Vector3 _boxSize =  new(.16f, .16f);

    public static TreeSpawnManager Instance => GameManager.Instance.TreeSpawnManager;

    private void Start()
    {
        _timeToSpawn = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
        foreach (var item in _possiblePositions)
        {
            _positions.Add(item.position);
        }
    }

    private void Update()
    {
        if(_positions.Count > 0)
            _passingTime += Time.deltaTime;
        if(_passingTime >= _timeToSpawn)
        {
            _passingTime = 0;
            _timeToSpawn = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            SpawnTree();
        }
    }

    private void SpawnTree()
    {
        int randomPos = Random.Range(0, _positions.Count - 1);
        Instantiate(_tree, _positions[randomPos], Quaternion.identity);
        _positions.Remove(_positions[randomPos]);
    }

    public void AddTree(Vector3 position)
    {
        _positions.Add(position);
    }
        
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var item in _possiblePositions)
        { 
            Gizmos.DrawCube(item.position, _boxSize);
        }
    }
}
