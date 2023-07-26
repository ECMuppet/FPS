using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieAI : MonoBehaviour
{
    public float wanderSpeed = 5.0f;
    public float chaseSpeed = 10.0f;
    public float attackRange = 5.0f;
    public float chaseRange = 15.0f;

    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Find the player only if it exists in the scene with the "Player" tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            UnityEngine.Debug.Log("Found Player");
        }
        else
        {
            UnityEngine.Debug.Log("Player not found. Zombie will wander randomly.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Wander if the player is out of the chase range.
            if (distanceToPlayer > chaseRange)
            {
                Wander();
            }
            // Chase the player if they are within the chase range and attack range.
            else if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
        }
    }

    private void Wander()
    {
        // Check if the zombie has reached its destination.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Generate a random point on the NavMesh to set as the destination.
            Vector3 randomDirection = Random.insideUnitSphere * 10.0f;
            randomDirection += transform.position;
            UnityEngine.AI.NavMeshHit navHit;
            UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out navHit, 10.0f, UnityEngine.AI.NavMesh.AllAreas);
            Vector3 targetPosition = navHit.position;

            agent.SetDestination(targetPosition);
            agent.speed = wanderSpeed;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
    }
}