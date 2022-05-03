using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALLBucket : MonoBehaviour, IInteractable, IEquippable
{
    void IInteractable.Interact()
    {
        Debug.Log("Interact with a hoe!");
    }
    void IEquippable.Use()
    {
        Vector3 tile = this.gameObject.transform.position;
        //Terrain.activeTerrain.GetComponent<TerrainData>().SetWater(tile, true);
        Debug.Log("USE[Water]: " + tile);
    }
}
