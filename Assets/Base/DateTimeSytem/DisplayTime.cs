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
    
    public Text text;
    public IntVariable hours;
    public IntVariable minutes;
    public IntVariable seconds;

    // Start is called before the first frame update
    void Start()
    {
	hours.Value = startHour;
	minutes.Value = startSecond;
	seconds.Value = startMinute;	
	text.text = "" + hours.Value + ":" + minutes.Value + ":" + seconds.Value.ToString("D2"); 
	//timeDisplay.GetComponent<Text>().text = "" + hour + ":" + minutes + ":" + seconds;
    }

    // Update is called once per frame
    void Update()
    {
	   // DeltaTime
	accumulator += Time.deltaTime;
	if (accumulator >= 1){
		seconds.Value +=1;
		if (seconds.Value >=60){
			minutes.Value +=1;
			seconds.Value = 0;
		}
		if (minutes.Value >=60){
			hours.Value += 1;
			minutes.Value = 0;
		}
		if (hours.Value >=24){
			hours.Value = 0;
		}

	}
	text.text = "" + hours.Value + ":" + minutes.Value.ToString("D2") + ":" + seconds.Value.ToString("D2"); 
	
    }
}
