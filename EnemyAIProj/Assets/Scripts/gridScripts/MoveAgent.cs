using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAgent : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    [SerializeField] private float moveSpeed;
    private Pathfinding pathFindClass;
    private List<Node> agentPath;
    // Start is called before the first frame update
    private void Awake()
    {
        pathFindClass = GetComponent<Pathfinding>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            pathFindClass.findPath(player.position, enemy.position);
            agentPath = pathFindClass.getFoundPath();
            StartCoroutine(followPath(agentPath, player));

        }
    }

    private IEnumerator followPath(List<Node> pathList, Transform movable)
    {
        int i = 0;
        Vector3 destination;
        while (i < pathList.Count)
        {
            destination = new Vector3(pathList[i].getNodeWorldPosX(), 1, pathList[i].getNodeWorldPosZ());

            rotateAgent(movable, destination);

            /** dont use character controller here. the move method in char controller uses
             *  a direction, not a destination
             */
            movable.position = Vector3.MoveTowards(movable.position, destination, moveSpeed * Time.deltaTime);

            if (movable.position == destination)
            {
                i++;
            }

            yield return null;
        }

    }

    private void rotateAgent(Transform rotatingObj, Vector3 target)
    {
        Vector3 lookVec = target - rotatingObj.position;
        Vector3 lookDirection = Vector3.RotateTowards(rotatingObj.forward, lookVec, moveSpeed * Time.deltaTime, 0);
        rotatingObj.rotation = Quaternion.LookRotation(lookDirection);
    }
}
