using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // A public "singleton" instance so our tiles can easily access it.
    public static BuildManager instance;

    // This will hold the blueprint of the tower the player has selected from the UI.
    private GameObject selectedTowerPrefab;

    void Awake()
    {
        // Set up the singleton.
        if (instance == null)
        {
            instance = this;
        }
    }

    // A public function that our UI buttons will call.
    // This tells the manager which tower blueprint to get ready.
    public void SelectTowerToBuild(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
    }

    // This is the function the BuildTile will call.
    public void BuildTowerOnTile(Transform buildTile)
    {
        if (selectedTowerPrefab == null)
        {
            Debug.Log("No tower selected!");
            return; // Exit if we haven't picked a tower from the UI first.
        }

        // We will add cost checks back in a future step. For now, let's just build.
        int cost = 100; // A placeholder cost.

        if (GameManager.instance.CurrentCurrency >= cost)
        {
            GameManager.instance.SpendCurrency(cost);
            Instantiate(selectedTowerPrefab, buildTile.position, Quaternion.identity);
            
            // We now disable the specific tile that was clicked.
            buildTile.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }
}