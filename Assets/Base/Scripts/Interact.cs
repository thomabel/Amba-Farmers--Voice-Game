using UnityEngine;

// For interacting with objects in the world.
public class Interact : MonoBehaviour
{
    public ColliderContainer container;
    public FloatReference ray_distance;
    public FloatReference interact_distance;
    public Color debug_color;

    public GameObject last_interacted;
    private RaycastHit hit;

    private void Awake()
    {
        last_interacted = null;
        hit = new RaycastHit();
    }

    /// <summary>
    /// Uses the closest item.
    /// </summary>
    /// <returns>The item that was used.</returns>
    public GameObject Use()
    {
        var interact = get_closest();
        check_interact(interact);
        return interact;
    }
    /// <summary>
    /// Uses the item hit by a ray.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns>The item that was hit.</returns>
    public GameObject Use(Ray ray)
    {
        var interact = get_pointed(ray);
        check_interact(interact);
        return interact;
    }


    // Find the closest gameobject.
    private GameObject get_closest()
    {
        var closest = container.GetClosest(transform.position, interact_distance.Value);
        if (closest != null)
        {
            return closest.gameObject;
        }
        return null;
    }
    // Use ray to find a gameobject.
    private GameObject get_pointed(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * ray_distance.Value, debug_color, 3);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, ray_distance.Value))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    // Check that the object is interactable.
    private IInteractable check_interact(GameObject check)
    {
        if (check != null)
        {
            var inter = check.GetComponent<IInteractable>();
            if (inter != null)
            {
                inter.Interact();
                last_interacted = check;
                Debug.Log("interact: " + last_interacted.name);
                return inter;
            }
        }
        return null;
    }
}
