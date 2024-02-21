using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField] [Range(0.1f, 1)] private float roomPercent = 0.8f;
    [SerializeField] public SimpleRandomWalkData roomGenerationParameters;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositons = new HashSet<Vector2Int>();

        CreateCorridors(floorPositons);

        tilemapVisualizer.PaintFloorTiles(floorPositons);
        WallGenerator.CreateWalls(floorPositons, tilemapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositons)
    {
        var currentPosition = startPosition;
        
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            floorPositons.UnionWith(corridor);
        }
    }
}
