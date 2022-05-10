using UnityEngine;

public class Hoe : MonoBehaviour, IEquippable, IInteractable
{
    void IEquippable.Use(GameObject with)
    {
        if (with == null)
        {
            var land = Terrain.activeTerrain.GetComponent<TerrainData>();
            land.SetTilled(transform.position, true);
        }
        else
        {
            var land = with.GetComponent<TerrainData>();
            land.SetTilled(transform.position, true);
        }
    }

    void IInteractable.Interact(GameObject with)
    {

    }
}
