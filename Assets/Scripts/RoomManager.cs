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
    [SerializeField] private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    [SerializeField] private int[,] roomGrid;
    [SerializeField] private int roomCount = 0;

    private void Start()
    {
        // Setting the size of the grid for the rooms
        roomGrid = new int[gridSizeX, gridSizeX];

        // Putting rooms in a queue for the rooms in FIFO order
        roomQueue = new Queue<Vector2Int>();

        // The starting room will be spawned in the middle of the grid
        Vector2Int initalRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initalRoomIndex);
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        // Add the starting room to the queue
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;

        // Instantiating the starting room prefab to the middle of the grid
        roomGrid[x, y] = 1;     // storing the value to the middle of the grid array
        roomCount++;
        GameObject initialRoom = Instantiate(normalRoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room - {roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    // Calculating individual grid positions in the grid index
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;

        // returns the grid size for each point of the index for the rooms
        return new Vector3(roomWidth * (gridX - gridSizeX / 2),
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
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1.0f));
            }
        }
    }
}
