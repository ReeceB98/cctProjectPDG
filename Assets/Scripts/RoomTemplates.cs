using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms = null;
    public GameObject[] topRooms = null;
    public GameObject[] leftRooms = null;
    public GameObject[] rightRooms = null;
    public GameObject closedRoom = null;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnBoss;
    public GameObject boss;

    private void Update()
    {
        if (waitTime <= 0 && !spawnBoss)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnBoss = true;
                }
            }
        }
        else 
        {
            waitTime -= Time.deltaTime;
        }
    }
}
