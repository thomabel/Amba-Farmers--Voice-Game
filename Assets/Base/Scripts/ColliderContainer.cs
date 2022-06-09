using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a hash set of all nearby colliders to the game object.
/// </summary>
public class ColliderContainer : MonoBehaviour
{
    public Collider RecentlyAdded;
    public StringVariable NearByObjectName;

    private HashSet<Collider> colliders = new HashSet<Collider>();


    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other); //hashset automatically handles duplicates

        // For UI
        RecentlyAdded = other;
        if (other.gameObject.CompareTag("Plant"))
            NearByObjectName.Value = other.gameObject.name;
        else
            NearByObjectName.Value = null;

    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

    public bool Add(Collider target)
    {
        return colliders.Add(target);
    }
    public void Add(Collider[] targets)
    {
        foreach (Collider c in targets)
        {
            colliders.Add(c);
        }
    }
    public bool Remove(Collider target)
    {
        return colliders.Remove(target);
    }
    public void Remove(Collider[] targets)
    {
        foreach (Collider c in targets)
        {
            colliders.Remove(c);
        }
    }

    /// <summary>
    /// Grabs the nearest collider to this object. Compares center of colliders to pos.
    /// </summary>
    /// <param name="pos">Compares to this position.</param>
    /// <param name="maximum">Maximum distance away collider can be.</param>
    /// <returns>The closest collider in the set.</returns>
    public Collider GetClosest(Vector3 pos, float maximum)
    {
        Collider closest = null;
        foreach (Collider collider in colliders)
        {
            if (collider == null)
            {
                continue;
            }
            var dist = Vector3.Distance(pos, collider.transform.position);
            if (collider.GetComponent<Pool>())
            {
                dist -= 3;
            }
            if (dist <= maximum)
            {
                closest = collider;
                maximum = dist;
            }
        }
        return closest;
    }
}