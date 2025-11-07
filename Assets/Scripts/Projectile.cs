using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float damage = 25f;
    public float speed = 30f; // Increased speed for a better feel.
    public float lifetime = 3f;

    // This is a private variable to store the target we are seeking.
    private Transform target;
    
    // This is the Rigidbody component we will use for movement.
    private Rigidbody rb;

    // This is a public function that our TowerAI can call.
    // It gives the projectile its target.
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // We no longer set the velocity here. We will do it in Update.
        
        // Destroy the projectile after a set time to clean up missed shots.
        Destroy(gameObject, lifetime);
    }

    // We use FixedUpdate for physics calculations like movement.
    void FixedUpdate()
    {
        // If we don't have a target, just destroy ourselves to prevent errors.
        if (target == null)
        {
            Destroy(gameObject);
            return; // Stop running the rest of the code in this function.
        }

        // Calculate the direction from us to the target.
        Vector3 direction = target.position - rb.position;

        // Move the projectile in that direction.
        rb.velocity = direction.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit is our target.
        if (collision.transform == target)
        {
            // Try to get the EnemyHealth script from the target.
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            
            // If the enemy has a health script, deal damage.
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        // Destroy the projectile after it hits anything.
        Destroy(gameObject);
    }
}