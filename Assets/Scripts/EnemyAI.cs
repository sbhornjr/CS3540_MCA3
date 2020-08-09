using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates { Idle, Patrol, Chase, Attack, Die };

    public FSMStates currentState;
    public float walkSpeed = 5;
    public float chaseDistance = 10;
    public float attackDistance = 5;
    public float attackRate = 2;
    public GameObject player;
    //public GameObject smokeEffect;
    public Transform enemyEyes;
    public float fieldOfView = 150f;
    public int damage = 20;
    public AudioClip attackSFX, dieSFX;

    EnemyHealth enemyHealth;
    int health;
    GameObject[] wanderPoints;
    Vector3 nextDestination;
    Animator animator;
    int currentDestinationIndex;
    float distanceToPlayer;
    float countdown = 0;
    bool isDead;
    NavMeshAgent agent;
    bool attack;
    LevelManager levelManager;
    float attackAnimLength = 2.266667f / 2;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        health = enemyHealth.currentHealth;

        if (health <= 0)
            currentState = FSMStates.Die;

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Die:
                UpdateDieState();
                break;
            default:
                break;
        }
    }

    public void Initialize()
    {
        currentState = FSMStates.Patrol;
        isDead = false;
        currentDestinationIndex = 0;
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();
        attack = true;
        levelManager = FindObjectOfType<LevelManager>();
        agent = GetComponent<NavMeshAgent>();
        GetNextWanderPoint();
    }

    void UpdatePatrolState()
    {
        animator.SetInteger("animState", 1);

        agent.stoppingDistance = 0;

        agent.speed = 2f;

        if (Vector3.Distance(transform.position, nextDestination) <= 2)
            GetNextWanderPoint();

        if (IsPlayerInClearFOV())
            currentState = FSMStates.Chase;

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState()
    {
        animator.SetInteger("animState", 2);

        agent.stoppingDistance = attackDistance;

        agent.speed = 6f;

        var playerPosition = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            countdown = 0;
            attack = true;
            currentState = FSMStates.Attack;
        }  
        else if (distanceToPlayer > chaseDistance)
        {
            GetNextWanderPoint();
            currentState = FSMStates.Patrol;
        }

        FaceTarget(playerPosition);

        agent.SetDestination(playerPosition);
    }

    void UpdateAttackState()
    {
        animator.SetInteger("animState", 3);
        var playerPosition = player.transform.position;

        if (distanceToPlayer <= attackDistance)
            currentState = FSMStates.Attack;
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
            currentState = FSMStates.Chase;
        else if (distanceToPlayer > chaseDistance)
            currentState = FSMStates.Patrol;

        FaceTarget(playerPosition);

        EnemyAttack();
    }

    void UpdateDieState()
    {
        animator.SetInteger("animState", 4);
        isDead = true;
        if (!isDead) AudioSource.PlayClipAtPoint(dieSFX, Camera.main.transform.position);
        Destroy(gameObject, 3);
    }

    private void OnDestroy()
    {
        LevelManager.numSkeletonsRemaining -= 1;
        levelManager.SetScoreText();
    }

    void GetNextWanderPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemyAttack()
    {
        countdown += Time.deltaTime;
       
        if (countdown >= attackAnimLength && !isDead)
        {
            countdown = 0.0f;
            attack = true;
        }
        else if (countdown >= attackAnimLength / 2 && !isDead && attack)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(Random.Range(damage - 10, damage + 10));
            attack = false;
            AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }

    bool IsPlayerInClearFOV()
    {
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        RaycastHit hit;
        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
