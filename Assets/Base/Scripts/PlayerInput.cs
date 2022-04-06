using UnityEngine;
using UnityEngine.InputSystem;

// Processes player input.
public class PlayerInput : MonoBehaviour
{
    public Camera cam;
    public Movement movement;
    public Interact interact;

    private Vector2 move_input;
    private void Awake()
    {
        move_input = new Vector2();
    }
    private void Update()
    {
        movement.Move(move_input, cam.transform.eulerAngles.y);
    }

    // Movement input check.
    public void OnMove(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
        Debug.Log(context);
    }
    // Interact input check.
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interact.Use();
            //interact.Use(cam.ScreenPointToRay(Mouse.current.position.ReadValue()));
        }
    }
}
