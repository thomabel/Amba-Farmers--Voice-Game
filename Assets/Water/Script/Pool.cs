using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour, IInteractable
{
    void IInteractable.Interact(GameObject with)
    {
        //Debug.Log("Interact with a pool!");
    }
   
}


//Ray ray = new Ray();
//ray.origin = transform.position;
//ray.direction = Vector3.down;
//RaycastHit hit;
//float ray_distance = 1;
//Physics.Raycast(ray.origin, ray.direction, out hit, ray_distance);

//hit.collider.gameObject.GetComponent<WaterPool>();