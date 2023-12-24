using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    public override void EnterState()
    {
        Debug.Log("Enter Attack");
     
    }

    public override void UpdateState(StateMachineManager stateMan)
    {
        Debug.Log("Enter Attack Update");
        Debug.Log("bang bang");
        bool isPlayerInRange = targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (!isPlayerInRange)
        {
            ExitState(stateMan);
        }
    }
    public override void ExitState(StateMachineManager stateMan)
    {
        Debug.Log("Attack Exit");
        // randomly choose between idle or patrol
        int num = UnityEngine.Random.Range(0, 2);
        BaseState newState;
        if (num == 1)
        {
            newState = stateMan.GetIdleState();
        }
        else
        {
            newState = stateMan.GetPatrolState();
        }
        stateMan.changeState(newState);
    }


}
