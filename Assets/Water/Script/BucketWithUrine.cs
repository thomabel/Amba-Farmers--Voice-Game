using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWithUrine : MonoBehaviour, IInteractable, IEquippable
{

    public GameObject EmptyBucket;
    public GameObject BucketWithWater;
    private Equipment equipment;
    private Interact interact;
    private float UrineVolume;
    GameObject newBucket;

    public GameObject canvas;
    private Hint hint;


    public void Start()
    {
        UrineVolume = 5.0f;
        canvas = GameObject.Find("Canvas");
        hint = canvas.GetComponent<Hint>();

    }


    private void OnTriggerEnter(Collider other)
    {
        hint = canvas.GetComponent<Hint>();
        hint.OpenMessage("Use left button to pick up bucket");
    }

    private void OnTriggerExit(Collider other)
    {
        hint.CloseMessage();
    }



    void IInteractable.Interact(GameObject with)
    {
        Debug.Log("From Water");
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


        if (with.GetComponent<BucketWithUrine>())
        {

            if (last != null)
            {
                if (last.GetComponent<Plant>())
  
                {
                    useUrine(with);
                    equipment.EquipTool(newBucket);
                    land.SetNutrients(transform.position, 5.0f);

                }

                else if (last.GetComponent<Pool>())
                {
                    Destroy(with);
                    with = Instantiate(BucketWithWater, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                    equipment.EquipTool(with);

                }

                else if (last.GetComponent<Restroom>())
                {
                    UrineVolume = 5.0f;
                }
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


