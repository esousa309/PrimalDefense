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

        // --- THIS IS THE FIX ---
        // Instead of looking directly at the target, we calculate a "flat" direction.
        
        // 1. Get the target's actual position.
        Vector3 targetPosition = currentTarget.position;
        
        // 2. This is the magic line: Force the Y-value of our target position
        //    to be the same as our turret's Y-value. This "flattens" the aim.
        targetPosition.y = turret.position.y;

        // 3. Now, make the turret look at this new, flattened position.
        turret.LookAt(targetPosition);
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