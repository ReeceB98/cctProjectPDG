using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Room2 : MonoBehaviour
{
    public enum Directions
    {
        up,
        right,
        down,
        left
    }

    [System.Serializable]
    public struct Doors
    {
        [HideInInspector]
        public bool active;

        public Directions direction;
        public SpriteRenderer spriteR;
        public Room2 leadsTo;
    }

    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer centerDecoration;

    public Doors[] roomDoors = new Doors[4];

    [HideInInspector] public bool collision;

    private void Start()
    {
        if (!GetComponent<RoomGenerator>())
        {
            body.color = new Color(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
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
        
        for (int j = 0; j < roomDoors.Length; j++)
        {
            Doors d = roomDoors[j];
            if (d.active && !steps.Contains(d.leadsTo))
            {
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
