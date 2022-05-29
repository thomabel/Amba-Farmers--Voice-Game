using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{	

    public int startHour;
    public int startSecond;
    public int startMinute;
    public float accumulator;
   
    public Text textDate;

    public int startDate;
    public int startMonth;
    public int startYear;

    public Text text; 
    public IntVariable hours; 
    public IntVariable minutes; 
    public IntVariable seconds; 
    public IntVariable date;
    public IntVariable month;
    public IntVariable year; 

    private string[] months = 
    			    {"Jan", "Feb", "Mar",
			     "Apr", "May", "Jun", 
			     "Jul", "Aug", "Sep", 
			     "Oct", "Nov", "Dec"};
    private int[] daysInMonth = {31, 28, 31, 30, 31, 30, 31, 31,30, 31, 30, 31, 31};
    public float timeScale;
    
    void Start()
    {
	hours.Value = startHour;
	minutes.Value = startMinute;
	seconds.Value = startSecond;	

	date.Value = startDate;
	month.Value = startMonth;
	year.Value = startYear;
        textDate.text = "" + date.Value.ToString("D2") + "-" + months[month.Value-1] + "-" + year.Value.ToString("D4") + " : "
	       		+ hours.Value.ToString("D2") + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"); 
		
  	accumulator = 0; 
    }
     
    private void Update()
    {
	accumulator += Time.deltaTime*timeScale;
	while (accumulator >= 1){
		seconds.Value +=1;
		while (seconds.Value >=60){
			minutes.Value +=1;
			seconds.Value = 0;
		}
		while (minutes.Value >=60){
			hours.Value += 1;
			minutes.Value = 0;
		}
		while (hours.Value >=24){
			date.Value += 1;
			hours.Value = 0;
		}
		while (date.Value > daysInMonth[month.Value-1]){
			month.Value += 1;
			date.Value = 1;
		}
		while (month.Value > 12){
			this.year.Value += 1;
			this.month.Value = 1;
			Debug.Log("in year");
		}
		accumulator-= 1;   
   	
	}
	text.text = "" + date.Value.ToString("D2") + "-" + months[month.Value-1] + "-" + year.Value.ToString("D4") + " : " + hours.Value.ToString("D2") + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"); 	
    }	

	public string TimeDisplay() {
		return ("" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"));

	}
}
