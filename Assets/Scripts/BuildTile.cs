using UnityEngine;

public class BuildTile : MonoBehaviour
{

}
```**Save the file.**

2.  Open your **`BuildManager.cs`** script. Erase everything and replace it with this. This ensures it is calling the correctly named class.

```csharp
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private GameObject selectedTowerPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SelectTowerToBuild(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (selectedTowerPrefab == null)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // This is the line that was breaking. It's now looking for the correct "BuildTile" class.
                BuildTile buildTile = hit.transform.GetComponent<BuildTile>();

                if (buildTile != null)
                {
                    int cost = 100;

                    if (GameManager.instance.CurrentCurrency >= cost)
                    {
                        GameManager.instance.SpendCurrency(cost);
                        Instantiate(selectedTowerPrefab, buildTile.transform.position, Quaternion.identity);
                        buildTile.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Not enough currency!");
                    }
                    
                    selectedTowerPrefab = null;
                }
            }
        }
    }
}