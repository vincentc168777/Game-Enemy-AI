using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public override void EnterState()
    {
        Debug.Log("Enter Patrol");
        
    }
    public override void UpdateState()
    {
        Debug.Log("Enter Patrol Update");
    }
    public override void ExitState()
    {
        Debug.Log("Patrol Exit");
    }

    
}
