using UnityEngine;

public class Hoe : MonoBehaviour, IEquippable, IInteractable
{
    void IEquippable.Use(GameObject with)
    {
        if (with == null)
        {
            return;
        }
        var land = with.GetComponent<TerrainData>();
        if (land != null)
        {
            land.SetTilled(transform.position, true);
        }
        else
        {
            land = Terrain.activeTerrain.GetComponent<TerrainData>();
            land.SetTilled(transform.position, true);
        }
    }

    void IInteractable.Interact(GameObject with)
    {

    }
}
