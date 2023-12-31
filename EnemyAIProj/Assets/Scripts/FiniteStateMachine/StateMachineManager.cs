
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    private IdleState idle;
    private PatrolState patrol;
    private AttackState attack;

    private BaseState currentState;

    private PathGrid grid;
    private Pathfinding pathFinder;


    [SerializeField] private Transform player;
    [SerializeField] private GameObject aStar;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float checkRange;
    [SerializeField] private float idleRotateSpeed;

    [SerializeField] private Transform bulletSpawnLoc;
    [SerializeField] private GameObject bullet;
    // Start is called before the first frame update
    void Awake()
    {
        idle = GetComponent<IdleState>();
        patrol = GetComponent<PatrolState>();
        attack = GetComponent<AttackState>();

        pathFinder = aStar.GetComponent<Pathfinding>();
        grid = aStar.GetComponent<PathGrid>();
    }
    void Start()
    {
        currentState = idle;
        idle.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void changeState(BaseState newState)
    {
        currentState = newState;
        newState.EnterState(this);
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
    #region getting transforms
    public Transform getPlayerTrans()
    {
        return player;
    }

    public Transform getSelfTransform()
    {
        return transform;
    }

    public Transform getBulletSpawn()
    {
        return bulletSpawnLoc;
    }
    #endregion

    #region get states
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
    #endregion

    #region pathfinding
    public Pathfinding getPathFinder()
    {
        return pathFinder;
    }

    public PathGrid getGrid()
    {
        return grid;
    }
    #endregion

    #region getting speed
    public float getMoveSpeed()
    {
        return moveSpeed;
    }
    public float getIdleRotateSpeed()
    {
        return idleRotateSpeed;
    }
    #endregion

    #region getting gameObjects
    public GameObject getBullet()
    {
        return bullet;
    }
    #endregion
}
