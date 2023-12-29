
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private List<Node> attackPath;
    private bool isStateRunning = false;
    private Coroutine attackCoroutine;
    private bool foundAttackPath = false;
    private Vector3 currPos;
    public override void EnterState()
    {
        Debug.Log("Enter Attack");
    }

    public override void UpdateState(StateMachineManager stateMan)
    {
        Debug.Log("Enter Attack Update");

        isStateRunning = true;

        bool isPlayerInRange = stateMan.targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (!isPlayerInRange)
        {
            // randomly choose between idle or patrol
            ExitState(stateMan, pickRandomState(stateMan));
        }

        if (isStateRunning)
        {
            if (!foundAttackPath)
            {
                attackPath = findSpecificPath(stateMan, stateMan.getPlayerTrans());
                foundAttackPath = true;
                followPlayer(stateMan);
            }

            currPos = new Vector3(stateMan.getSelfTransform().position.x, 0, stateMan.getSelfTransform().position.z);
            if (currPos == attackPath[attackPath.Count - 1].getNodeWorldPos())
            {
                resetState();
            }

        }
    }
    public override void ExitState(StateMachineManager stateMan, BaseState newState)
    {
        isStateRunning = false;
        stopStateCoroutine();
        resetState();
        Debug.Log("Attack Exit");
        stateMan.changeState(newState);
    }

    private void followPlayer(StateMachineManager stateMan)
    {
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(followPath(stateMan, attackPath));
    }

    private BaseState pickRandomState(StateMachineManager stateMan)
    {
        int num = Random.Range(0, 2);
        BaseState newState;
        if (num == 1)
        {
            newState = stateMan.GetIdleState();
        }
        else
        {
            newState = stateMan.GetPatrolState();
        }
        return newState;
    }

    private void resetState()
    {
        attackPath = null;
        attackCoroutine = null;
        foundAttackPath = false;
    }

    private void stopStateCoroutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}
