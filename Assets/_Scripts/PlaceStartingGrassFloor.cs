using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceStartingGrassFloor : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private RuleTile _grassTile;
    [SerializeField] private int _minHowManyToPlace;
    [SerializeField] private int _maxHowManyToPlace;

    private void Start()
    {
        float height = 1.9f * 6.25f;
        float width = height / 9 * 16; 

        int howMany = Random.Range(_minHowManyToPlace, _maxHowManyToPlace);
        for (int i = 0; i < howMany; i++)
        {
            _floorTilemap.SetTile(new Vector3Int((int)Random.Range(-width, width), (int)Random.Range(-height, height)), _grassTile);
        }
    }
}
