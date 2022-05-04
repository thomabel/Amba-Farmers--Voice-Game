using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DisplayTime : MonoBehaviour
{	
	// Time-Based Events
	public UnityEvent onHourChange;
	public UnityEvent onDayChange;

//    public GameObject timeDisplay;
    public int startHour;
    public int startSecond;
    public int startMinute;
    public float accumulator;
	public int timeMultiplier = 1;
	public int minMultiplier = 1;
	public int maxMultiplier = 10;
    
    //public Text text;
    public IntVariable hours;
    public IntVariable minutes;
    public IntVariable seconds;

    // Start is called before the first frame update
    void Start()
    {
	hours.Value = startHour;
	minutes.Value = startSecond;
	seconds.Value = startMinute;	
	//text.text = "" + hours.Value + ":" + minutes.Value + ":" + seconds.Value.ToString("D2"); 
	//timeDisplay.GetComponent<Text>().text = "" + hour + ":" + minutes + ":" + seconds;
   	accumulator = 0; 
    }

    // Update is called once per frame
    void Update()
    {
	   // DeltaTime
	accumulator += Time.deltaTime;
	if (accumulator >= 1){
		seconds.Value += 1 * timeMultiplier;
		if (seconds.Value >=60){
			minutes.Value +=1;
			seconds.Value = 0;
		}
		if (minutes.Value >=60){
			hours.Value += 1;
			minutes.Value = 0;
			onHourChange.Invoke();
		}
		if (hours.Value >=24){
			hours.Value = 0;
			onDayChange.Invoke();
		}
		accumulator-=1;
	}	
	//text.text = "" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"); 
	
    }

	public string TimeDisplay() {
		return ("" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"));

	}

	public void incrementMultiplier()
	{
		if (timeMultiplier < maxMultiplier)
			timeMultiplier += 1;
	}

	public void decrementMultiplier()
	{
		if (timeMultiplier > minMultiplier)
		{
			timeMultiplier -= 1;
		}
	}

	public void resetMultiplier()
	{
		timeMultiplier = minMultiplier;
	}
}
