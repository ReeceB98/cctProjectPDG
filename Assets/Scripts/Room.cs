using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] private Vector2Int doorNumber; 

    public Vector2Int RoomIndex { get; set; }

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

    private void Update()
    {
        OpenDoor(doorNumber);
    }
}
