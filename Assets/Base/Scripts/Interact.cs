using UnityEngine;

public class Interact : MonoBehaviour
{
    public FloatReference interact_distance;
    public Color debug_color;
    public GameObject Use(Ray ray)
    {
        var obj = gameObject;
        var hit = new RaycastHit();
        Debug.DrawRay(ray.origin, ray.direction * interact_distance.Value, debug_color, 3);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, interact_distance.Value))
        {
            obj = hit.collider.gameObject;
        }
        return obj;
    }
}
