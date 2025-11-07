using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // [Header("Pathing")]
    // This is the list of points our enemy will follow.
    // We will drag our Waypoint objects onto this in the Unity Editor.
    public Transform[] path; 
    
    // [Header("Movement")]
    // This is how fast our enemy will move. We can change this in the Editor.
    public float moveSpeed = 5f;

    // This is a private variable to keep track of which waypoint we are currently moving towards.
    private int waypointIndex = 0;

    // Start is called once, right at the beginning of the game.
    void Start()
    {
        // Set the enemy's starting position to be the first waypoint's position.
        transform.position = path[waypointIndex].position;
    }

    // Update is called every single frame. This is where all the movement logic goes.
    void Update()
    {
        // First, check if there are any waypoints left in our path.
        if (waypointIndex < path.Length)
        {
            // Get the position of the waypoint we are moving towards.
            Vector3 targetPosition = path[waypointIndex].position;

            // Calculate the distance to move in this frame.
            // Time.deltaTime makes our movement smooth and independent of the computer's speed.
            float movementThisFrame = moveSpeed * Time.deltaTime;

            // Move our enemy towards the target position.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);

            // Check if we have reached the target waypoint.
            if (transform.position == targetPosition)
            {
                // If we have, increment our index to move towards the next one.
                waypointIndex++;
            }
        }
        else
        {
            // If there are no waypoints left, the enemy has reached the end.
            // For now, let's just destroy it.
            Destroy(gameObject);
        }
    }
}