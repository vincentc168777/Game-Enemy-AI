using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public abstract void EnterState(StateMachineManager stateMan);
    //constanly checks for any condition that would result in exiting state
    public abstract void UpdateState(StateMachineManager stateMan);
    public abstract void ExitState(StateMachineManager stateMan, BaseState newState);

    private protected List<Node> findRandomPath(StateMachineManager stateMan)
    {
        //pick random walkable node on map
        Node destNode = getRandomNode(stateMan);
        List<Node> destPath = null;
        if (destNode.getWalkable())
        {
            stateMan.getPathFinder().findPath(stateMan.getSelfTransform().position, destNode.getNodeWorldPos());
            destPath = stateMan.getPathFinder().getFoundPath();
        }
        else
        {
            //if we get unwalkable node, just make a path list with one node: the node the AI already on.
            destPath = new List<Node>() { stateMan.getGrid().worldPosToNode(stateMan.getSelfTransform().position) };

        }

        return destPath;
    }

    private protected List<Node> findSpecificPath(StateMachineManager stateMan, Transform target)
    {
        stateMan.getPathFinder().findPath(stateMan.getSelfTransform().position, target.position);
        List<Node> speciPath = stateMan.getPathFinder().getFoundPath();
        return speciPath;
    }

    private protected Node getRandomNode(StateMachineManager stateMan)
    {
        float x = ((stateMan.getGrid().getGridXDimension()) / 2);
        float y = ((stateMan.getGrid().getGridYDimension()) / 2);
        float xPos = Random.Range(-x, x);
        float yPos = Random.Range(-y, y);

        Vector3 patrolDest = new Vector3(xPos, 1, yPos);

        Node randNode = stateMan.getGrid().worldPosToNode(patrolDest);

        return randNode;
    }

    private protected IEnumerator followPath(StateMachineManager stateMan, List<Node> path)
    {
        int i = 0;
        if (path != null)
        {
            while (i < path.Count)
            {
                Vector3 walkDest = new Vector3(path[i].getNodeWorldPosX(), 1, path[i].getNodeWorldPosZ());
                rotateObjTowards(stateMan, walkDest);

                stateMan.getSelfTransform().position = Vector3.MoveTowards(stateMan.getSelfTransform().position, walkDest, stateMan.getMoveSpeed() * Time.deltaTime);

                if (stateMan.getSelfTransform().position == walkDest)
                {
                    i++;
                }
                yield return null;
            }

        }
    }

    private protected IEnumerator followPathNoTurning(StateMachineManager stateMan, List<Node> path)
    {
        int i = 0;
        if (path != null)
        {
            while (i < path.Count)
            {
                Vector3 walkDest = new Vector3(path[i].getNodeWorldPosX(), 1, path[i].getNodeWorldPosZ());

                stateMan.getSelfTransform().position = Vector3.MoveTowards(stateMan.getSelfTransform().position, walkDest, stateMan.getMoveSpeed() * Time.deltaTime);

                if (stateMan.getSelfTransform().position == walkDest)
                {
                    i++;
                }
                yield return null;
            }

        }
    }

    private protected void rotateObjTowards(StateMachineManager stateMan, Vector3 target)
    {
        Vector3 lookVec = target - stateMan.getSelfTransform().position;
        Vector3 lookDir = Vector3.RotateTowards(stateMan.getSelfTransform().forward, lookVec, stateMan.getMoveSpeed() * Time.deltaTime, 0);
        stateMan.getSelfTransform().rotation = Quaternion.LookRotation(lookDir);
    }

   

}
