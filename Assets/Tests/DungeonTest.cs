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
        SceneManager.LoadScene(0);
        yield return null;

        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        bool generatorActive = generator.isActiveAndEnabled && generator.generatingRooms;

        Assert.AreEqual(true, generatorActive);
    }

    [UnityTest]
    public IEnumerator T02_Every_Room_Has_A_Door()
    {
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }

        bool foundBrokenRoom = false;
        List<Room2> generatedRooms = generator.rooms;
        for (int i = 0; i < generatedRooms.Count; i++)
        {
            if (generatedRooms[i].GetActiveDoorsAmount() == 0)
            {
                foundBrokenRoom = true;
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
        RoomGenerator generator = Object.FindObjectOfType<RoomGenerator>();

        while (generator.generatingRooms)
        {
            yield return new WaitForSeconds(0.05f);
        }

        bool foundBrokenConnection = false;
        List<Room2> generatedRooms = generator.rooms;
        for (int i = 0; i < generatedRooms.Count && !foundBrokenConnection; i++)
        {
            Room2 currentRoom = generatedRooms[i];
            Room2.Doors[] doors = generatedRooms[i].roomDoors;
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
}
