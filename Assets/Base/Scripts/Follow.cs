using UnityEngine;

/// <summary>
/// A script for the camera to follow the player.
/// </summary>
public class Follow : MonoBehaviour
{
    public bool look_at; // Look directly at follow?
    public Transform follow; // The position to point toward.
    public FloatReference move_speed; // Movement speed in m/s.
    public FloatReference distance_max; // Max distance object can be from follow before moving.
    public FloatReference distance_offset; // Distance from max.
    public FloatReference height; // Height from the ground in metres.

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, follow.position);

        // We are too far from the follow.
        if (distance > distance_max.Value)
        {
            TranslateToward(transform.position, follow.position);
        }
        // Too close to follow.
        else if (distance < distance_max.Value - distance_offset.Value)
        {
            TranslateToward(follow.position, transform.position);
        }

        // Rotate towards the follow.
        if (look_at)
        {
            transform.LookAt(follow.position);
        }
    }

    // Move correctly.
    Vector3 TranslateToward(Vector3 origin, Vector3 direction)
    {
        Vector3 move = Vector3.Normalize(direction - origin);
        move.y = height.Value - transform.position.y;
        move *= move_speed.Value * Time.deltaTime;
        transform.Translate(move, Space.World);
        return move;
    }
}
