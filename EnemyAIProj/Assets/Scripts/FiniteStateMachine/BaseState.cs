using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public abstract void EnterState();
    //constanly checks for any condition that would result in exiting state
    public abstract void UpdateState(StateMachineManager stateMan);
    public abstract void ExitState(StateMachineManager stateMan);


    

    
}
