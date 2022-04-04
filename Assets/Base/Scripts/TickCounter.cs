using UnityEngine;

public class TickCounter : MonoBehaviour
{
    public FloatVariable seconds_total;
    public FloatVariable seconds_per_tick;
    public IntVariable ticks;
    private float seconds;

    private void Awake()
    {
        seconds_total.Value = 0;
        ticks.Value = 0;
        seconds = 0;
    }

    private void Update()
    {
        seconds_total.Value += Time.deltaTime;
        seconds += Time.deltaTime;
        if (seconds >= seconds_per_tick.Value)
        {
            seconds -= seconds_per_tick.Value;
            ticks.Value++;
        }
    }
}
