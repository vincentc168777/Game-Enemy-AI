using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private List<Node> patrolPath;
    public override void EnterState()
    {
        Debug.Log("Enter Patrol");
        
    }
    public override void UpdateState(StateMachineManager stateMan)
    {
        Debug.Log("Enter Patrol Update");
        bool isPlayerInRange = targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (isPlayerInRange)
        {
            ExitState(stateMan);
            stateMan.changeState(stateMan.GetAttackState());
        }

        patrolPath = findPatrolPath(stateMan);
        //pick random walkable node on map
        
    }
    private List<Node> findPatrolPath(StateMachineManager stateMan)
    {
        float x = ((stateMan.getGrid().getGridXDimension()) / 2) - 1;
        float y = ((stateMan.getGrid().getGridYDimension()) / 2) - 1;
        float xPos = Random.Range(-x, x);
        float yPos = Random.Range(-y, y);
        Vector3 patrolDest = new Vector3(xPos, 1, yPos);
        Debug.Log(patrolDest);
        Node destNode = stateMan.getGrid().worldPosToNode(patrolDest);
        List<Node> destPath = null;
        if (destNode.getWalkable())
        {
            stateMan.getPathFinder().findPath(stateMan.getSelfTransform().position, patrolDest);
            destPath = stateMan.getPathFinder().getFoundPath();
        }
        return destPath;
    }


    public override void ExitState(StateMachineManager stateMan)
    {
        Debug.Log("Patrol Exit");
        //call change state
    }


}
