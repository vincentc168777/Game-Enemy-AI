using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;


public class Pathfinding : MonoBehaviour
{
    private pathGrid grid;

    private Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    private Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
    private SimplePriorityQueue<Node> pQueue = new SimplePriorityQueue<Node>();
    
    private void Awake()
    {
        //gets pathGrid class
        grid = GetComponent<pathGrid>();
    }

    private void findPath(Node start, Node goal)
    {

    }
}
