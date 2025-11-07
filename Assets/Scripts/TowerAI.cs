using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("References")]
    // This is the part of the tower that will rotate left and right.
    public Transform turretRotator; 

    [Header("Tower Settings")]
    public float range = 10f;
    public float turnSpeed = 10f; // How fast the turret turns.

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

        // If we don't have a target, do nothing.
        if (currentTarget == null)
            return;

        // --- THIS IS THE FINAL FIX ---
        // We make the TurretRotator look at the enemy. This is the correct way.
        Vector3 direction = currentTarget.position - turretRotator.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(turretRotator.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        turretRotator.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        // --- END OF FIX ---

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
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