using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderContainer : MonoBehaviour
{
    public Collider RecentlyAdded;
    private HashSet<Collider> colliders = new HashSet<Collider>();
    public HashSet<Collider> Colliders { get { return colliders; } }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other); //hashset automatically handles duplicates
        RecentlyAdded = other;
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

    public Collider GetClosest(Vector3 pos, float maximum)
    {
        Collider closest = null;
        foreach (Collider collider in Colliders)
        {
            if (collider == null)
            {
                continue;
            }
            var dist = Vector3.Distance(pos, collider.transform.position);
            if (dist <= maximum)
            {
                closest = collider;
                maximum = dist;
            }
        }
        return closest;
    }
}