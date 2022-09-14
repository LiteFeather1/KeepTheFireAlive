using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private float _waitSpawnHands = 30;
    [SerializeField] private GameObject _handsPrefab;
    [SerializeField] private float _handsMinTimeToSpawn;
    [SerializeField] private float _handsMaxTimeToSpawn;
    [SerializeField] private Transform[] _handSpawnPos;
    
    [Header("Ice")]
    [SerializeField] private float _waitSpawnIce = 180f;
    [SerializeField] private GameObject _icePrefab;
    [SerializeField] private float _iceMinTimeToSpawn;
    [SerializeField] private float _iceMaxTimeToSpawn;
    [SerializeField] private Transform[] _iceSpawnPos;

    private void Start()
    {
        StartHandSpawn();
        StartIceSpawn();
    }

    private void StartHandSpawn()
    {
        IEnumerator handSpawner = SpawnCo(_handsMinTimeToSpawn, _handsMaxTimeToSpawn, _handsPrefab, HandPosToSpawn(), true);
        StartCoroutine(StartCO(handSpawner, _waitSpawnHands));
    }

    private void StartIceSpawn()
    {
        IEnumerator iceSpawner = SpawnCo(_iceMinTimeToSpawn, _iceMaxTimeToSpawn, _icePrefab, IcePosToSpawn(), false);
        StartCoroutine(StartCO(iceSpawner, _waitSpawnIce));
    }

    private IEnumerator StartCO(IEnumerator co, float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(co);
    }

    private IEnumerator SpawnCo(float minTime, float maxTime, GameObject whatToSpawn, Vector2 posToSpawn, bool hand)
    {
        float timeToWait = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(timeToWait);
        Instantiate(whatToSpawn, posToSpawn, Quaternion.identity);
        // If Hand == true HandPose else IcePos
        Vector2 pos = hand ? HandPosToSpawn() : IcePosToSpawn();
        IEnumerator newCo = SpawnCo(minTime, maxTime, whatToSpawn, pos, hand);
        StartCoroutine(newCo);
    }

    private Vector2 HandPosToSpawn()
    {
        return _handSpawnPos[Random.Range(0, _handSpawnPos.Length)].position;
    }

    private Vector2 IcePosToSpawn()
    {
        int chosePos = Random.Range(0, _iceSpawnPos.Length);
        Vector2 pos = _iceSpawnPos[chosePos].position;
        Vector2 size = _iceSpawnPos[chosePos].localScale;
        Vector2 posReturn = new(pos.x + Random.Range(-size.x, size.x), pos.y + Random.Range(-size.y, size.y));
        return posReturn;
    }
}
