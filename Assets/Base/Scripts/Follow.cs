using UnityEngine;

public class Follow : MonoBehaviour
{
    public bool look_at;
    public Transform follow;
    public FloatReference move_speed;
    public FloatReference distance_max;
    public FloatReference distance_offset;
    public FloatReference height;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, follow.position);

        if (distance > distance_max.Value)
        {
            TranslateToward(transform.position, follow.position);
        }
        else if (distance < distance_max.Value - distance_offset.Value)
        {
            TranslateToward(follow.position, transform.position);
        }

        if (look_at)
        {
            transform.LookAt(follow.position);
        }
    }

    Vector3 TranslateToward(Vector3 origin, Vector3 direction)
    {
        Vector3 move = Vector3.Normalize(direction - origin);
        move.y = height.Value - transform.position.y;
        move *= move_speed.Value * Time.deltaTime;
        transform.Translate(move, Space.World);
        return move;
    }
}
