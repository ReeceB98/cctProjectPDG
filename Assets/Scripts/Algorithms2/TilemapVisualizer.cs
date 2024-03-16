using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;
    [SerializeField] private TileBase floorTile, wallTop;
    SimpleRandomWalkDungeonGenerator simpleRandom;
    SettingsManager settingsManager;


    /*private void Awake()
    {
        simpleRandom = GameObject.FindAnyObjectByType<SimpleRandomWalkDungeonGenerator>();
    }*/
    private void Start()
    {
        simpleRandom = GameObject.FindAnyObjectByType<SimpleRandomWalkDungeonGenerator>();
        settingsManager = GameObject.FindAnyObjectByType<SettingsManager>();
        //StartCoroutine(PaintTiles(simpleRandom.floorPositions, floorTilemap, floorTile));
    }
    private void Update()
    {
        if (settingsManager.startRW)
        {
            StartCoroutine(PaintTiles(simpleRandom.floorPositions, floorTilemap, floorTile));
        }
        else
        {
            Debug.Log("Coroutine Finished");
            simpleRandom.endLoop = true;
        }
    }

    public void PaintFloorTiles(HashSet<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private IEnumerator PaintTiles(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
            yield return new WaitForSeconds(0.1f);
        }

        settingsManager.startRW = false;
    }

    internal void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
