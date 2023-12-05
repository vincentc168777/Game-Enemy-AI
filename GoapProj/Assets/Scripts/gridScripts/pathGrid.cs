using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class pathGrid : MonoBehaviour
{
    [SerializeField] private Node[,] grid;

    [SerializeField] private LayerMask unWalkableMask;
    [SerializeField] private GameObject groundplane;

    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private int gridNodeSizeX;
    [SerializeField] private int gridNodeSizeY;

    [SerializeField] private float nodeRadius;
    [SerializeField] private float nodeDiameter;
    [SerializeField] private 
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
                grid[x, y] = new Node(walkable, nodePos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        
        if (grid != null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = n.getWalkable() ? Color.white : Color.red;
                Gizmos.DrawCube(n.getNodePos(), Vector3.one * (nodeDiameter - .1f));
            }
        }
    }


}
