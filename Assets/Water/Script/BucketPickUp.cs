using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPickUp : MonoBehaviour
{
    public GameObject bucket;
    public GameObject myHands; 


    // Start is called before the first frame update



    public void bucketPickup(GameObject BucketNeedToPickUp)
    {
        BucketNeedToPickUp.GetComponent<Rigidbody>().isKinematic = true;
        BucketNeedToPickUp.transform.position = myHands.transform.position;
        BucketNeedToPickUp.transform.parent = myHands.transform;
    }

    public void bucketDrop(GameObject BucketNeedToDrop)
    {
        BucketNeedToDrop.GetComponent<Rigidbody>().isKinematic = false;
        BucketNeedToDrop.transform.rotation = bucket.transform.rotation;
        BucketNeedToDrop.transform.parent = bucket.transform;
        
    }



    public bool hasBucket()
    {
        return true;
    }

    public void setHasBucket(bool condition)
    {
       
    }
}
