using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Room2))]
public class RoomGenerator : MonoBehaviour
{
    public static bool useSeed = false;
    public static readonly int seed = 4555;
    [SerializeField] private int amountToGenerate = 8;

    public Room2 roomPrefab1x1;

    public static readonly float prefabsDistance = 1;
    public readonly Vector2[] offsets = new Vector2[]
    {
        Vector2.up * prefabsDistance,
        Vector2.right * prefabsDistance,
        Vector2.down * prefabsDistance,
        Vector2.left * prefabsDistance,
    };

    public List<Room2> rooms;

    private Transform roomsContainer;
    public bool generatingRooms;
    public Room2 generatorRoom;

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
            dir = (Room2.Directions)Random.Range(0, 4);
            offset = offsets[(int)dir];
            Vector2 newRoomPos = last + offset;
            //last = newRoomPos;

            Room2 newRoom = Instantiate(prefab, newRoomPos, Quaternion.identity, roomsContainer);
            newRoom.gameObject.name = "Room " + rooms.Count;
            //rooms.Add(newRoom);
            //yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.2f);
            //yield return null;

            last = newRoomPos;
            if (newRoom.collision)
            {
                newRoom.gameObject.SetActive(false);
                Destroy(newRoom.gameObject);
                i--;
                continue;
            }
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
        List<Room2> checkedRooms = new List<Room2>();

        int index = -1;
        float biggestDist = 0;
        for (int i = rooms.Count - 1; i >= 0; i--)
        {
            if (checkedRooms.Contains(rooms[i]))
            {
                continue;
            }

            List<Room2> path = rooms[i].GetShortestPathTo(generatorRoom);
            if (path == null)
            {
                Debug.LogError($"Paths error - {rooms[i].name} can't lead to generator");
                break;
            }

            int dist = path.Count;
            if (dist > biggestDist)
            {
                index = i;
                biggestDist = dist;
            }

            for (int j = 0; j < path.Count; j++)
            {
                if (!checkedRooms.Contains(path[j]))
                {
                    checkedRooms.Add(path[j]);
                }
            }

        }
        if (index != -1)
        {
            return rooms[index];
        }
        else
        {
            return null;
        }
        
    }

    private void SetPathToRoom(Room2 r)
    {
        if (r)
        {
            List<Room2> steps = r.GetShortestPathTo(generatorRoom);
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
