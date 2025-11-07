using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("Tower Settings")]
    // The part of the tower that will rotate to face the enemy.
    public Transform turret;
    
    // The detection range of the tower. We can see this in the Scene view.
    public float range = 10f;

    // A private variable to store the enemy we are currently targeting.
    private Transform currentTarget;

    // This is a special Unity function that draws gizmos in the Scene view.
    // It helps us visualize the tower's range without needing to play the game.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the color of our gizmo to red.
        Gizmos.DrawWireSphere(transform.position, range); // Draw a wireframe sphere around the tower.
    }

    // Update is called every single frame.
    void Update()
    {
        // If we don't have a target, try to find one.
        if (currentTarget == null)
        {
            FindTarget();
        }
        else // If we DO have a target...
        {
            // ...check if it's still in range.
            if (Vector3.Distance(transform.position, currentTarget.position) > range)
            {
                // If the target has moved out of range, lose focus and set it to null.
                currentTarget = null;
            }
            else
            {
                // If the target is still valid and in range, rotate our turret to look at it.
                turret.LookAt(currentTarget);
            }
        }
    }

    // This is our own custom function to find the closest enemy.
    void FindTarget()
    {
        // Find all game objects in the scene that have the "Enemy" tag.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        float closestDistance = Mathf.Infinity; // Start with an infinitely large distance.
        GameObject closestEnemy = null; // We haven't found a closest enemy yet.

        // Loop through every enemy we found.
        foreach (GameObject enemy in enemies)
        {
            // Calculate the distance from our tower to this specific enemy.
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // If this enemy is closer than the last one we checked...
            if (distanceToEnemy < closestDistance)
            {
                // ...update our variables to remember this enemy as the new "closest".
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        // After checking all enemies, if we found a closest one AND it's within our range...
        if (closestEnemy != null && closestDistance <= range)
        {
            // ...set it as our current target!
            currentTarget = closestEnemy.transform;
        }
    }
}