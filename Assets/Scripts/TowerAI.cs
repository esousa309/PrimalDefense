using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("Tower Settings")]
    public Transform turret;
    public float range = 10f;

    [Header("Firing Settings")] // NEW SECTION
    public float fireRate = 1f; // How many times we shoot per second.
    public GameObject projectilePrefab; // The blueprint for our projectile.
    public Transform firePoint; // The spot where projectiles will spawn.

    private Transform currentTarget;
    private float fireCountdown = 0f; // A timer to control our firing speed.

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Update()
    {
        // We moved the target finding logic to its own function to keep Update() clean.
        UpdateTarget();

        // If we don't have a target, do nothing.
        if (currentTarget == null)
            return;

        // If we have a target, rotate our turret to look at it.
        turret.LookAt(currentTarget);

        // --- FIRING LOGIC ---
        // If our countdown timer has reached zero...
        if (fireCountdown <= 0f)
        {
            // ...shoot a projectile!
            Shoot();
            // And reset the countdown timer based on our fire rate.
            fireCountdown = 1f / fireRate;
        }

        // Count down the timer every frame.
        fireCountdown -= Time.deltaTime;
    }

    // This is a new function to handle the shooting itself.
    void Shoot()
    {
        // Instantiate means "create a new instance of an object from a prefab".
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        // This is a temporary script to make the projectile move. We'll improve this later.
        // It gets the Rigidbody component of the new projectile and tells it to move forward.
        Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * 20f; // The "20f" is the projectile's speed.
        }
    }
    
    // We renamed FindTarget to UpdateTarget for clarity. The logic is the same.
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && closestDistance <= range)
        {
            currentTarget = closestEnemy.transform;
        }
        else
        {
            currentTarget = null;
        }
    }
}