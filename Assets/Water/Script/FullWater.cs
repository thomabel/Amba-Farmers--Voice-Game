using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullWater : MonoBehaviour
{
    public Equipment equipment;
    public BucketPickUp pickup;
    public GameObject myHands;
    public GameObject BucketWithWater;
    public GameObject EmptyBucket;
    public GameObject HierarchyBucket;
    private bool canFull;

    private GameObject BucketHolding;
    private Animator animator;
    bool canPour;

    // Start is called before the first frame update
    void Start()
    {
        animator = new Animator();
        canFull = false;
        canPour = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        animator = myHands.GetComponent<Animator>();
        animator.SetBool("Pour", false);
        //AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //if (info.normalizedTime >= 1.0f)

        //{

        //    afterPour();

        //}

    }


    public void Full()
    {
        if (canFull)
        {
            foreach (Transform child in myHands.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            BucketHolding = Instantiate(BucketWithWater, myHands.transform.position, Quaternion.identity, myHands.transform) as GameObject;
            BucketHolding.GetComponent<Rigidbody>().isKinematic = true;
            //pickup.setHasBucket(true);
        }
        else if (equipment.ifHasBucket())
        {
            animator = myHands.GetComponent<Animator>();
            animator.SetBool("Pour", true);
        

            
        }
    }


    public void afterPour()
    {
        foreach (Transform child in myHands.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        BucketHolding = Instantiate(EmptyBucket, myHands.transform.position, Quaternion.identity, myHands.transform) as GameObject;
        BucketHolding.GetComponent<Rigidbody>().isKinematic = true;

    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water" && equipment.ifHasBucket())
        {
            Debug.Log("Pool nearby, can get some water");
            canFull = true;
        }
       

    }
    private void OnTriggerExit(Collider other)
    {
        canFull = false;
    }

}
