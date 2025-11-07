using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float damage = 25f; // How much damage this projectile deals.
    public float speed = 20f; // How fast the projectile moves.
    public float lifetime = 3f; // How long the projectile exists before destroying itself.

    private Rigidbody rb;

    // Start is called before the first frame update.
    void Start()
    {
        // Get the Rigidbody component from this GameObject.
        rb = GetComponent<Rigidbody>();
        
        // Make the projectile move forward as soon as it spawns.
        rb.velocity = transform.forward * speed;

        // Destroy the projectile after a set amount of time, so our scene doesn't fill up with missed shots.
        Destroy(gameObject, lifetime);
    }

    // This special Unity function is called whenever this object's collider hits another collider.
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit has the "Enemy" tag.
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Try to get the EnemyHealth script from the object we hit.
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            
            // If the enemy has a health script...
            if (enemyHealth != null)
            {
                // ...call its TakeDamage function and pass in our damage value.
                enemyHealth.TakeDamage(damage);
            }
        }

        // Destroy the projectile after it hits *anything* (an enemy, the ground, etc.).
        Destroy(gameObject);
    }
}