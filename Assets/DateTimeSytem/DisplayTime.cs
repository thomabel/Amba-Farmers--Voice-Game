using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{	
//    public GameObject timeDisplay;
    public int startHour;
    public int startSecond;
    public int startMinute;
    public float accumulator;

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
//    [31, 28, 31]
    public string[] months = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
    public int[] daysInMonth = {31, 28, 31, 30, 31, 30, 31, 31,30, 31, 30, 31}
    public float timeScale;
    // Start is called before the first frame update
    void Start()
    {
	hours.Value = startHour;
	minutes.Value = startSecond;
	seconds.Value = startMinute;	
	date.Value = startDate;
	month.Value = startMonth;
	year.Value = startYear;
        text.text = "" + year.Value.ToString("D4") + ":" + months[month.Value-1].ToString("D2")+ ":" date.Value.ToString("D2");
	text.text = "" + hours.Value + ":" + minutes.Value + ":" + seconds.Value.ToString("D2"); 
	
	//timeDisplay.GetComponent<Text>().text = "" + hour + ":" + minutes + ":" + seconds;
   	accumulator = 0; 
    }

    // Update is called once per frame
    void Update()
    {
	// DeltaTime
	// use while loop, ifs->while, 
	
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
			//date.Value = date.Value % 24;
			hours.Value = 0;
		}
		while (date.Value >=daysInMonth[months[month.Value-1]]){
			month.Value += 1;
			//month.Value = month.Value %12;
			date.Value = 0;
		}
		while (month.Value >= 12){
			year.Value += 1;
			month.Value = 0;
		}
		accumulator-=1;
	}
	text.text = "" + year.Value.ToString("D4") + ":" + months[month.Value-1].ToString("D2")+ ":" date.Value.ToString("D2");	
	text.text = "" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"); 	
    }
}
