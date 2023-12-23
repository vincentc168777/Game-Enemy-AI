using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    
    public abstract void EnterState();
    //constanly checks for any condition that would result in exiting state
    public abstract void UpdateState();
    public abstract void ExitState();

    
    
}
