using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DisplayTime : MonoBehaviour
{	
	// Time-Based Events
	public GameEvent onHourChange;
	public GameEvent onDayChange;

//    public GameObject timeDisplay;
    public int startHour;
    public int startSecond;
    public int startMinute;
    public float accumulator;
	int previousHour;

	public GameSpeed currentSpeed = GameSpeed.Normal;
	public enum GameSpeed { Normal, Medium, Fast };
    
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
	switch (currentSpeed) {
		case GameSpeed.Medium:
			accumulator += Time.deltaTime * 60;
			break;
		case GameSpeed.Fast:
			accumulator += Time.deltaTime * 3600;
			break;
		default:
			accumulator += Time.deltaTime;
			break;
	}

	previousHour = hours.Value;
	seconds.Value = (int) Math.Floor(accumulator) % 60;
	minutes.Value = (int) Math.Floor(accumulator / 60) % 60;
	hours.Value = (int) Math.Floor(accumulator / 3600) % 24;
	if (Math.Abs(hours.Value - previousHour) > 0) {
		onHourChange.raise();
		previousHour = hours.Value;
		if (hours.Value == 0) {
			onDayChange.raise();
		}
	}
	//text.text = "" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"); 
    }

	public string TimeDisplay() {
		return ("" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"));

	}

	public void incrementMultiplier()
	{
		if (currentSpeed != GameSpeed.Fast) {
			currentSpeed += 1;
		}
	}

	public void decrementMultiplier()
	{
		if (currentSpeed != GameSpeed.Normal) {
			currentSpeed -= 1;
		}
	}

	public void resetMultiplier()
	{
		currentSpeed = GameSpeed.Normal;
	}
}
