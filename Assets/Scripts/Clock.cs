using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;
    double time_since_start;
    int tick;
    double tick_time;
    public float seconds_per_tick;

    // Start is called before the first frame update
    void Start()
    {
        time_since_start = 0;
        tick = 0;
        tick_time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time_since_start += Time.deltaTime;
        tick_time += Time.deltaTime;

        if (tick_time > seconds_per_tick)
        {
            tick_time -= seconds_per_tick;
            tick++;
            //Debug.Log("tick: " + tick);
        }
        if (text)
        {
            text.text = time_since_start.ToString("F2");
        }
    }
}
