using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWithWater : MonoBehaviour, IInteractable, IEquippable
{

    public GameObject EmptyBucket;
    private Equipment equipment;
    private Interact interact;
    private float WaterVolume;
    GameObject newBucket;

    public GameObject canvas;
    private Hint hint;
    private VolumeBar volumeBar;

    public void Start()
    {
        WaterVolume = 5.0f;
        canvas = GameObject.Find("Canvas");
        hint = canvas.GetComponent<Hint>();
        volumeBar = canvas.GetComponent<VolumeBar>();
    }


    private void OnTriggerEnter(Collider other)
    {
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
                
                useWater(with);
                volumeBar.OpenBar(true, WaterVolume);
                equipment.EquipTool(newBucket);
                land.SetWater(transform.position, 1.0f);
                if (WaterVolume == 0.0f)
                {
                    volumeBar.CloseBar();
                }
            }

        }

    }



    public float useWater(GameObject with)
    {
        GameObject needToBeDestroyed;
        needToBeDestroyed = with;

        if (WaterVolume > 0.0)
        {
            WaterVolume -= 1.0f;
            Debug.Log("Current Water Volume: " + WaterVolume);

            if (WaterVolume == 0.0)
            {
                Debug.Log("It is empty ");
                GameObject.Destroy(needToBeDestroyed);
                newBucket = Instantiate(EmptyBucket, with.transform.position, Quaternion.identity, with.transform.parent) as GameObject;

            }
            return 1.0f;
        }
        else
        {
            Debug.Log("Something Wrong in useWater");
            return 0.0f;
        }

    }


    public float getCurrentWaterVolume()
    {
        return WaterVolume;
    }
}


