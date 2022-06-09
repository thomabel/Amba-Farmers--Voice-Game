using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player input master class.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    // References that input controls.
    public Camera cam;
    public Movement movement;
    public Interact interact;
    public InventoryHolder bag;
    public Equipment equipment;
    public Inventory house;

    // Holds input values.
    private Vector2 move_input;
    private bool moving;

    private void Start()
    {
        move_input = new Vector2();
        moving = false;
    }
    private void Update()
    {
        if (moving)
        {
            movement.Move(move_input, cam.transform.eulerAngles.y);
        }
    }

    // All OnAction methods connect via the Editor to the Input System.
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moving = true;
            move_input = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moving = false;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Tries to interact with the closest object.
            interact.try_closest();

            var last = interact.last_interacted;
            if (last == null)
            {
                if (equipment.etool != null)
                {
                    equipment.DropTool();
                }
                return;
            }
            
            // Automatically stores collected fruit in house.
            var fruit = last.GetComponent<Fruit>();
            if (fruit != null)
            {
                house.Add(last);
                return;
            }

            // Tries to pick item up.
            equipment.Pickup(last);
        }
    }
    public void OnToolUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            if (equipment.etool == null)
            {
                return;
            }
            else
            {
                equipment.Tool.Use(equipment.etool.obj);

            }
        }
    }
    public void OnItemUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (equipment.Item != null)
            {
                equipment.Item.Use();
            }
        }
    }
    public void OnInventoryOpen(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // TODO: Open inventory here.
        }
    }
}
