using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controls : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(move);
    }

    Vector2 move;
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public float move_speed;
    void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
        {
            return;
        }
        var scaled_move_speed = move_speed * Time.deltaTime;
        var move = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        move *= scaled_move_speed;
        transform.Translate(move);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player_interact();
        }
        else if (context.canceled)
        {
            Debug.Log("canceled");
        }
    }

    public float interact_distance;
    void player_interact()
    {
        RaycastHit hit = new RaycastHit();
        var mpos = Mouse.current.position.ReadValue();
        //Debug.Log(mpos.ToString());

        Ray ray = cam.ScreenPointToRay(mpos);
        //Debug.Log(ray.origin.ToString());
        //Debug.Log(ray.direction.ToString());
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 5);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, interact_distance))
        {
            Debug.Log("hit: " + hit.transform.name);
        }
    }
}
