using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class afterPour : StateMachineBehaviour
{
    
    public GameObject EmptyBucket;
    private GameObject myHands;
    private GameObject BucketHolding;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myHands = GameObject.Find("Hand");
        myHands.transform.rotation = EmptyBucket.transform.rotation;
        foreach (Transform child in myHands.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        BucketHolding = Instantiate(EmptyBucket, myHands.transform.position, Quaternion.identity, myHands.transform) as GameObject;
        BucketHolding.GetComponent<Rigidbody>().isKinematic = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
