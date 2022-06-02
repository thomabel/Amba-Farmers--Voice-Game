using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restroom : MonoBehaviour, IInteractable
{
    private GameObject player;
    public GameObject canvas;
    private Hint hint;

    public void Start()
    {
        canvas = GameObject.Find("Canvas");
        hint = canvas.GetComponent<Hint>();
        player = GameObject.Find("Player");
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (player.GetComponentInChildren<EmptyBucket>())
    //    {
    //        hint.OpenMessage("Use down button to get urine nutrients");
    //    }
            
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    hint.CloseMessage();
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (player.GetComponentInChildren<EmptyBucket>())
        {
            hint.OpenMessage("Use down button to get urine nutrients");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        hint.CloseMessage();
    }
    void IInteractable.Interact(GameObject with)
    {
        //Debug.Log("Interact with a pool!");
    }

    // Start is called before the first frame update

}
