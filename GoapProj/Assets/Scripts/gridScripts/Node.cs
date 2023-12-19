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
    private int movePenalty;

    public Node(bool walk, Vector3 wp, int nodeXLoc, int nodeYLoc, int movePen)
    {
        worldPos = wp;
        walkable = walk;
        this.nodeXLoc = nodeXLoc;
        this.nodeYLoc = nodeYLoc;
        movePenalty = movePen;
    }

    public Vector3 getNodeWorldPos()
    {
        return worldPos;
    }

    public float getNodeWorldPosX()
    {
        return worldPos.x;
    }

    public float getNodeWorldPosZ()
    {
        return worldPos.z;
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

    public void setMovePenalty(int n)
    {
        movePenalty = n;
    }
    public int getmovePenalty()
    {
        return movePenalty;
    }
}
