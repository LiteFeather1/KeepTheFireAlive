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
        _timeToSpawn = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
    }

    private void Update()
    {
        HandleSpawns();
    }

    private void OnDrawGizmos()
    {
        //float xPosition = (-_cam.orthographicSize * _cam.aspect) - .25f + Random.Range(-.25f, .25f);
        //float yPosition = Random.Range(-_cam.orthographicSize + 0.2f, _cam.orthographicSize - 0.2f);
        //Gizmos.DrawCube(new Vector3(xPosition, yPosition), Vector3.one / 6);
    }

    private void HandleSpawns()
    {
        _spawnPassingTime += Time.deltaTime;
        if (_spawnPassingTime >= _timeToSpawn)
        {
            _spawnPassingTime = 0;
            _timeToSpawn = Random.Range(_minTimeToSpawn, _maxTimeToSpawn);
            float whatToSpawn = Random.Range(0, 100);
            if (whatToSpawn <= 20)
                whatToSpawn = 0;
            else if (whatToSpawn <= 67.5f)
                whatToSpawn = 1;
            else if (whatToSpawn <= 100)
                whatToSpawn = 2;
            float xPosition = (-_cam.orthographicSize * _cam.aspect) - 0.25f + Random.Range(-.25f, .25f);
            float yPosition = Random.Range(-_cam.orthographicSize + 0.2f, _cam.orthographicSize - 0.2f);
            Instantiate(_materials[(int)whatToSpawn], new Vector2(xPosition, yPosition), Quaternion.identity);
        }
    }
}