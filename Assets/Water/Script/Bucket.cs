using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable, IEquippable
{
    public GameObject BucketWithWater;
    public GameObject EmptyBucket;
    private Equipment equipment;
    public float WaterVolume;
    public enum bucket
    {
        EmptyBucket,
        BucketWithWater,
        BucketWithUrine
    }
    private void Start()
    {
        WaterVolume = 0.0f;
    }
    void IInteractable.Interact(GameObject with)
    {
        //Debug.Log("Interact with a bucket!");
    }
    void IEquippable.Use(GameObject with)
    {
        var land = Terrain.activeTerrain.GetComponent<TerrainData>();
        if(with != null)
        {
            
            GameObject needToBeDestroyed;
            needToBeDestroyed = with;
            GameObject.Destroy(needToBeDestroyed);
            GameObject player = GameObject.Find("Player");
            equipment = player.GetComponent<Equipment>();


            if (with.name == "EmptyBucket")
            {

                with = Instantiate(BucketWithWater, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                with.name = "BucketWithWater";
                equipment.EquipTool(with);
                WaterVolume = 5.0f;

            }
            else if(with.name == "BucketWithWater")
            {

                with = Instantiate(EmptyBucket, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                with.name = "EmptyBucket";
                equipment.EquipTool(with);

                WaterVolume = 0.0f;

                land.SetWater(transform.position, 5.0f);
            }
            else if(with.name == "BucketWithUrine")
            {
                with = Instantiate(EmptyBucket, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                with.name = "EmptyBucket";
                equipment.EquipTool(with);
                land.SetNutrients(transform.position, 5.0f);
            }


        }
    }
}
