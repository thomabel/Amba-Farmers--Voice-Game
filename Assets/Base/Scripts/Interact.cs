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
        hit = new RaycastHit();
    }

    // Interact with the closest interactable object.
    public GameObject Use()
    {
        if (container.Colliders != null)
        {
            Collider closest = container.GetClosest(transform.position, interact_distance.Value);
            if (closest != null)
            {
                return InteractCheck(closest.gameObject);
            }

        }
        return null;
    }
    // Use the mouse to specify object interact.
    public GameObject Use(Ray ray)
    {
        Debug.DrawRay(ray.origin, ray.direction * ray_distance.Value, debug_color, 3);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, ray_distance.Value))
        {
            Debug.Log("hit: " + last_interacted.name);
            return InteractCheck(hit.collider.gameObject);
        }
        return null;
    }

    // Check if the object is close enough to interact with.
    private bool CloseEnough(Vector3 obj)
    {
        return Vector3.Distance(transform.position, obj) <= interact_distance.Value;
    }
    // Check that the object is interactable.
    private GameObject InteractCheck(GameObject check)
    {
        var inter = check.GetComponent<IInteractable>();
        if (inter != null && CloseEnough(check.transform.position))
        {
            inter.Interact();
            last_interacted = check;
            return check;
        }
        return null;
    }
}
