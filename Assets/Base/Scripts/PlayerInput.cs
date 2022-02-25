using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Camera cam;
    public Movement movement;
    public Interact interact;
    private Vector2 input_move;
    private GameObject last_interacted;

    private void Update()
    {
        movement.Move(input_move, cam.transform.eulerAngles.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        input_move = context.ReadValue<Vector2>();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var mpos = Mouse.current.position.ReadValue();
            var ray = cam.ScreenPointToRay(mpos);
            
            last_interacted = interact.Use(ray);
            Debug.Log("hit: " + last_interacted.name);

            var inter = last_interacted.GetComponent<IInteractable>();
            if (inter != null)
            {
                inter.Interact();
            }
        }
    }
}
