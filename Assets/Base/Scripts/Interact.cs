using UnityEngine;

/// <summary>
/// For interacting with objects in the world.
/// </summary>
public class Interact : MonoBehaviour
{
    public ColliderContainer container;
    public FloatReference ray_distance;
    public FloatReference interact_distance;
    public Color debug_color;
    public GameObject last_interacted = null;

    /// <summary>
    /// Try to interact with the closest game object.
    /// </summary>
    public void try_closest()
    {
        Try(get_closest());
    }

    /// <summary>
    /// Find the closest game object.
    /// </summary>
    /// <returns>The object.</returns>
    public GameObject get_closest()
    {
        var closest = container.GetClosest(transform.position, interact_distance.Value);
        return closest == null ? null : closest.gameObject;
    }

    /// <summary>
    /// Try to interact with some game object in the world.
    /// </summary>
    /// <param name="item">The object to check.</param>
    /// <returns>Success of interaction.</returns>
    public bool Try(GameObject item)
    {
        if (item == null)
        {
            last_interacted = null;
            return false;
        }
        //Debug.Log("Try interacting with " + item.name);

        var inter = item.GetComponent<IInteractable>();
        if (inter == null)
        {
            last_interacted = null;
            return false;
        }

        inter.Interact(null);
        last_interacted = item;
        Debug.Log("Interacted with " + last_interacted.name + '.');
        return true;
    }

    // Use ray to find a gameobject.
    //private GameObject get_pointed(Ray ray)
    //{
    //    Debug.DrawRay(ray.origin, ray.direction * ray_distance.Value, debug_color, 3);

    //    if (Physics.Raycast(ray.origin, ray.direction, out hit, ray_distance.Value))
    //    {
    //        return hit.collider.gameObject;
    //    }
    //    return null;
    //}
    //interact.Use(cam.ScreenPointToRay(Mouse.current.position.ReadValue()));
}
