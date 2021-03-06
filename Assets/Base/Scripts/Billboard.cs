using UnityEngine;

/// <summary>
/// Represents a 2D sprite in a 3D world that perpetually turns to face the camera.
/// Place on a child object.
/// </summary>
public class Billboard : MonoBehaviour
{
    public Vector2 size; // The vertical and horizontal dimensions.
    public Material mat; // Holds the actual sprite.
    public Renderer rend; // Renderer for changing sprites.
    public Camera cam; // Camera to turn toward.

    private void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
        rend.material = mat;
    }
    private void OnEnable()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        // Changes to the new sprite on enable.
        rend.material = mat;
        transform.localScale = size;
        var pos = transform.localPosition;
        pos.y = size.y / 2;
        transform.localPosition = pos;
    }
    private void Update()
    {
        transform.rotation = cam.transform.rotation;
    }

    public void ChangeMaterial(Material mat)
    {
        rend.material = mat;
    }
}
