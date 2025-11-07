using UnityEngine;

public class BuildTile : MonoBehaviour
{
    // This is a special Unity function that is called when a mouse clicks on any 3D object with a collider.
    void OnMouseDown()
    {
        // When this tile is clicked, we access the global BuildManager instance...
        // ...and we call its function, passing in our own transform (position, rotation, scale).
        BuildManager.instance.BuildTowerOnTile(transform);
    }
}