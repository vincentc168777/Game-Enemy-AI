using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    [SerializeField] private Vector3 worldPos;
    [SerializeField] private bool walkable;

    public Node(bool walk, Vector3 wp)
    {
        worldPos = wp;
        walkable = walk;
    }

    public Vector3 getNodePos()
    {
        return worldPos;
    }

    public bool getWalkable()
    {
        return walkable;
    }
}
