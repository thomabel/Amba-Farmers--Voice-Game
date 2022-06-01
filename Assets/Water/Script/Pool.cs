using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour, IInteractable
{
    public GameObject canvas;
    private Hint hint;
    private GameObject player;
    public void Start()
    {
        player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        hint = canvas.GetComponent<Hint>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (player.GetComponentInChildren<EmptyBucket>())
        {
            hint.OpenMessage("Use down button to full the water");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        hint.CloseMessage();
    }

  

    void IInteractable.Interact(GameObject with)
    {
       
 
        //hint.OpenMessage("");

    }

}


//Ray ray = new Ray();
//ray.origin = transform.position;
//ray.direction = Vector3.down;
//RaycastHit hit;
//float ray_distance = 1;
//Physics.Raycast(ray.origin, ray.direction, out hit, ray_distance);

//hit.collider.gameObject.GetComponent<WaterPool>();