using UnityEngine;

public class Hoe : MonoBehaviour, IInteractable, IEquippable
{
    void IInteractable.Interact()
    {
        Debug.Log("Interact with a hoe!");
    }
    void IEquippable.Use()
    {
        Vector3 tile = this.gameObject.transform.position;
        Terrain.activeTerrain.GetComponent<TerrainData>().SetTilled(tile, true);
        Debug.Log("USE[Hoe]: " + tile);
    }
}
