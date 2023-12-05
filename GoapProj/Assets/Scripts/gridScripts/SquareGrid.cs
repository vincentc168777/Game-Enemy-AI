using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject groundPlane;
    [SerializeField] private Node[,] grid;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;

    [SerializeField] private int gridNodeSizeX;
    [SerializeField] private int gridNodeSizeY;
    [SerializeField] private float nodeDiameter;

    [SerializeField] private LayerMask unWalkableLayer;
    void Start()
    {
        //convert ground plane scale into width and length
        //uses 10 as unity uses 1 unit and the plane with scale 1 has 10 by 10 units
        gridWorldSize = new Vector2(groundPlane.transform.localScale.x * 10, groundPlane.transform.localScale.z * 10);
        nodeDiameter = nodeRadius * 2;
        // calculates number of nodes for the grid's x and y axis
        gridNodeSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridNodeSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        createGrid();
    }

    private void createGrid()
    {
        //fills grid with node starting from bottom left of the plane
        grid = new Node[gridNodeSizeX,gridNodeSizeY];
        Vector3 bottomLeft = transform.position - (Vector3.right * gridNodeSizeX / 2) - (Vector3.forward * gridNodeSizeY / 2);

        for(int x = 0; x < gridNodeSizeX; x++)
        {
            for(int y = 0; y < gridNodeSizeY; y++)
            {
                //x and y tells us how many nodes are already created. we multiply by the diameter and add radius to it to get the next node location
                Vector3 nodePosition = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //uses a invisible sphere with nodeRadius on node position to check if node collides with any obstacles
                bool walkable = !(Physics.CheckSphere(nodePosition, nodeRadius, unWalkableLayer));
                grid[x, y] = new Node(walkable, nodePosition);
            }
        }

    }

    //drawing the nodes for visualization
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridNodeSizeX, 1, gridNodeSizeY));

        if(grid != null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.getWalkable()) ? Color.white:Color.red;
                Gizmos.DrawCube(n.getNodePos(), Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
