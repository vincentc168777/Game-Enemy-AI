using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Node[,] grid;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;

    [SerializeField] private int gridNodeSizeX;
    [SerializeField] private int gridNodeSizeY;
    [SerializeField] private float nodeDiameter;

    [SerializeField] private LayerMask unWalkableLayer;
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        // calculates number of nodes for the grid
        gridNodeSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridNodeSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        createGrid();
    }

    private void createGrid()
    {
        grid = new Node[gridNodeSizeX,gridNodeSizeY];
        Vector3 bottomLeft = transform.position - (Vector3.right * gridNodeSizeX / 2) - (Vector3.forward * gridNodeSizeY / 2);

        for(int x = 0; x < gridNodeSizeX; x++)
        {
            for(int y = 0; y < gridNodeSizeY; y++)
            {
                Vector3 nodePosition = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(nodePosition, nodeRadius, unWalkableLayer));
                grid[x, y] = new Node(walkable, nodePosition);
            }
        }

    }

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
