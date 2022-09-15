using UnityEngine;

public class MaterialSpawnManager : MonoBehaviour
{
    [Header("SpawnManager")]
    [SerializeField] private GameObject[] _materials;
    [SerializeField] private float _minTimeToSpawn;
    [SerializeField] private float _maxTimeToSpawn;
    private float _timeToSpawn;
    private float _spawnPassingTime;

    [SerializeField] private Camera _cam;

    private void Start()
    {
        _timeToSpawn = UnityEngine.Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
    }

    private void Update()
    {
        HandleSpawns();
    }

    private void HandleSpawns()
    {
        _spawnPassingTime += Time.deltaTime;
        if (_spawnPassingTime >= _timeToSpawn)
        {
            _spawnPassingTime = 0;
            _timeToSpawn = UnityEngine.Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            float whatToSpawn = UnityEngine.Random.Range(0, 100);
            if (whatToSpawn <= 20)
                whatToSpawn = 0;
            else if (whatToSpawn <= 67.5f)
                whatToSpawn = 1;
            else if (whatToSpawn <= 100)
                whatToSpawn = 2;
            float xPosition = (-_cam.orthographicSize * _cam.aspect) - 2 + UnityEngine.Random.Range(0, 1f);
            float yPosition = UnityEngine.Random.Range(-_cam.orthographicSize + 0.2f, _cam.orthographicSize - 0.2f);
            Instantiate(_materials[(int)whatToSpawn], new Vector2(xPosition, yPosition), Quaternion.identity);
        }
    }
}