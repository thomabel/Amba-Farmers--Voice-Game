using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable, IEquippable
{
    public GameObject BucketWithWater;
    public Equipment equipment;
    void IInteractable.Interact(GameObject with)
    {
        //Debug.Log("Interact with a bucket!");
    }
    void IEquippable.Use(GameObject with)
    {
        if (with == null)
        {
            var land = Terrain.activeTerrain.GetComponent<TerrainData>();
            land.SetWater(transform.position, 2.0f);
        }
        else
        {
            GameObject needToBeDestroyed;
            needToBeDestroyed = with;
            GameObject.Destroy(needToBeDestroyed);

            with = Instantiate(BucketWithWater, with.transform.position, Quaternion.identity, GameObject.Find("Player").transform) as GameObject;
            equipment.EquipTool(with);

        }
    }
}
