using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFOController : MonoBehaviour
{
    public enum State { Idle, Fleeing }

    [Header("References")]
    public Transform player;

    [Header("Detection")]
    public float detectionRadius = 100f;

    [Header("Idle Wander")]
    public float wanderSpeed = 5f;
    public float wanderChangeInterval = 3f;

    [Header("Flee")]
    public float fleeSpeed = 700f;
    public float fleeDirectionChangeInterval = 5f;

    private Rigidbody rb;
    private State currentState = State.Idle;
    private Vector3 currentMoveDirection;
    private float directionTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        PickNewWanderDirection();
    }

    void FixedUpdate()
    {

        if (GameManager.Instance.IsGameOver) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // State transition check
        if (currentState == State.Idle && distanceToPlayer <= detectionRadius)
        {
            currentState = State.Fleeing;
            directionTimer = 0f; // force immediate new flee direction
        }

        directionTimer -= Time.fixedDeltaTime;

        if (currentState == State.Idle)
        {
            if (directionTimer <= 0f)
            {
                PickNewWanderDirection();
                directionTimer = wanderChangeInterval;
            }
            MoveInDirection(currentMoveDirection, wanderSpeed);
        }
        else if (currentState == State.Fleeing)
        {
            if (directionTimer <= 0f)
            {
                PickFleeDirection(distanceToPlayer);
                directionTimer = fleeDirectionChangeInterval;
            }
            MoveInDirection(currentMoveDirection, fleeSpeed);
        }

        Vector3 pos = transform.position;
        float boundary = 1100f; // MUST match the same value used on PlayerShip
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        pos.y = Mathf.Clamp(pos.y, -boundary, boundary);
        pos.z = Mathf.Clamp(pos.z, -boundary, boundary);
        transform.position = pos;
    }

    void PickNewWanderDirection()
    {
        currentMoveDirection = Random.insideUnitSphere.normalized;
    }

    void PickFleeDirection(float distanceToPlayer)
    {
        // Base direction: straight away from player
        Vector3 awayFromPlayer = (transform.position - player.position).normalized;

        // Add some randomness so it's not a dead-straight line
        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        currentMoveDirection = (awayFromPlayer + randomOffset).normalized;

        // If player got far enough away, go back to idle
        if (distanceToPlayer > detectionRadius * 1.5f)
        {
            currentState = State.Idle;
        }
    }

    void MoveInDirection(Vector3 direction, float speed)
    {
        rb.linearVelocity = direction * speed;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        Debug.Log($"Velocity being set: {rb.linearVelocity}, IsKinematic: {rb.isKinematic}");
    }
}