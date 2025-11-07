using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("References")]
    // This is the blueprint of the tower we want to build.
    public GameObject towerPrefab;
    
    // This is the spot where we will build the tower.
    public Transform buildTile;

    [Header("Tower Settings")]
    public int towerCost = 100;

    // This is the function our UI button will call.
    public void BuildTower()
    {
        // First, check if the player has enough money.
        if (GameManager.instance.CurrentCurrency >= towerCost)
        {
            // If yes, spend the currency.
            GameManager.instance.SpendCurrency(towerCost);

            // Create the new tower from the blueprint at the tile's position.
            // We don't need to worry about rotation, so we use Quaternion.identity.
            Instantiate(towerPrefab, buildTile.position, Quaternion.identity);

            // Hide the build tile so we can't build on it again.
            buildTile.gameObject.SetActive(false);
        }
        else
        {
            // If the player doesn't have enough money, print a message to the console.
            Debug.Log("Not enough currency to build a tower!");
        }
    }
}