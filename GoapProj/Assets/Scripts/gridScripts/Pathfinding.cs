using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System.Runtime.CompilerServices;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;


public class Pathfinding : MonoBehaviour
{
    private PathGrid grid;
    private List<Node> foundpath;
    private Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    private Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
    private SimplePriorityQueue<Node> frontier = new SimplePriorityQueue<Node>();
    
    void Awake()
    {
        //gets pathGrid class
        grid = GetComponent<PathGrid>();
    }

    public List<Node> getFoundPath()
    {
        return foundpath;
    }

    public void findPath(Vector3 start, Vector3 goal)
    {
        Node s = grid.worldPosToNode(start);
        Node g = grid.worldPosToNode(goal);

        // clear info from previous pathfinding 
        cameFrom.Clear();
        costSoFar.Clear();
        frontier.Clear();

        //add start to frontier, costSpFar, and cameFrom
        cameFrom.Add(s, null);
        costSoFar.Add(s, 0);
        frontier.Enqueue(s, 0);

        while (frontier.Count != 0)
        {
            Node curr = frontier.Dequeue();

            if (curr == g)
            {
                /*
                 *  the loop terminates here so when we reverse the path, we can start
                 *  reversing from curr
                 */
                grid.setPath(retracePath(curr, s));
                foundpath = retracePath(curr, s);
                return;
            }

            foreach (Node next in grid.getNeighbors(curr))
            {
                if (!next.getWalkable())
                {
                    continue;
                }
                // get cost used to get dist from curr to next node
                int newCost = costSoFar[curr] + grid.getCost(curr, next);

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + grid.getCost(next, g);// get cost here is used to get heuristic(dist from goal)
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = curr;
                }
            }
        }    
    }

    //reverses path from goal
    private List<Node> retracePath(Node goal, Node pathStart)
    {
        List<Node> path = new List<Node>();
        Node current = goal;
        while(current != pathStart)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        //finally add startNode to the list then reverse the list
        path.Add(pathStart);
        path.Reverse();
        path = simplifyPath(path);
        return path;
    }
    
    //need to debug corner issue and the last node wont get added sometimes if enemy is in the same direction as player path
    private List<Node> simplifyPath(List<Node> pathlist)
    {
        List<Node> newL = new List<Node>();
        Vector2 oldDirection = Vector2.zero;

        for(int i = 1; i < pathlist.Count; i++)
        {
            Vector2 newDirection = new Vector2(pathlist[i].getNodeWorldPosX() - pathlist[i - 1].getNodeWorldPosX(), pathlist[i].getNodeWorldPosZ() - pathlist[i - 1].getNodeWorldPosZ());
            if(newDirection != oldDirection)
            {
                //we add the previous node to the one that changes direction
                newL.Add(pathlist[i - 1]);
            }
            oldDirection = newDirection;
        }
        // once we finish simplifying the path, we need to check if the enemy destination node is in the list
        Vector2 lastNodeDir = new Vector2(pathlist[pathlist.Count - 1].getNodeWorldPosX() - pathlist[pathlist.Count - 2].getNodeWorldPosX(), pathlist[pathlist.Count - 1].getNodeWorldPosZ() - pathlist[pathlist.Count - 2].getNodeWorldPosZ());
        if (lastNodeDir == oldDirection)
        {
            newL.Add(pathlist[pathlist.Count - 1]);
        }

        return newL;
    }

    


}
