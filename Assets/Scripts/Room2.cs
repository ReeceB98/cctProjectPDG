using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Room2 : MonoBehaviour
{
    // The directions the door can be placed from its neighbour
    public enum Directions
    {
        up,
        right,
        down,
        left
    }

    [System.Serializable]
    // The characteristics/variables for the door
    // This can all be found in the inspector
    public struct Doors
    {
        [HideInInspector]
        public bool active;

        public Directions direction;
        public SpriteRenderer spriteR;
        public Room2 leadsTo;
    }

    // Sprites for the pieces of the room
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer centerDecoration;

    // Door Array
    public Doors[] roomDoors = new Doors[4];

    [HideInInspector] public bool collision;

    private IEnumerator Start()
    {
        if (!GetComponent<RoomGenerator>())
        {
            body.color = new Color(0.0f, 125.0f, 125.0f, 255.0f);
            yield return new WaitForSeconds(0.3f);
            body.color = new Color(0.0f, 0.0f, 255.0f, 255.0f);
            yield return new WaitForSeconds(0.3f);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        collision = true;
    }

    public void AssignAllNeighbours(Vector2[] offsets)
    {
        for (int i = 0; i < roomDoors.Length; i++)
        {
            Vector2 offset = offsets[(int)roomDoors[i].direction];
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, offset, RoomGenerator.prefabsDistance);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider != null && hit[j].collider.gameObject != this.gameObject)
                {
                    roomDoors[i].leadsTo = hit[j].collider.GetComponentInChildren<Room2>();
                    roomDoors[i].active = true;
                    roomDoors[i].spriteR.enabled = true;
                }
            }
        }
    }

    public void MarkAsBossRoom()
    {
        centerDecoration.color = Color.red;
        centerDecoration.transform.localScale *= 1.2f;
    }

    public void MarkAsPathToBossRoom()
    {
        centerDecoration.color = Color.yellow;
    }

    public List<Room2> GetShortestPathTo(in Room2 target, List<Room2> steps = null, List<Room2> shortest = null)
    {
        bool canChangeShortest(in List<Room2> _steps, in List<Room2> _shortest)
        {
            return _shortest == null || _shortest.Count > _steps.Count;
        }

        if (steps == null)
        {
            steps = new List<Room2>();
        }
        steps.Add(this);
        if (shortest != null && steps.Count > shortest.Count)
        {
            return null;
        }

        // Check if the current target is the neighour
        for (int i = 0; i < roomDoors.Length; i++)
        {
            if (roomDoors[i].leadsTo == target)
            {
                if (canChangeShortest(steps, shortest))
                {
                    shortest = new List<Room2>(steps);
                }
            }
        }   
        
        // Tell neighbours to look for the target
        for (int j = 0; j < roomDoors.Length; j++)
        {
            Doors d = roomDoors[j];
            if (d.active && !steps.Contains(d.leadsTo))
            {
                // Check the shortest from neighbour is shorter than current shortest path
                List<Room2> result = d.leadsTo.GetShortestPathTo(target, new List<Room2>(steps), shortest);
                if (result != null)
                {
                    if (canChangeShortest(result, shortest))
                    {
                        shortest = result;
                    }
                }
            }
        }

        return shortest;

    }

    // Find the amount of doors active and return the output of the amount
    public int GetActiveDoorsAmount()
    {
        int output = 0;
        foreach (Doors d in roomDoors)
        {
            if (d.active)
            {
                output++;
            }
        }
        return output;
    }
}
