using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
 
    public override void EnterState()
    {
        Debug.Log("Enter Idle");
        
    }

    public override void UpdateState(StateMachineManager stateMan)
    {
        Debug.Log("Enter Idle Update");
        bool isPlayerInRange = stateMan.targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (isPlayerInRange)
        {
            ExitState(stateMan);
            stateMan.changeState(stateMan.GetAttackState());
        }
    }



    public override void ExitState(StateMachineManager stateMan)
    {
        Debug.Log("Idle Exit");
    }

    
}
