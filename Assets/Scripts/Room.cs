using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Doors")]
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [Space]
    // Used to test if the doors open correctly
    [Header("Door Test"), SerializeField] private Vector2Int doorVector; 

    public Vector2Int RoomIndex { get; set; }
    
    // Opens the door if the vector is going in that direction
    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            Debug.Log("Top door is now open.");
            topDoor.SetActive(true);
        }

        if (direction == Vector2Int.down)
        {
            Debug.Log("Bottom door is now open.");
            bottomDoor.SetActive(true);
        }

        if (direction == Vector2Int.left)
        {
            Debug.Log("Left door is now open.");
            leftDoor.SetActive(true);
        }

        if (direction == Vector2.right)
        {
            Debug.Log("Right door is now open.");
            rightDoor.SetActive(true);
        }
    }

    /*private void Update()
    {
        // Testing if the doors update when entering door number
        OpenDoor(doorVector);
    }*/
}
