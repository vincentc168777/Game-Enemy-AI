using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System.Runtime.CompilerServices;
using System;


public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;

    private PathGrid grid;
    private Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    private Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
    private SimplePriorityQueue<Node> frontier = new SimplePriorityQueue<Node>();
    
    void Awake()
    {
        //gets pathGrid class
        grid = GetComponent<PathGrid>();
    }

    private void Update()
    {
        /*
         *  if (Input.GetKeyUp(KeyCode.Space))
            {
                findPath(player.position, enemy.position);
            }
        **/
          findPath(player.position, enemy.position);
        
        
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
        return path;
    }
    
}
