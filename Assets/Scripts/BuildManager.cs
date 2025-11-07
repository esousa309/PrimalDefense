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
}```
**Save the file.**

---

### **Step 3: Re-Create the `BuildTile` Script**

1.  Back in Unity, inside your **`Scripts`** folder, right-click and choose **Create > C# Script**.
2.  Name it **exactly `BuildTile`**.
3.  Double-click the new, clean file to open it.
4.  **Erase everything** inside it.
5.  Copy and paste this final, correct, comment-free code:

```csharp
using UnityEngine;

public class BuildTile : MonoBehaviour
{

}