using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // We no longer need public references here, we will pass them in from the button.
    
    // We create a private variable to hold the tower we want to build.
    private GameObject towerToBuild;

    // This is a new public function that our UI will call.
    // It tells the BuildManager which tower blueprint to get ready.
    public void SelectTowerToBuild(GameObject tower)
    {
        towerToBuild = tower;
    }
    
    // This is a new public function that the BuildTile will call.
    public void BuildTowerOnTile(Transform buildTile)
    {
        // First, check if we have even selected a tower to build.
        if (towerToBuild == null)
        {
            Debug.Log("No tower selected to build!");
            return; // Exit the function.
        }

        // We will get the cost from the tower's script in the future. For now, let's hardcode it.
        int towerCost = 100; // We'll improve this later.

        if (GameManager.instance.CurrentCurrency >= towerCost)
        {
            GameManager.instance.SpendCurrency(towerCost);
            Instantiate(towerToBuild, buildTile.position, Quaternion.identity);
            
            // We disable the build tile by passing it in as an argument.
            buildTile.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }
}
csharp
using UnityEngine;

public class BuildTile : MonoBehaviour
{
    // A private reference to the BuildManager.
    private BuildManager buildManager;

    void Start()
    {
        // Find the BuildManager in the scene when the game starts.
        buildManager = FindObjectOfType<BuildManager>();
    }

    // This is a special Unity function that is called when a mouse clicks on a 3D object with a collider.
    void OnMouseDown()
    {
        // When this tile is clicked, tell the BuildManager to build a tower on our position.
        buildManager.BuildTowerOnTile(transform);
    }
}