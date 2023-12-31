
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private List<Node> patrolPath;
    private Vector3 currPos;
    private bool foundPath = false;
    private bool stateRunning = false;
    private Coroutine pathCoroutine;
    public override void EnterState(StateMachineManager stateMan)
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
            ExitState(stateMan, stateMan.GetAttackState());
        }

        if (stateRunning)
        {
            if (!foundPath)
            {
                patrolPath = findRandomPath(stateMan);
                foundPath = true;

                pathCoroutine = StartCoroutine(followPath(stateMan, patrolPath));

            }

            //when finished patrolling, choose to keep patrolling or stay idle
            currPos = new Vector3(stateMan.getSelfTransform().position.x, 0, stateMan.getSelfTransform().position.z);
            if (currPos == patrolPath[patrolPath.Count - 1].getNodeWorldPos())
            {
                resetState();
                
                if (pickDiffState())
                {
                    ExitState(stateMan, stateMan.GetIdleState());
                }
                
            }
        }
    }

    public override void ExitState(StateMachineManager stateMan, BaseState newState)
    {
        stateRunning = false;
        stopStateCoroutine();
        resetState();
        stateMan.changeState(newState);
        Debug.Log("Patrol Exit");   
    }

    private void resetState()
    {
        pathCoroutine = null;
        patrolPath = null;  
        foundPath = false;
    }

    private void stopStateCoroutine()
    {
        if (pathCoroutine != null)
        {
            StopCoroutine(pathCoroutine);
        }
    }

    private bool pickDiffState()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
