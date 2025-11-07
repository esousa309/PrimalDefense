using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("Tower Settings")]
    public Transform turret;
    public float range = 10f;

    [Header("Firing Settings")]
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform currentTarget;
    private float fireCountdown = 0f;

    // This special Unity function draws gizmos in the Scene view to help us see the range.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Update is called every single frame.
    void Update()
    {
        // First, try to find a target.
        UpdateTarget();

        // If we don't have a target, do nothing else.
        if (currentTarget == null)
            return;

        // If we DO have a target, make our turret look at it.
        turret.LookAt(currentTarget);

        // This is our firing logic.
        // If our countdown timer has reached zero (or less)...
        if (fireCountdown <= 0f)
        {
            // ...shoot!
            Shoot();
            // And reset the timer.
            fireCountdown = 1f / fireRate;
        }

        // Make the timer count down every frame.
        fireCountdown -= Time.deltaTime;
    }

    // This function's only job is to create a new projectile.
    void Shoot()
    {
        // Create a new projectile from our blueprint, at our fire point's position and rotation.
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
    
    // This function looks for the closest enemy.
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