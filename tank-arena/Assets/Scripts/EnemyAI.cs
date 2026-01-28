using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private enum State { Patrol, Chase, Attack }
    private State currentState;

    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform turret;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [Header("AI Settings")]
    [SerializeField] float sightRange = 15f;
    [SerializeField] float attackRange = 8f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] float patrolTimeout = 5f;

    private float patrolTimer;
    private float nextFireTime;

    // Player
    private Transform player;

    // Patrol variables
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] float walkPointRange = 10f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("WARNING: No GameObject with the tag 'Player' found in the scene! Enemy is blind.");
        }

        currentState = State.Patrol;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Chase;
            }
        }
        else
        {
            currentState = State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol: Patroling(); break;
            case State.Chase: Chasing(); break;
            case State.Attack: Attacking(); break;
        }
    }

    bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > sightRange) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        Vector3 startPos = transform.position + Vector3.up * 0.125f;

        Debug.DrawRay(startPos, directionToPlayer * sightRange, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(startPos, directionToPlayer, out hit, sightRange, obstructionMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    // --- STATES ---

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            patrolTimer += Time.deltaTime;
            if (patrolTimer > patrolTimeout)
            {
                walkPointSet = false;
                patrolTimer = 0;
            }
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance + 0.5f)
            {
                walkPointSet = false;
                patrolTimer = 0;
            }
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(hit.position, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                walkPoint = hit.position;
                walkPointSet = true;
                patrolTimer = 0;
            }
        }
    }

    void Chasing()
    {
        agent.SetDestination(player.position);
    }

    void Attacking()
    {
        agent.SetDestination(transform.position);

        Vector3 targetPostition = new Vector3(player.position.x, turret.position.y, player.position.z);
        turret.LookAt(targetPostition);

        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            SoundManager.Instance.PlaySound(SoundManager.Instance.shootSound, 0.3f);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(walkPoint, 1f);
    }
}