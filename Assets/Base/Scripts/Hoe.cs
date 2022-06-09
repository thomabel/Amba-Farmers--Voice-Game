using UnityEngine;

/// <summary>
/// Tool that applies the tilled status to the ground at the current position.
/// </summary>
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
            //var land = with.GetComponent<TerrainData>();
            //land.SetTilled(transform.position, true);
            var land = Terrain.activeTerrain.GetComponent<TerrainData>();
            land.SetTilled(transform.position, true);
        }
    }

    void IInteractable.Interact(GameObject with)
    {

    }
}
