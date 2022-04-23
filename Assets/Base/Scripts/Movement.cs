using UnityEngine;

public class Movement : MonoBehaviour
{
    public FloatReference move_speed;
    private Vector3 rotated;

    private void Start()
    {
        rotated = new Vector3();
    }
    
    /// <summary>
    /// Move this gameobject in the direction given.
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector3 direction)
    {
        direction *= move_speed.Value * Time.deltaTime;
        transform.Translate(direction, Space.World);
        //transform.Rotate(
        //    Quaternion.Angle(
        //        transform.rotation, 
        //        Quaternion.LookRotation(transform.forward)), 
        //    Space.Self);
        transform.rotation = Quaternion.LookRotation(direction);
    }
    /// <summary>
    /// Move this gameobject by first converting stick input.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cameraYangle"></param>
    public void Move(Vector2 input, float cameraYangle)
    {
        DetermineMoveVector(input, cameraYangle);
        Move(rotated);
    }

    // 
    private void DetermineMoveVector(Vector2 input, float cameraYangle)
    {
        rotated.x = input.x;
        rotated.z = input.y;
        rotated.y = 0;
        rotated = Quaternion.Euler(0, cameraYangle, 0) * rotated;
    }
}
