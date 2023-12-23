using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    private IdleState idle;
    private PatrolState patrol;
    private AttackState attack;

    private BaseState currentState;
    // Start is called before the first frame update
    private void Awake()
    {
        idle = new IdleState();
        patrol = new PatrolState();
        attack = new AttackState();
    }
    void Start()
    {
        currentState = idle;
        idle.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public void setCurrentState(BaseState curr)
    {
        currentState = curr;
    }
   
}
