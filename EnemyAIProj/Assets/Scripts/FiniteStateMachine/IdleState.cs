
using UnityEngine;

public class IdleState : BaseState
{
    private bool isStateRunning = false;
    private bool isTargetAnglePicked = false;
    private float targetAngle;
    private float turnAngle;

    public override void EnterState()
    {
        Debug.Log("Enter Idle");
        
    }

    public override void UpdateState(StateMachineManager stateMan)
    {
        isStateRunning = true;

        Debug.Log("Enter Idle Update");

        bool isPlayerInRange = stateMan.targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (isPlayerInRange)
        {
            ExitState(stateMan, stateMan.GetAttackState());
        }

        if (isStateRunning)
        {
            /*
             * this if block finds a new target angle and turn direction
             */
            if (!isTargetAnglePicked)
            {
                targetAngle = getRandomTargetAngle();
                turnAngle = pickTurnDirection(stateMan);
                isTargetAnglePicked = true;
            }
            /*
             * keeps turning until it reaches target anngle, then finds a new target angle to rotate to
             */
            lookAround(stateMan, stateMan.getSelfTransform(), turnAngle, targetAngle);
   
        }

    }
    private void resetState()
    {
        isTargetAnglePicked=false;
        targetAngle = 0;
        turnAngle = 0;
    }

    public override void ExitState(StateMachineManager stateMan, BaseState newState)
    {
        isStateRunning = false;
        resetState();
        Debug.Log("Idle Exit");
        stateMan.changeState(newState);
        
    }

    private void lookAround(StateMachineManager stateMan, Transform objToRotate, float angleSpeed, float targetAngle)
    {
        //if we reach target angle, find a new target angle or switch to patrol
        if (Mathf.Abs(Mathf.Abs(objToRotate.rotation.eulerAngles.y) - targetAngle) > .5f)
        {
            objToRotate.Rotate(objToRotate.up, angleSpeed * Time.deltaTime);
        }
        else
        {
            resetState();
            if (pickDiffState())
            {
                ExitState(stateMan, stateMan.GetPatrolState());
            }
        }
           
        
    }

    private float getRandomTargetAngle()
    {
        float rand = Random.Range(0, 360);
        return rand;
    }

    /*
     * chooses to turn left or right based on positive or negative
     * ritate speed
     */
    private float pickTurnDirection(StateMachineManager stateMan)
    {
        float angleDir;
        int rand = Random.Range(0,2);
        if(rand == 1)
        {
            angleDir = stateMan.getIdleRotateSpeed();
        }
        else
        {
            angleDir = -stateMan.getIdleRotateSpeed();
        }
        return angleDir;
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
