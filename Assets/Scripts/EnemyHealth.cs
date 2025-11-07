using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;

    // This is a private variable to track the current health.
    private float currentHealth;

    // Start is called before the first frame update.
    void Start()
    {
        // When the enemy spawns, set its health to the maximum.
        currentHealth = maxHealth;
    }

    // This is a special function that we can call from other scripts (like our projectile).
    // It must be "public" to be accessible.
    public void TakeDamage(float damageAmount)
    {
        // Subtract the damage from our current health.
        currentHealth -= damageAmount;

        // Check if the enemy has run out of health.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // This is our own private function to handle the enemy's death.
    private void Die()
    {
        // For now, dying simply means destroying the GameObject.
        // Later, we could add explosion effects or award points here.
        Destroy(gameObject);
    }
}