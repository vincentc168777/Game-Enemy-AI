using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Vector3 worldPos;
    private bool walkable;
    private int nodeXLoc;
    private int nodeYLoc;

    // costs hCost = dist cost goal, gCost cost from start
    private int hCost;
    private int gCost;

    public Node(bool walk, Vector3 wp, int nodeXLoc, int nodeYLoc)
    {
        worldPos = wp;
        walkable = walk;
        this.nodeXLoc = nodeXLoc;
        this.nodeYLoc = nodeYLoc;
    }

    public Vector3 getNodePos()
    {
        return worldPos;
    }

    public bool getWalkable()
    {
        return walkable;
    }

    public int getNodeXLoc()
    {
        return nodeXLoc;
    }

    public int getNodeYLoc()
    {
        return nodeYLoc;
    }

    public int getTotalNodeCost()
    {
        return hCost + gCost;
    }

    public void setHCost(int h)
    {
        hCost = h;
    }

    public void setGCost(int g)
    {
        gCost = g;
    }
}
