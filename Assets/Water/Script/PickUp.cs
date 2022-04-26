using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject bucket;
    public GameObject myHands; 
    bool ifCanPickUp; 
    GameObject BucketNeedToPickUp; 
    bool hasItem;
    // Start is called before the first frame update
    void Start()
    {
        ifCanPickUp = false;    
        hasItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        //pickup bucket
        if (ifCanPickUp == true) 
        {
            if (Input.GetKeyDown(KeyCode.E))  
            {

                hasItem = true;
                BucketNeedToPickUp.GetComponent<Rigidbody>().isKinematic = true;  
                BucketNeedToPickUp.transform.position = myHands.transform.position; 
                BucketNeedToPickUp.transform.parent = myHands.transform; 
            }
        }
        //put bucket in my hand down
        if (Input.GetKeyDown(KeyCode.Q) && hasItem == true)
        {
   
            BucketNeedToPickUp.GetComponent<Rigidbody>().isKinematic = false; 
            BucketNeedToPickUp.transform.parent = bucket.transform; 
            hasItem = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Bucket") 
        {
            ifCanPickUp = true;  
            BucketNeedToPickUp = other.gameObject; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ifCanPickUp = false; 

    }


    public bool hasBucket()
    {
        return hasItem;
    }

    public void setHasBucket(bool condition)
    {
        hasItem = condition;
    }
}
