using UnityEngine;
using UnityEngine.InputSystem;

// Processes player input.
public class PlayerInput : MonoBehaviour
{
    public Camera cam;
    public Movement movement;
    public Interact interact;
    public InventoryHolder bag;
    public Equipment equipment;
    public Inventory house;

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
            interact.try_closest();

            var last = interact.last_interacted;
            if (last == null)
            {
                if(equipment.etool != null)
                {
                    equipment.DropTool();
                }

                return;
            }
            
            var fruit = last.GetComponent<Fruit>();
            if (fruit != null)
            {
                house.Add(last);
                return;
            }

            equipment.Pickup(last);
        }
    }
    public void OnToolUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (equipment.Tool != null)
            {
                if(equipment.etool.obj.tag == "Bucket")
                {
                    interact.try_closest();
                    var last = interact.last_interacted;

                    if (last != null && last.GetComponent<Pool>())
                    {
                        if (equipment.etool.obj.name == "EmptyBucket")
                        {
                            equipment.Tool.Use(equipment.etool.obj);
                            return;
                        }
               
                    }
                    else if (last ==null && equipment.etool.obj.name == "BucketWithWater")
                    {
                        //Debug.Log(equipment.etool.obj.name + "Check from INPUT");
                        equipment.Tool.Use(equipment.etool.obj);
                        return;
                    }
                }

               
                
                equipment.Tool.Use(null);
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
