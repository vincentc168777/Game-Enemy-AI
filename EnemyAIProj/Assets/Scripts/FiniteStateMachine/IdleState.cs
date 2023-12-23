using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
 
    public override void EnterState()
    {
        Debug.Log("Enter Idle");
        
    }

    public override void UpdateState()
    {
        Debug.Log("Enter Idle Update");
    }

    public override void ExitState()
    {
        Debug.Log("Idle Exit");
    }

    
}
