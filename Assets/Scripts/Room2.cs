using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private SpriteRenderer[] body;
    [SerializeField] private SpriteRenderer centerDecoration;

    public Doors[] roomDoors = new Doors[4];
}
