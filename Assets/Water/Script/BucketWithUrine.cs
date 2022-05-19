using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWithUrine : MonoBehaviour, IInteractable, IEquippable
{

    public GameObject EmptyBucket;
    private Equipment equipment;
    private Interact interact;
    private float UrineVolume;
    GameObject newBucket;

    private void Start()
    {
        UrineVolume = 5.0f;
    }
    void IInteractable.Interact(GameObject with)
    {
        return;
    }
    void IEquippable.Use(GameObject with)
    {
        var land = Terrain.activeTerrain.GetComponent<TerrainData>();

        GameObject player = GameObject.Find("Player");
        equipment = player.GetComponent<Equipment>();
        interact = player.GetComponent<Interact>();
        interact.try_closest();
        var last = interact.last_interacted;


        if (with.GetComponent<BucketWithWater>())
        {

            if (last != null && last.GetComponent<Plant>())
            //if (last == null)
            {

                useUrine(with);
                equipment.EquipTool(newBucket);
                land.SetNutrients(transform.position, 5.0f);
            }

        }

    }



    public float useUrine(GameObject with)
    {
        GameObject needToBeDestroyed;
        needToBeDestroyed = with;

        if (UrineVolume > 0.0)
        {
            UrineVolume -= 1.0f;
            Debug.Log("Current Urine Volume: " + UrineVolume);

            if (UrineVolume == 0.0)
            {
                Debug.Log("It is empty ");
                GameObject.Destroy(needToBeDestroyed);
                newBucket = Instantiate(EmptyBucket, with.transform.position, Quaternion.identity, with.transform.parent) as GameObject;

            }
            return 1.0f;
        }
        else
        {
            Debug.Log("Something Wrong in useUrine");
            return 0.0f;
        }

    }


    public float getCurrentUrineVolume()
    {
        return UrineVolume;
    }
}


