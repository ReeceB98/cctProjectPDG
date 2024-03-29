using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;
    [SerializeField] private TileBase floorTile, wallTop;
    SimpleRandomWalkDungeonGenerator simpleRandom;
    CorridorFirstDungeonGenerator corridorFirstDungeonGenerator;
    SettingsManager settingsManager;

    [SerializeField] private Text generatorTimer;

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
        /*if (settingsManager.startBSP)
        {
            PaintTiles3(simpleRandom.floorPositions, floorTilemap, floorTile);
        }*/
    }
    private void Update()
    {
        if (settingsManager.startRW)
        {
            StartCoroutine(PaintTiles(simpleRandom.floorPositions, floorTilemap, floorTile));
            //PaintTiles2(simpleRandom.floorPositions, floorTilemap, floorTile);
        }
        else if (settingsManager.startBSP)
        {
            PaintTiles3(simpleRandom.floorPositions, floorTilemap, floorTile);
        }
        else
        {
            //Debug.Log("Coroutine Finished");
            //simpleRandom.endLoop = true;
        }
        Debug.Log(settingsManager.startRW);
    }

    public void PaintFloorTiles(HashSet<Vector2Int> floorPositions)
    {
        if (settingsManager.startRW)
        {
            PaintTiles(floorPositions, floorTilemap, floorTile);
            //PaintTiles2(floorPositions, floorTilemap, floorTile);
        }

        if (settingsManager.startBSP)
        {
            PaintTiles3(floorPositions, floorTilemap, floorTile);
        }
    }

    private IEnumerator PaintTiles(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        settingsManager.timeReamining += Time.deltaTime;
        generatorTimer.text = settingsManager.timeReamining.ToString();

        if (settingsManager.startRW)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
                yield return new WaitForSeconds(0.05f);
            }

            /*settingsManager.isTimerStarting = false;
            if (!settingsManager.isTimerStarting)
            {
                Time.timeScale = 0.0f;
            }*/

            settingsManager.startRW = false;
            simpleRandom.endLoop = true;
        }

        /*settingsManager.startRW = false;
        simpleRandom.endLoop = true;*/

    }

    /*public void PaintTiles2(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        settingsManager.timeReamining += Time.deltaTime;
        generatorTimer.text = settingsManager.timeReamining.ToString();
        if (settingsManager.startRW)
        {
            //settingsManager.timeReamining += Time.deltaTime;
            //generatorTimer.text = settingsManager.timeReamining.ToString();

            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
            }

            *//* settingsManager.isTimerStarting = false;
             if (!settingsManager.isTimerStarting)
             {
                 Time.timeScale = 0.0f;
             }*//*
            settingsManager.startRW = false;
            simpleRandom.endLoop = true;
        }

        *//*settingsManager.isTimerStarting = false;
        if (!settingsManager.isTimerStarting)
        {
            Time.timeScale = 0.0f;
        }*/

        /*settingsManager.startRW = false;
        simpleRandom.endLoop = true;*//*
    }*/

    public void PaintTiles3(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        settingsManager.timeReamining += Time.deltaTime;
        generatorTimer.text = settingsManager.timeReamining.ToString();
        if (settingsManager.startBSP)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
            }
        }

        //settingsManager.startBSP = false;
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
