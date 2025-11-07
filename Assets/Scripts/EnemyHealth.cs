using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public int currencyOnDeath = 10; // NEW: How much this enemy is worth.

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // THIS IS THE NEW PART:
        // Before we destroy the enemy, we access the global GameManager instance
        // and call its AddCurrency function, passing in how much this enemy is worth.
        GameManager.instance.AddCurrency(currencyOnDeath);

        // Then, we destroy the enemy GameObject.
        Destroy(gameObject);
    }
}