using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("References")]
    public Transform turretRotator; 

    [Header("Tower Settings")]
    public float range = 10f;
    public float turnSpeed = 10f;

    [Header("Firing Settings")]
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform currentTarget;
    private float fireCountdown = 0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Update()
    {
        UpdateTarget();

        if (currentTarget == null)
            return;

        // --- THE AIMING FIX ---
        // We will make the rotator look directly at the target. This provides the correct 3D angle.
        Vector3 direction = currentTarget.position - turretRotator.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        // We smoothly rotate towards the target. This looks much better.
        turretRotator.rotation = Quaternion.Lerp(turretRotator.rotation, lookRotation, Time.deltaTime * turnSpeed);
        // --- END OF AIMING FIX ---


        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        // Create the projectile from the blueprint.
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        // --- THE "SMART PROJECTILE" FIX ---
        // Get the Projectile script from the object we just created.
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        // If we successfully found the script...
        if (projectile != null)
        {
            // ...call its Seek function and give it our current target.
            projectile.Seek(currentTarget);
        }
        // --- END OF FIX ---
    }
    
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