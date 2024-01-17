using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Room2))]
public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private int amountToGenerate = 32;

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
    }

    private IEnumerator GenerateRooms(Room2 prefab)
    {
        generatingRooms = true;
        Room2.Directions dir = Room2.Directions.right;
        Vector2 offset = offsets[(int)dir] * prefabsDistance;
        Vector2 last = transform.position;

        for (int i = 0; i < amountToGenerate; i++)
        {
            Vector2 newRoomPos = last + offset;
            last = newRoomPos;

            Room2 newRoom = Instantiate(prefab, newRoomPos, Quaternion.identity, roomsContainer);
            rooms.Add(newRoom);
            yield return new WaitForSeconds(0.2f);
            //yield return null;
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
}
