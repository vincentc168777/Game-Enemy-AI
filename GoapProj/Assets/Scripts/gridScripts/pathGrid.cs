using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Node[,] grid;

    [SerializeField] private List<RegionType> regions;

    [SerializeField] private LayerMask walkableLayers;
    private Dictionary<int, int> movePenDict = new Dictionary<int, int>();

    [SerializeField] private LayerMask unWalkableMask;
    [SerializeField] private GameObject groundplane;

    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private int gridNodeSizeX;
    [SerializeField] private int gridNodeSizeY;

    [SerializeField] private float nodeRadius;
    [SerializeField] private float nodeDiameter;

    private List<Node> generatedPath;
    
    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        //get node size from plane scale. Unity uses 1 unit, and the plane with scale 1 is 10 by 10
        gridWorldSize = new Vector2(groundplane.transform.localScale.x * 10, groundplane.transform.localScale.z * 10);
        //calc nodes required for x and y
        gridNodeSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridNodeSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        /* takes the binary value of each layer and adds it to walkableLayers using bitwise or
         * each layer's binary has 32 bits, each zero representing layernumber first zero  mean index0
         * index 9 is 10_0000_0000
         * so 10_0000_0001 means that we have layer index 0 and 9
         */
        foreach(RegionType r in regions)
        {
            walkableLayers |= r.getRegionLayer().value;
            int layerIndex = (int) Mathf.Log(r.getRegionLayer().value, 2);
            // dictionary so we dont have to loop through array of layers to find the one we hit
            movePenDict.Add(layerIndex, r.getMovePenalty());
        }

        createGrid();
    }

    private void createGrid()
    {
        grid = new Node[gridNodeSizeX, gridNodeSizeY];

        Vector3 bottomLeft = transform.position - (Vector3.right * gridNodeSizeX / 2) - (Vector3.forward * gridNodeSizeY / 2);

        for (int x = 0; x < gridNodeSizeX; x++)
        {
            for (int y = 0; y < gridNodeSizeY; y++)
            {
                Vector3 nodePos = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(nodePos, nodeRadius, unWalkableMask));

                /**
                 * ray will come from above the node and point down.
                 * the hit variable will have the resulting layer that was hit by the ray.
                 * walkable has all the layers that can possibly be hit.
                 */
                int movePen = 0;
                
                Ray r = new Ray(nodePos + (Vector3.up * 30), Vector3.down);
                RaycastHit hit;
                if(Physics.Raycast(r, out hit, 100f, walkableLayers))
                {
                    movePen = movePenDict[hit.collider.gameObject.layer];
                }

                grid[x, y] = new Node(walkable, nodePos, x, y, movePen);
            }
        }

        boxBlur(3);
    }

    private void boxBlur(int blurScale)
    {
        int kernalDimensions = 2 * blurScale - 1;
        int kernalRadius = (kernalDimensions - 1) / 2;

        int[,] horziontal = new int[gridNodeSizeX, gridNodeSizeX];
        int[,] vertical = new int[gridNodeSizeX, gridNodeSizeX];

        //calculate horizonatal first
        for(int y = 0; y < gridNodeSizeY; y++)
        {
            // find sum for first cell of horz array
            for(int x = -kernalRadius; x <= kernalRadius; x++)
            {
                int kernalIndex = Mathf.Clamp(x, 0, kernalRadius);
                horziontal[0, y] += grid[kernalIndex, y].getmovePenalty();
            }

            for (int x = 1; x < gridNodeSizeX; x++)
            {
                int addIndex = Mathf.Clamp(x + kernalRadius, 0, gridNodeSizeX - 1);
                int subtractIndex = Mathf.Clamp(x - kernalRadius - 1, 0, gridNodeSizeX);

                // value for array cell = previous sum - previous leftmost number in kernel + new rightmost number in kernel
                horziontal[x, y] = horziontal[x - 1, y] - grid[subtractIndex, y].getmovePenalty() + grid[addIndex, y].getmovePenalty();
            }
            // then calculate the numbers for the rest of the row

        }

        // do the same for vertical 
        for (int x = 0; x < gridNodeSizeX; x++)
        {
            for(int y = -kernalRadius; y <= kernalRadius; y++)
            {
                int kernalYIndex = Mathf.Clamp(y, 0, kernalRadius);
                vertical[x, 0] += horziontal[x, kernalYIndex];
            }

            for (int y = 1; y < gridNodeSizeY; y++)
            {
                int addIndex = Mathf.Clamp(y, 0, gridNodeSizeY - 1);
                int subIndex = Mathf.Clamp(y - kernalRadius - 1, 0, gridNodeSizeY);

                vertical[x, y] = vertical[x, y - 1] - horziontal[x, subIndex] + horziontal[x, addIndex];

                /* cast to float to prevent integer division from rounding down quotient
                 * we dont want the quotient to be rounded down, we want the decimal values so we can use RoundToInt 
                 * to find the best newPenalty value based on how close the decimal is to an integer
                 */
                int newPenalty = Mathf.RoundToInt( (float) vertical[x, y] / (kernalDimensions * kernalDimensions));
                grid[x, y].setMovePenalty(newPenalty);
            }
        }

    }

    public Node worldPosToNode(Vector3 worldpos)
    {
        float xPercent = worldpos.x / gridNodeSizeX + .5f;
        float yPercent = worldpos.z / gridNodeSizeY + .5f;

        Mathf.Clamp01(xPercent);
        Mathf.Clamp01(yPercent);

        /*we clamp the result of gridNodeSizeX * xPercent & gridNodeSizeY * yPercent to their gridNodeSize - 1, as the array with
          the nodes go from 0 -> gridNodeSize - 1, so if player goes beyond, the x an y int will still be gridNodeSize - 1
            
          Not only that, if we place the player at the very edges, the percent calcluation will 
          return gridNodeSize(results in out of bounds for array), not gridNodeSize - 1, 
          which is why we clamp the result to gridNodeSize - 1 as well
        */

        int x = Mathf.FloorToInt(Mathf.Clamp(gridNodeSizeX * xPercent, 0, gridNodeSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp(gridNodeSizeY * yPercent, 0, gridNodeSizeY - 1));

        return grid[x, y];
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
                if ((x == 0 && y == 0) || Mathf.Abs(x) + Math.Abs(y) == 2)
                {
                    continue;
                }

                int neighborX = n.getNodeXLoc() + x;
                int neighborY = n.getNodeYLoc() + y;
                // check if neighbor x & y are in grid array range
                if ((neighborX >= 0 && neighborX < gridNodeSizeX) && (neighborY >= 0 && neighborY < gridNodeSizeY))
                {
                    neighbors.Add(grid[neighborX, neighborY]);
                }
            }
   
        }

        return neighbors;
    }

    

    //for getting cost from start and heuristic
    public int getCost(Node start, Node end)
    {
        return Mathf.Abs(start.getNodeXLoc() - end.getNodeXLoc()) + Mathf.Abs(start.getNodeYLoc() - end.getNodeYLoc());
    }

    public void setPath(List<Node> inputpathList)
    {
        generatedPath = inputpathList;
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {

            foreach(Node n in grid)
            {
                Gizmos.color = n.getWalkable() ? Color.white : Color.red;
                if (generatedPath != null)
                {
                    if (generatedPath.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                
                
                
                Gizmos.DrawCube(n.getNodeWorldPos(), Vector3.one * (nodeDiameter - .1f));
            }
        }
    }


}
