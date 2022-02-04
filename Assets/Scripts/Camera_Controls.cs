using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controls : MonoBehaviour
{
    public Transform focus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        track();
    }

    public float move_speed;
    public float distance_min;
    public float distance_max;
    void track()
    {
        transform.LookAt(focus.position);
        float distance = Vector3.Distance(transform.position, focus.position);
        if (distance > distance_max)
        {
            transform.Translate(FindTranslation(transform.position, focus.position), Space.World);
        }
        else if (distance < distance_min) 
        {
            transform.Translate(FindTranslation(focus.position, transform.position), Space.World);
        }
    }

    Vector3 FindTranslation(Vector3 pos1, Vector3 pos2)
    {
        var scaled_move_speed = move_speed * Time.deltaTime;
        var move = Vector3.Normalize(pos2 - pos1);
        move.y = 0;
        move *= scaled_move_speed;
        return move;
    }
}
