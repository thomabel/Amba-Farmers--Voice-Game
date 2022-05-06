using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Threading;

public class WeatherSystem: MonoBehaviour
{
    public enum Season {NONE, SPRING, SUMMER, AUTUMN, WINTER};
    public enum Weather {NONE, SUNNY, HOTSUN, RAIN, SNOW};

    public Season currentSeason;
    public Weather currentWeather;

    [Header ("Time Settings")]
    public float seasonTime;
    public float springTime;
    public float summerTime;
    public float autumnTime;
    public float winterTime;

    [Header ("Light Settings")]
    public Light sunLight;
    private float defaultLightIntensity;
    public float summerLightIntensity;
    public float autumnLightIntensity;
    public float winterLightIntensity;

    public Color defaultLightColor;
    public Color summerColor;
    public Color autumnColor;
    public Color winterColor;
   
    public int currentYear = 0;
//    private int currentSeasonIndex = 0;
//    private IEnumerator coroutine;

    void Start() {
        this.currentSeason = Season.SPRING;
	this.currentWeather = Weather.SUNNY;
	this.currentYear = 1;
	
	this.seasonTime = this.springTime;
	
	this.defaultLightColor = this.sunLight.color;
	this.defaultLightIntensity = this.sunLight.intensity;
   	
       	//coroutine = WaitAndChange(5.0f);
	//StartCoroutine(coroutine);	
    }
    public void ChangeSeason (Season seasonType)
    {
	if (seasonType != this.currentSeason)
	{
		//currentSeason = seasonType;
		switch (seasonType)
		{
		case Season.SPRING:
			currentSeason = Season.SPRING;
			break;
		case Season.SUMMER:
			currentSeason = Season.SUMMER;
			break;
		case Season.AUTUMN:
			currentSeason = Season.AUTUMN;
			break;
		case Season.WINTER:
			currentSeason = Season.WINTER;
			break;
		}
	}			
    }		 
    public void ChangeWeather(Weather weatherType)
    {
        if (weatherType != this.currentWeather)
	{		
	switch(weatherType){
		case Weather.SUNNY:
			currentWeather = Weather.SUNNY;
			break;
		case Weather.HOTSUN:
			currentWeather = Weather.HOTSUN;
			break; case Weather.RAIN:
			currentWeather = Weather.RAIN;
			break;
		case Weather.SNOW:
			currentWeather = Weather.SNOW;
			break;
	}
	} 
    } 
//    private IEnumerator WaitAndChange(float waitTime) {
//	Thread.Sleep(10000);
     void update(){ 
	this.seasonTime -= Time.deltaTime;
//	yield return new WaitForSeconds(waitTime);
	if (this.currentSeason == Season.SPRING)
	{
		LerpSunIntensity(this.sunLight, defaultLightIntensity);
		LerpLightColor(this.sunLight, defaultLightColor);
		
		ChangeWeather(Weather.SUNNY);
		if (this.seasonTime <= 0f)
		{
		   ChangeSeason(Season.SUMMER);
		   this.seasonTime = this.summerTime;
		}
	}
	else if (this.currentSeason == Season.SUMMER)
	{
		LerpSunIntensity(this.sunLight, summerLightIntensity);
		LerpLightColor(this.sunLight, summerColor);

		ChangeWeather(Weather.HOTSUN);
		if (this.seasonTime <= 0f)
		{
		   ChangeSeason(Season.AUTUMN);
		   this.seasonTime = this.autumnTime;
		}

	}
	else if (this.currentSeason == Season.AUTUMN)
	{
		LerpSunIntensity(this.sunLight, autumnLightIntensity);
		LerpLightColor(this.sunLight, autumnColor);

		ChangeWeather(Weather.RAIN);
		if (this.seasonTime <= 0f)
		{
		   ChangeSeason(Season.WINTER);
		   this.seasonTime = this.winterTime;
		}
	}
	else if (this.currentSeason == Season.WINTER)
	{
		LerpSunIntensity(this.sunLight, winterLightIntensity);
		LerpLightColor(this.sunLight, winterColor);

		ChangeWeather(Weather.SNOW);
		if (this.seasonTime <= 0f)
		{
		   this.currentYear++;
		   ChangeSeason(Season.SPRING);
		   this.seasonTime = this.springTime;
		}
	}
    }
     private void LerpLightColor (Light light, Color c)
     {
     	light.color = Color.Lerp(light.color, c, 0.2f*Time.deltaTime);
     }
     private void LerpSunIntensity (Light light, float intensity)
     {
     	light.intensity = Mathf.Lerp(light.intensity, intensity, 0.2f*Time.deltaTime);
     }
}
