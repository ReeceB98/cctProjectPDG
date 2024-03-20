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
    CorridorFirstDungeonGenerator corridorFirstDungeonGenerator;
    SettingsManager settingsManager;



    /*private void Awake()
    {
        simpleRandom = GameObject.FindAnyObjectByType<SimpleRandomWalkDungeonGenerator>();
    }*/
    private void Start()
    {
        simpleRandom = GameObject.FindAnyObjectByType<SimpleRandomWalkDungeonGenerator>();
        corridorFirstDungeonGenerator = GameObject.FindAnyObjectByType<CorridorFirstDungeonGenerator>();
        settingsManager = GameObject.FindAnyObjectByType<SettingsManager>();
        //StartCoroutine(PaintTiles(simpleRandom.floorPositions, floorTilemap, floorTile));
    }
    private void Update()
    {
        if (settingsManager.startRW)
        {
            StartCoroutine(PaintTiles(simpleRandom.floorPositions, floorTilemap, floorTile));
        }
        if (settingsManager.startRWC)
        {
            StartCoroutine(PaintTiles2(corridorFirstDungeonGenerator.floorPositions, floorTilemap, floorTile));
        }
        else
        {
            Debug.Log("Coroutine Finished");
            //simpleRandom.endLoop = true;
        }
    }

    public void PaintFloorTiles(HashSet<Vector2Int> floorPositions)
    {
        if (settingsManager.startRW)
        {
            PaintTiles(floorPositions, floorTilemap, floorTile);
        }

        if (settingsManager.startRWC)
        {
            PaintTiles2(floorPositions, floorTilemap, floorTile);
        }
    }

    private IEnumerator PaintTiles(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        
        //if (settingsManager.startRW)
        //{
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
                yield return new WaitForSeconds(0.1f);
            }
        //}

        settingsManager.startRW = false;
        simpleRandom.endLoop = true;
    }

    public IEnumerator PaintTiles2(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        if (settingsManager.startRWC)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
                yield return new WaitForSeconds(0.1f);
            }
        }

        settingsManager.startRW = false;
        simpleRandom.endLoop = true;
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
