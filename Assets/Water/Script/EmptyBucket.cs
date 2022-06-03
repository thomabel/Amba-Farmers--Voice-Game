using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBucket : MonoBehaviour, IInteractable, IEquippable
{
    public GameObject BucketWithWater;
    public GameObject BucketWithUrine;
    private Equipment equipment;
    private Interact interact;

    public GameObject canvas;
    private Hint hint;

    public void Start()
    {
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
            if (last != null)
            {
                if (last.GetComponent<Pool>())
                {
                    GameObject.Destroy(needToBeDestroyed);
                    with = Instantiate(BucketWithWater, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                    equipment.EquipTool(with);
                }

                else if (last.GetComponent<Restroom>())
                {

                    GameObject.Destroy(needToBeDestroyed);
                    with = Instantiate(BucketWithUrine, with.transform.position, Quaternion.identity, player.transform) as GameObject;
                    equipment.EquipTool(with);
                }
            }

            //else if(){}    for Urine

        }







    }
}


