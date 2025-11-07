using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Tower Prefabs")]
    // A public spot for our basic turret blueprint.
    public GameObject turretPrefab; 
    // A public spot for our cannon blueprint.
    public GameObject cannonPrefab; 

    [Header("Build Location")]
    // The tile where we will build the tower.
    public Transform buildTile;

    // --- Turret Building Logic ---
    public void BuildTurret()
    {
        int cost = 100; // The price of this tower.

        // Check if we have enough money.
        if (GameManager.instance.CurrentCurrency >= cost)
        {
            // Spend the money.
            GameManager.instance.SpendCurrency(cost);
            // Build the tower.
            Instantiate(turretPrefab, buildTile.position, Quaternion.identity);
            // Hide the build tile.
            buildTile.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough money for a Turret!");
        }
    }

    // --- Cannon Building Logic ---
    public void BuildCannon()
    {
        int cost = 150; // The price of this tower.

        if (GameManager.instance.CurrentCurrency >= cost)
        {
            GameManager.instance.SpendCurrency(cost);
            Instantiate(cannonPrefab, buildTile.position, Quaternion.identity);
            buildTile.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough money for a Cannon!");
        }
    }
}