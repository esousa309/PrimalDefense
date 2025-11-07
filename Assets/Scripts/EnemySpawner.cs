using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    // This is the blueprint of the enemy we want to spawn.
    public GameObject enemyPrefab; 
    
    // This is the path the enemy should follow in this level.
    public Transform[] path;

    [Header("Spawner Settings")]
    public float spawnDelay = 2f; // Time in seconds between each spawn.

    private float spawnCountdown = 0f;

    // Update is called once per frame
    void Update()
    {
        // If the countdown has reached zero...
        if (spawnCountdown <= 0f)
        {
            // ...spawn an enemy!
            SpawnEnemy();
            // And reset the countdown.
            spawnCountdown = spawnDelay;
        }

        // Count down the timer.
        spawnCountdown -= Time.deltaTime;
    }

    void SpawnEnemy()
    {
        // Create a new enemy from our blueprint at the very start of the path.
        GameObject enemyInstance = Instantiate(enemyPrefab, path[0].position, Quaternion.identity);

        // Get the EnemyMovement script from the new enemy we just created.
        EnemyMovement movementScript = enemyInstance.GetComponent<EnemyMovement>();

        // THIS IS THE FIX: Tell the new enemy's movement script where the path is.
        movementScript.path = path;
    }
}