using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class pathGrid : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Node[,] grid;

    [SerializeField] private LayerMask unWalkableMask;
    [SerializeField] private GameObject groundplane;

    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private int gridNodeSizeX;
    [SerializeField] private int gridNodeSizeY;

    [SerializeField] private float nodeRadius;
    [SerializeField] private float nodeDiameter;
    
    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        //get node size from plane scale. Unity uses 1 unit, and the plane with scale 1 is 10 by 10
        gridWorldSize = new Vector2(groundplane.transform.localScale.x * 10, groundplane.transform.localScale.z * 10);
        //calc nodes required for x and y
        gridNodeSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridNodeSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        createGrid();
    }

    public List<Node> getNeighbors(Node n)
    {
        List<Node> neighbors = new List<Node>();

        /*explanaiton for the nested for loop
         *    ----|----|----
         * 1      |    |
         *    ----|----|----
         * 0      | n  |
         *    ----|----|----
         * -1     |    |
         *  y ____|____|____
         *   x -1   0     1
         */

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                /* this if statement skips center and diagonal nodes
                 * since the diagonal nodes' x and y are -1 or 1, we can check if sum of their abs val == 2
                 * to identify it as the diagonal node to skip it
                 */
                if((x == 0 && y == 0) || Mathf.Abs(x) + Math.Abs(y) == 2)
                {
                    continue;
                }

                int neighborX = n.getNodeXloc() + x;
                int neighborY = n.getNodeYLoc() + y;
                // check if neighbor x & y are in grid array range
                if ( (neighborX >= 0 && neighborX < gridNodeSizeX) && (neighborY >= 0 && neighborY < gridNodeSizeY))
                {
                    neighbors.Add(grid[neighborX, neighborY]);
                }
            }
            return neighbors;
        }


        return neighbors;
    }

    private void createGrid()
    {
        grid = new Node[gridNodeSizeX, gridNodeSizeY];

        Vector3 bottomLeft = transform.position - (Vector3.right * gridNodeSizeX / 2) - (Vector3.forward * gridNodeSizeY / 2);

        for(int x = 0; x < gridNodeSizeX; x++)
        {
            for(int y = 0; y < gridNodeSizeY; y++)
            {
                Vector3 nodePos = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(nodePos, nodeRadius, unWalkableMask));
                grid[x, y] = new Node(walkable, nodePos, x, y);
            }
        }
    }

    private Node worldPosToNode(Vector3 worldpos)
    {
        float xPercent = worldpos.x / gridNodeSizeX + .5f;
        float yPercent = worldpos.z / gridNodeSizeY + .5f;

        Mathf.Clamp01(xPercent);
        Mathf.Clamp01(yPercent);

        /*we clamp the result of gridNodeSizeX * xPercent & gridNodeSizeY * yPercent to their gridNodeSize - 1, as the array with
          the nodes go from 0 -> girdNodeSize - 1, so if player goes beyond, the x an y int will still be gridNodeSize - 1
            
          Not only that, if we place the player at the very edges, the percent calcluation will 
          return gridNodeSize(results in out of bounds for array), not gridNodeSize - 1, 
          which is why we clamp the result to gridNodeSize - 1 as well
        */

        int x = Mathf.FloorToInt(Mathf.Clamp(gridNodeSizeX * xPercent, 0, gridNodeSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp(gridNodeSizeY * yPercent, 0, gridNodeSizeY - 1));

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach(Node n in grid)
            {
                if(worldPosToNode(player.position) == n)
                {
                    Gizmos.color = Color.cyan;
                }
                else
                {
                    Gizmos.color = n.getWalkable() ? Color.white : Color.red;
                }
                Gizmos.DrawCube(n.getNodePos(), Vector3.one * (nodeDiameter - .1f));
            }
        }
    }


}
