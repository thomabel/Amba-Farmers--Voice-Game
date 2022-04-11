using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderContainer : MonoBehaviour
{

    private HashSet<Collider> colliders = new HashSet<Collider>();
    public HashSet<Collider> Colliders { get { return colliders; } }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other); //hashset automatically handles duplicates
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

}