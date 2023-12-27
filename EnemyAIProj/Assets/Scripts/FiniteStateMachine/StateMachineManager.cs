using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    private IdleState idle;
    private PatrolState patrol;
    private AttackState attack;

    private BaseState currentState;

    private PathGrid grid;
    private Pathfinding pathFinder;

    private CharacterController enemyCont;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject aStar;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float checkRange;
    // Start is called before the first frame update
    void Awake()
    {
        idle = GetComponent<IdleState>();
        patrol = GetComponent<PatrolState>();
        attack = GetComponent<AttackState>();

        enemyCont = GetComponent<CharacterController>();

        pathFinder = aStar.GetComponent<Pathfinding>();
        grid = aStar.GetComponent<PathGrid>();
    }
    void Start()
    {
        currentState = idle;
        idle.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void changeState(BaseState newState)
    {
        currentState = newState;
    }

    public bool targetInRange(Transform self, Transform target)
    {
        float dist = Vector3.Distance(self.position, target.position);
        if (dist < checkRange)
        {
            return true;
        }
        return false;
    }

    public Transform getPlayerTrans()
    {
        return player;
    }

    public Transform getSelfTransform()
    {
        return transform;
    }

    public IdleState GetIdleState()
    {
        return idle;
    }

    public AttackState GetAttackState()
    {
        return attack;
    }

    public PatrolState GetPatrolState()
    {
        return patrol;
    }

    public Pathfinding getPathFinder()
    {
        return pathFinder;
    }

    public PathGrid getGrid()
    {
        return grid;
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public CharacterController getEnemyController()
    {
        return enemyCont;
    }
   
}
