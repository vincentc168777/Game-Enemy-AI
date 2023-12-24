using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{

    
    public abstract void EnterState();
    //constanly checks for any condition that would result in exiting state
    public abstract void UpdateState(StateMachineManager stateMan);
    public abstract void ExitState(StateMachineManager stateMan);

    public bool targetInRange(Transform self, Transform target)
    {
        float dist = Vector3.Distance(self.position, target.position);
        if (dist < 2f)
        {
            return true;
        }
        return false;
    }

}
