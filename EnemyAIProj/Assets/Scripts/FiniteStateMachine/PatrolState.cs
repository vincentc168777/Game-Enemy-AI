using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private List<Node> patrolPath;
    private Vector3 currPos;
    private bool foundPath = false;
    private bool stateRunning = false;
    private Coroutine pathCoroutine;
    public override void EnterState()
    {
        Debug.Log("Enter Patrol");
        
    }
    public override void UpdateState(StateMachineManager stateMan)
    {
        Debug.Log("Enter Patrol Update");

        stateRunning = true;

        bool isPlayerInRange = stateMan.targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (isPlayerInRange)
        {
            stateRunning = false;
            StopCoroutine(pathCoroutine);
            ExitState(stateMan);
            stateMan.changeState(stateMan.GetAttackState());
        }

        if (stateRunning)
        {
            if (!foundPath)
            {
                Debug.Log("enter found path if statement");
                patrolPath = findPath(stateMan);
                foundPath = true;

                pathCoroutine = StartCoroutine(followPath(stateMan, patrolPath));

            }

            currPos = new Vector3(stateMan.getSelfTransform().position.x, 0, stateMan.getSelfTransform().position.z);
            if (currPos == patrolPath[patrolPath.Count - 1].getNodeWorldPos())
            {
                Debug.Log("REACHED PATROL DESTINATION");
                resetState();
            }
        }
    }

    public override void ExitState(StateMachineManager stateMan)
    {
        resetState();
        Debug.Log("Patrol Exit");   
    }

    private void resetState()
    {
        pathCoroutine = null;
        patrolPath = null;  
        foundPath = false;
    }

    private List<Node> findPath(StateMachineManager stateMan)
    {
        //pick random walkable node on map
        float x = ((stateMan.getGrid().getGridXDimension()) / 2);
        float y = ((stateMan.getGrid().getGridYDimension()) / 2);
        float xPos = Random.Range(-x, x);
        float yPos = Random.Range(-y, y);

        Vector3 patrolDest = new Vector3(xPos, 1, yPos);

        Node destNode = stateMan.getGrid().worldPosToNode(patrolDest);
        List<Node> destPath = null;
        if (destNode.getWalkable())
        {
            stateMan.getPathFinder().findPath(stateMan.getSelfTransform().position, patrolDest);
            destPath = stateMan.getPathFinder().getFoundPath();
        }
        else
        {
            //if we get unwalkable node, just make a path list with one node: the one its already on.
            destPath = new List<Node>() { stateMan.getGrid().worldPosToNode(stateMan.getSelfTransform().position) };

        }

        return destPath;
    }

    private IEnumerator followPath(StateMachineManager stateMan, List<Node> path)
    {
        Debug.Log("enter coroutine");
        int i = 0;
        if (path != null)
        {
            while (i < path.Count)
            {
                Vector3 walkDest = new Vector3(path[i].getNodeWorldPosX(), 1, path[i].getNodeWorldPosZ());
                rotateObj(stateMan, walkDest);

                stateMan.getSelfTransform().position = Vector3.MoveTowards(stateMan.getSelfTransform().position, walkDest, stateMan.getMoveSpeed() * Time.deltaTime);

                if (stateMan.getSelfTransform().position == walkDest)
                {
                    i++;
                }
                yield return null;
            }

        }
    }

    private void rotateObj(StateMachineManager stateMan, Vector3 target)
    {
        Vector3 lookVec = target - stateMan.getSelfTransform().position;
        Vector3 lookDir = Vector3.RotateTowards(stateMan.getSelfTransform().forward, lookVec, stateMan.getMoveSpeed() * Time.deltaTime, 0);
        stateMan.getSelfTransform().rotation = Quaternion.LookRotation(lookDir);
    }
}
