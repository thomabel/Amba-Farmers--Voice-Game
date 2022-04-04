using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderContainer : MonoBehaviour
{

    private HashSet<Collider> colliders = new HashSet<Collider>();
    public HashSet<Collider> Colliders { get { return colliders; } }

    public Collider GetClosest(Vector3 origin, float min_distance)
    {
        Collider closest = null;
        foreach (Collider col in colliders)
        {
            var dist = Vector3.Distance(origin, col.transform.position);
            if (dist <= min_distance)
            {
                min_distance = dist;
                closest = col;
            }
        }
        return closest;
    }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other); //hashset automatically handles duplicates
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

}