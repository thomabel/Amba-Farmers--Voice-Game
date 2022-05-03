using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullWater : MonoBehaviour
{
    public PickUp pickup;
    public GameObject myHands;
    public GameObject NewBucket;
    public GameObject Bucket;
    public GameObject HierarchyBucket;
    private bool canFull;

    Animator animation;
    bool canPour;

    // Start is called before the first frame update
    void Start()
    {
        canFull = false;
        canPour = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Full water by destroy empty bucket, create a bucket with water
        if (Input.GetKeyDown(KeyCode.F))  // can be e or any key
        {
            if (canFull)
            {
                 foreach (Transform child in myHands.transform)
                 {
                    GameObject.Destroy(child.gameObject);
                }
                Bucket = Instantiate(NewBucket, myHands.transform.position, Quaternion.identity, myHands.transform) as GameObject;
                Bucket.GetComponent<Rigidbody>().isKinematic = true;
                pickup.setHasBucket(true);
            }

        }

        //put bucket in my hand down
        //if (Input.GetKeyDown(KeyCode.Q) == true) 
        //{
        //    Bucket.GetComponent<Rigidbody>().isKinematic = false; 
        //    Bucket.transform.parent = HierarchyBucket.transform; 
        //}


        //Implement pour water action if holding a bucket
        if (Input.GetKeyDown(KeyCode.P) && pickup.hasBucket())  
        {
            canPour = true;
        }
        else
        {
            canPour = false;
        }

        if (canPour)
        {
            animation = myHands.GetComponent<Animator>();
            animation.SetBool("Pour", true);
        }
        else
        {
            animation = myHands.GetComponent<Animator>();
            animation.SetBool("Pour", false);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water" && pickup.hasBucket())
        {
            Debug.Log("true");
            canFull = true;
        }
       

    }
    private void OnTriggerExit(Collider other)
    {
        canFull = false;
    }

}
