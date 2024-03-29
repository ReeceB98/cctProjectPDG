using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    public SettingsManager settingsManager;
    [SerializeField] protected SimpleRandomWalkData randomWalkParameters;
    public HashSet<Vector2Int> floorPositions;
    public bool endLoop = false;

    private void Awake()
    {
        settingsManager = GameObject.FindAnyObjectByType<SettingsManager>();
        //floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        endLoop = false;
    }

    private void Start()
    {
        floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
    }

    private void Update()
    {
        if (endLoop)
        {
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        }
    }

    protected override void RunProceduralGeneration()
    {
        ///*HashSet<Vector2Int> */floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);

        /*if (endLoop)
        {
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        }*/
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData parameters, Vector2Int position)
    {
        position = new Vector2Int(0, 0);
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
