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

        //pick random walkable node on map
        float xPos = Random.Range(-(stateMan.getGrid().getGridXDimension()) / 2, (stateMan.getGrid().getGridXDimension()) / 2);
        float yPos = Random.Range(-(stateMan.getGrid().getGridYDimension()) / 2, (stateMan.getGrid().getGridYDimension()) / 2);
        Vector3 patrolDest = new Vector3(xPos, 1, yPos);

        Node destNode = stateMan.getGrid().worldPosToNode(patrolDest);
        if (destNode.getWalkable())
        {
            stateMan.getPathFinder().findPath(stateMan.getSelfTransform().position, patrolDest);
            patrolPath = stateMan.getPathFinder().getFoundPath();
        }
    }
    public override void ExitState(StateMachineManager stateMan)
    {
        Debug.Log("Patrol Exit");
        //call change state
    }


}
