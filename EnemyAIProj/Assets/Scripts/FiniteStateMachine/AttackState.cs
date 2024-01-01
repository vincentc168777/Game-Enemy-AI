
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private List<Node> attackPath;
    private bool isStateRunning = false;
    private Coroutine attackCoroutine;
    private Coroutine shootingCoroutine;
    private bool foundAttackPath = false;
    private Vector3 currPos;
    public override void EnterState(StateMachineManager stateMan)
    {
        
        shootingCoroutine = StartCoroutine(shootBalls(stateMan));
    }

    public override void UpdateState(StateMachineManager stateMan)
    {
        

        isStateRunning = true;

        bool isPlayerInRange = stateMan.targetInRange(stateMan.getSelfTransform(), stateMan.getPlayerTrans());
        if (!isPlayerInRange)
        {
            // randomly choose between idle or patrol
            ExitState(stateMan, pickRandomState(stateMan));
        }

        if (isStateRunning)
        {
            //when attackin, always face player to shoot bullet in right direction
            rotateObjTowards(stateMan, stateMan.getPlayerTrans().position);

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
        StopAllCoroutines();
        resetState();
        
        stateMan.changeState(newState);
    }

    private void followPlayer(StateMachineManager stateMan)
    {
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(followPathNoTurning(stateMan, attackPath));
        
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

    private IEnumerator shootBalls(StateMachineManager stateMan)
    {
        while (true)
        {
            Instantiate(stateMan.getBullet(), stateMan.getBulletSpawn().position, stateMan.getSelfTransform().rotation);
            yield return new WaitForSeconds(.5f);
        }
    }

    private void resetState()
    {
        attackPath = null;
        foundAttackPath = false;
        attackCoroutine = null;
        shootingCoroutine = null;
    }

}
