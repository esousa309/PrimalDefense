using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public int currencyOnDeath = 10;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // NEW LINE! Let's print the enemy's remaining health.
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health is now: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.AddCurrency(currencyOnDeath);
        Destroy(gameObject);
    }
}