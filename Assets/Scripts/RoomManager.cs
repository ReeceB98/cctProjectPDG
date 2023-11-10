using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Prefab & Room Amount")]
    [SerializeField] private GameObject normalRoomPrefab;
    [SerializeField] private int minRooms = 0;
    [SerializeField] private int maxRooms = 0;
    [Space]
    [Header("Room Size")]
    [SerializeField] private int roomWidth = 20;
    [SerializeField] private int roomHeight = 12;
    [Space]
    [Header("Room Grid Size")]
    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;
    [Space]
    [Header("Other Settings")]
    [SerializeField] private List<GameObject> roomObjects = new List<GameObject>();
    [SerializeField] private int[,] roomGrid;
    [SerializeField] private int roomCount;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeX];
    }

    // Calculating individual grid positions in the grid index
    private Vector2Int GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;

        // returns the grid size for each point of the index for the rooms
        return new Vector2Int(roomWidth * (gridX - gridSizeX / 2),
            roomHeight * (gridY - gridSizeY / 2));
    }

    // To draw out the grid in scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Getting the postion for each cell in the grid and drawing it to screen
                Vector2Int position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(new Vector3(position.x, position.y), new Vector3(roomWidth, roomHeight, 1.0f));
            }
        }
    }
}
