using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBucket : MonoBehaviour, IInteractable, IEquippable
{
    public GameObject BucketWithWater;
    
    private Equipment equipment;
    private Interact interact;



    void IInteractable.Interact(GameObject with)
    {
        return;
    }
    void IEquippable.Use(GameObject with)
    {
        var land = Terrain.activeTerrain.GetComponent<TerrainData>();


        GameObject needToBeDestroyed;
        needToBeDestroyed = with;
        GameObject player = GameObject.Find("Player");
        equipment = player.GetComponent<Equipment>();
        interact = player.GetComponent<Interact>();
        interact.try_closest();
        var last = interact.last_interacted;


        if (with.GetComponent<EmptyBucket>())
        {
            if (last != null && last.GetComponent<Pool>())
            {
                GameObject.Destroy(needToBeDestroyed);
                with = Instantiate(BucketWithWater, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                equipment.EquipTool(with);
            }

            //else if(){}    for Urine

        }





    }
}


