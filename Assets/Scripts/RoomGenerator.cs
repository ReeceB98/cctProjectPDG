using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Room2))]
public class RoomGenerator : MonoBehaviour
{
    public static bool useSeed = false;
    public static readonly int seed = 1000;


    [SerializeField] private int amountToGenerate = 8;

    public Room2 roomPrefab1x1;

    // Determines the distance for each room generated in any direction
    public static readonly float prefabsDistance = 1;
    public readonly Vector2[] offsets = new Vector2[]
    {
        Vector2.up * prefabsDistance,
        Vector2.right * prefabsDistance,
        Vector2.down * prefabsDistance,
        Vector2.left * prefabsDistance,
    };

    public List<Room2> rooms;

    // Stores the rooms generated in one area
    // Shows if a room is being generated or not
    // Will be the starting room for the player
    private Transform roomsContainer;
    public bool generatingRooms;
    public Room2 generatorRoom;

    [SerializeField] private Text generatortext;
    [SerializeField] private float elapsedTime;

    private void Awake()
    {
        if (useSeed)
        {
            Random.InitState(seed);
        }

        rooms = new List<Room2>();
        generatorRoom = GetComponent<Room2>();
        roomsContainer = new GameObject("Rooms").transform;
    }

    IEnumerator Start()
    {
        StartCoroutine(GenerateRooms(roomPrefab1x1));
        while (generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }

        GenerateDoors();
        Room2 furthest = FindFurthestRoom();
        furthest.MarkAsBossRoom();
        SetPathToRoom(furthest);
    }

    private IEnumerator GenerateRooms(Room2 prefab)
    {
        generatingRooms = true;
        Room2.Directions dir;
        Vector2 offset;
        Vector2 last = transform.position;

        for (int i = 0; i < amountToGenerate; i++)
        {
            // Pick a random direction for a room to generate to
            // The distance between each room with the offset
            // The new room position
            dir = (Room2.Directions)Random.Range(0, 4);
            offset = offsets[(int)dir];
            Vector2 newRoomPos = last + offset;

            Room2 newRoom = Instantiate(prefab, newRoomPos, Quaternion.identity, roomsContainer);
            newRoom.gameObject.name = "Room " + rooms.Count;

            //yield return new WaitForFixedUpdate(); // Best performance
            yield return new WaitForSeconds(0.05f);  // Artifical Timer effect
            //yield return null;                    // Wait one frame

            elapsedTime += Time.deltaTime;
            generatortext.text = elapsedTime.ToString();

            last = newRoomPos;

            // To check if a new room collides with an existing which will then be destroyed
            if (newRoom.collision)
            {
                newRoom.gameObject.SetActive(false);
                Destroy(newRoom.gameObject);
                i--;
                continue;
            }
            // Add the new room to the list
            rooms.Add(newRoom);
            //Debug.Log($"Generated: {dir}");
        }

        generatingRooms = false;
        yield return null;
    }

    private void GenerateDoors()
    {
        generatorRoom.AssignAllNeighbours(offsets);

        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].AssignAllNeighbours(offsets);
        }
    }

    private Room2 FindFurthestRoom()
    {
        // Store the checked rooms in a list
        List<Room2> checkedRooms = new List<Room2>();

        int index = -1;
        float biggestDist = 0;
        for (int i = rooms.Count - 1; i >= 0; i--)
        {
            // Check if the check room contains a new room
            if (checkedRooms.Contains(rooms[i]))
            {
                continue;
            }

            // Condition to check how many rooms have been visted before to reach the target/boss
            List<Room2> path = rooms[i].GetShortestPathTo(generatorRoom);
            if (path == null)
            {
                Debug.LogError($"Paths error - {rooms[i].name} can't lead to generator");
                break;
            }

            // Store the biggest distance away from starting room
            int dist = path.Count;
            if (dist > biggestDist)
            {
                index = i;
                biggestDist = dist;
            }

            // Mark the visited room as checked rooms
            for (int j = 0; j < path.Count; j++)
            {
                if (!checkedRooms.Contains(path[j]))
                {
                    checkedRooms.Add(path[j]);
                }
            }

        }
        // Display the furthest room
        if (index != -1)
        {
            return rooms[index];
        }
        else
        {
            return null;
        }
        
    }

    // Iterating through steps to make a path to the boss room
    private void SetPathToRoom(Room2 r)
    {
        if (r)
        {
            List<Room2> steps = r.GetShortestPathTo(generatorRoom);
            //Debug.Log($"Steps: {steps.Count}\n{string.Join<Room2>("\n", steps.ToArray())}");
            for (int i = 1; i < steps.Count; i++)
            {
                steps[i].MarkAsPathToBossRoom();
            }
        }
        else
        {
            Debug.LogError("FindFurthestRoom() return null - can't set path");
        }
    }
}
