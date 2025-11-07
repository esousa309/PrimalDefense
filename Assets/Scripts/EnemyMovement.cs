using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Pathing")]
    public Transform[] path;
    
    [Header("Movement")]
    public float moveSpeed = 5f;

    private int waypointIndex = 0;

    void Start()
    {
        // We removed the line that sets the position here because the Spawner now does it for us.
    }

    void Update()
    {
        if (waypointIndex < path.Length)
        {
            Vector3 targetPosition = path[waypointIndex].position;
            float movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            // --- THIS IS THE FIX ---
            // The enemy has reached the end of the path.
            // Instead of just destroying it, let's get its health component...
            EnemyHealth health = GetComponent<EnemyHealth>();
            // ...and tell it to take a huge amount of damage to ensure it dies properly.
            if (health != null)
            {
                health.TakeDamage(9999); // This will call the Die() function and give currency.
            }
            else
            {
                // As a fallback, if there's no health script, just destroy it.
                Destroy(gameObject);
            }
        }
    }
}