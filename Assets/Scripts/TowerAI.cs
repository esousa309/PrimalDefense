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

        // --- THIS IS THE REAL FIX ---
        // Get the direction from us to the enemy.
        Vector3 directionToEnemy = currentTarget.position - transform.position;
        
        // Calculate the rotation we need to look in that direction.
        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
        
        // This is the magic part: We create a new rotation that only uses the
        // Left-Right part (Y-axis) of the calculated rotation. We ignore the Up-Down.
        // The "10f" controls how fast the turret turns.
        Vector3 rotation = Quaternion.Lerp(turret.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
        turret.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        // --- END OF REAL FIX ---

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