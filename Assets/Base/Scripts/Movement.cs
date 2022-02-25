using UnityEngine;

public class Movement : MonoBehaviour
{
    public FloatReference move_speed;
    public FloatReference deadzone;
    public void Move(Vector2 direction, float y_angle)
    {
        if (direction.sqrMagnitude < deadzone.Value)
        {
            return;
        }
        var scaled_move_speed = move_speed.Value * Time.deltaTime;
        var move = Quaternion.Euler(0, y_angle, 0) * new Vector3(direction.x, 0, direction.y);
        move *= scaled_move_speed;
        transform.Translate(move);
    }
}
