using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class DungeonTest
{
    [UnityTest]
    public IEnumerator T01_Generator_Active()
    {
        RoomGenerator.useSeed = true;

        // Load scene and wait one frame
        SceneManager.LoadScene(0);
        yield return null;

        // Get the room generator
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        bool generatorActive = generator.isActiveAndEnabled && generator.generatingRooms;

        Assert.AreEqual(true, generatorActive);
    }

    [UnityTest]
    public IEnumerator T02_Every_Room_Has_A_Door()
    {
        // Get the room generator
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        // Waiting for room generation
        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.05f);

        bool foundBrokenRoom = false;
        // Get the list of generated rooms
        List<Room2> generatedRooms = generator.rooms;
        for (int i = 0; i < generatedRooms.Count; i++)
        {
            // A condition to find any broken rooms that need fixing
            if (generatedRooms[i].GetActiveDoorsAmount() == 0)
            {
                foundBrokenRoom = true;
                Debug.Log(generatedRooms[i].name);
                yield return new WaitForSeconds(3);
                break;
            }
        }

        Assert.AreEqual(false, foundBrokenRoom);
    }

    [UnityTest]
    public IEnumerator T03_All_Active_Doors_Lead_Somewhere()
    {
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }

        bool foundBrokenDoor = false;
        List<Room2> generatedRooms = generator.rooms;
        for (int i = 0; i < generatedRooms.Count && !foundBrokenDoor; i++)
        {
            Room2.Doors[] doors = generatedRooms[i].roomDoors;
            for (int j = 0; j < doors.Length; j++)
            {
                if (doors[j].active && doors[j].leadsTo == null)
                {
                    foundBrokenDoor = true;
                    break;
                }
            }
        }

        Assert.AreEqual(false, foundBrokenDoor);
    }

    [UnityTest]
    public IEnumerator T04_Two_Way_Connections()
    {
        // Get room generator
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        // Waiting for room generation
        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }

        bool foundBrokenConnection = false;
        List<Room2> generatedRooms = generator.rooms;
        for (int i = 0; i < generatedRooms.Count && !foundBrokenConnection; i++)
        {
            // Get the current room and its doors
            Room2 currentRoom = generatedRooms[i];
            Room2.Doors[] doors = generatedRooms[i].roomDoors;

            // Check through all doors and neighbouring rooms are connected together
            for (int j = 0; j < doors.Length; j++)
            {
                if (doors[j].active)
                {
                    Room2.Doors[] neighbourDoors = doors[j].leadsTo.roomDoors;
                    bool canGoBack = false;
                    foreach (Room2.Doors d in neighbourDoors)
                    {
                        if (d.leadsTo == currentRoom)
                        {
                            canGoBack = true;
                        }
                    }
                    if (!canGoBack)
                    {
                        foundBrokenConnection = true;
                        Debug.Log($"{currentRoom.name} - no two-way connection");
                        break;
                    }
                }
            }
        }

        Assert.AreEqual(false, foundBrokenConnection);
    }

    [UnityTest]
    // Checking each room individually to see if they can be visited
    // If not the test will fail.
    public IEnumerator T05_Can_Visit_Every_Room()
    {
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }

        bool foundUnreachableRoom = false;
        List<Room2> generatedRooms = generator.rooms;
        Room2 generatorRoom = generator.generatorRoom;
        for (int i = 0; i < generatedRooms.Count && !foundUnreachableRoom; i++)
        {
            List<Room2> path = generatedRooms[i].GetShortestPathTo(generatorRoom);
            if (path == null)
            {
                foundUnreachableRoom = true;
                Debug.Log($"{generatedRooms[i].name} isn't connected to generator (start point)");
            }
        }

        Assert.AreEqual(false, foundUnreachableRoom);
    }

    [UnityTest]
    // A test to see if a seed in room generator can generate the same layout multiple times
    public IEnumerator T06_Same_Seed_Dungeons_Are_Equal()
    {
        string dungeonToString(RoomGenerator _generator)
        {
            // Getting the array of rooms generated
            // Create the list of neighbours
            Room2[] generatedRooms = _generator.rooms.ToArray();
            List<Room2> neighbours = new List<Room2>();


            for (int i = 0; i < generatedRooms.Length; i++)
            {
                // Get the room doors
                // And add to list to every neighbour room and the doors
                Room2.Doors[] doors = generatedRooms[i].roomDoors;
                for (int j = 0; j < doors.Length; j++)
                {
                    neighbours.Add(doors[j].leadsTo);
                }
            }
            return string.Join("\n", neighbours);
        }

        string generation1;
        string generation2;
        RoomGenerator.useSeed = true;

        // Generation test 1
        SceneManager.LoadScene(0);
        yield return null;

        RoomGenerator generator = Object.FindAnyObjectByType<RoomGenerator>();

        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }
        generation1 = dungeonToString(generator);
        //

        // Generation test 2
        SceneManager.LoadScene(0);
        yield return null;

        generator = Object.FindAnyObjectByType<RoomGenerator>();

        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }
        generation2 = dungeonToString(generator);
        //

        Assert.AreEqual(generation1, generation2);
    }
}
