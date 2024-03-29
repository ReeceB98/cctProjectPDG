using System.Collections;
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
    [SerializeField] private bool generationComplete = false;

    private void Start()
    {
        // Setting the size of the grid for the rooms
        roomGrid = new int[gridSizeX, gridSizeX];

        // Putting rooms in a queue for the rooms in FIFO order
        roomQueue = new Queue<Vector2Int>();

        // The starting room will be spawned in the middle of the grid
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            // Rooming current room from the queue
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            // Generating rooms from neighbour cells
            /*TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));*/
            StartCoroutine(Delay(gridX, gridY));
        }
        else if (roomCount < minRooms)
        {
            Debug.Log("Room count was less than minimum amount of rooms. Trying again.");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation complete, {roomCount} room created");
            generationComplete = true;
        }
    }

    private IEnumerator Delay(int gridX, int gridY)
    {
        yield return new WaitForSecondsRealtime(3);
        TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
        yield return new WaitForSecondsRealtime(3);
        TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
        yield return new WaitForSecondsRealtime(3);
        TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
        yield return new WaitForSecondsRealtime(3);
        TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
    }

    // Starting room generation
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

    // Generate more rooms from the starting room
    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        // Room count higher than maximum rooms set stop generating
        if (roomCount >= maxRooms)
        {
            return false;
        }

        // Random seed to allow for room generation check
        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }

        // Check to see if room does not overlap existing room
        if (CountAdjacentRooms(roomIndex) > 1)
        {
            return false;
        }

        // Adding rooms to the queue
        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        // Instantiating more rooms to the grid
        GameObject newRoom = Instantiate(normalRoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"Room - {roomCount}";
        roomObjects.Add(newRoom);
        OpenDoors(newRoom, x, y);

        return true;
    }

    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        // Neighbours
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        // Determine which doors to open based on direction of rooms
        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            // Neighbouring room to the left
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }

        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            // Neighbouring room to the right
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }

        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            // Neighbouring room at the bottom
            newRoomScript.OpenDoor(Vector2Int.down);
            bottomRoomScript.OpenDoor(Vector2Int.up);
        }

        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            // Neighbouring room at the top
            newRoomScript.OpenDoor(Vector2Int.up);
            topRoomScript.OpenDoor(Vector2Int.down);
        }

    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);

        if (roomObject != null)
        {
            return roomObject.GetComponent<Room>();
        }

        return null;
    }

    // Counting the adjacent rooms from the current room generated
    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        // Checks condition(s) of room to see if it allowed to be adjacent to the current room
        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            count++;
        }

        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            count++;
        }

        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            count++;
        }

        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            count++;
        }

        return count;
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
