using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    

    public override void EnterState()
    {
        Debug.Log("Enter Attack");
        
    }

    public override void UpdateState()
    {
        Debug.Log("Enter Attack Update");
    }
    public override void ExitState()
    {
        Debug.Log("Attack Exit");
    }

    
}
