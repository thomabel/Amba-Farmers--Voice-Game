using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Threading;

public class WeatherSystem: MonoBehaviour
{
    public enum Season {NONE, SUMMER,  WINTER};
    public enum Weather {NONE, SUNNY, RAINY};

    public Season currentSeason;
    public Weather currentWeather;

    [Header ("Time Settings")]
    public float seasonTime;
//    public float springTime;
    public float summerTime;
 //   public float autumnTime;
    public float winterTime;

    [Header ("Light Settings")]
    public Light sunLight;
    private float defaultLightIntensity;
    public float summerLightIntensity;
  //  public float autumnLightIntensity;
    public float winterLightIntensity;

    public ParticleSystem rain;

    private Color defaultLightColor;
    public Color summerColor;
  //  public Color autumnColor;
    public Color winterColor;
  
    public int currentYear = 0;

    public IntVariable month;

    private int [] summerMonths = {10, 11, 12, 0, 1, 2, 3};
    private int [] winterMonths = {4, 5, 6, 7, 8, 9};
    private void Start() {
	Debug.Log("In start Weather");
	//this.month.Value = 1; 
	this.currentSeason = Season.SUMMER;
	this.currentWeather = Weather.SUNNY;
	this.currentYear = 1;
	
//	this.seasonTime = this.summerTime;
	
	this.defaultLightColor = this.sunLight.color;
	this.defaultLightIntensity = this.sunLight.intensity;
        	
    }
    // May - Oct, rainy, Nov - March Dry,  March - April Windy, 
    public void ChangeSeason (Season seasonType)
    {
	if (seasonType != this.currentSeason)
	{
		switch (seasonType)
		{
	/*
		case Season.SPRING:
			this.rain.Stop();
			currentSeason = Season.SPRING;
			break;
		*/
		case Season.SUMMER:
			this.rain.Stop();
			currentSeason = Season.SUMMER;
			break;
			/*
		case Season.AUTUMN:
			this.rain.Stop();
			currentSeason = Season.AUTUMN;
			break;
			*/
		case Season.WINTER:
			this.rain.Play();
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
				this.rain.Stop();
				break;
	/*		case Weather.HOTSUN:
				currentWeather = Weather.HOTSUN;
				this.rain.Stop();
				break;
	*/
			case Weather.RAINY:
				currentWeather = Weather.RAINY;
				this.rain.Play();
				break;
	/*		case Weather.SNOW:
				currentWeather = Weather.SNOW;
				this.rain.Stop();
				break;
	 */
      		}
	}
    } 
    // May - Nov, rain, Nov - April
     private void Update(){ 
	//Debug.Log("Update Weather");	
//	this.seasonTime -= Time.deltaTime;
//	yield return new WaitForSeconds(waitTime);
	/*if (this.currentSeason == Season.SPRING)
	{
		LerpSunIntensity(this.sunLight, defaultLightIntensity);
		LerpLightColor(this.sunLight, defaultLightColor);
		
		ChangeWeather(Weather.SUNNY);
		//if (this.seasonTime <= 0f)
		if (this.month 	
		{
		   ChangeSeason(Season.SUMMER);
		   this.seasonTime = this.summerTime;
		}
	}
	*/
	if (this.currentSeason == Season.SUMMER)
	{
		LerpSunIntensity(this.sunLight, summerLightIntensity);
		LerpLightColor(this.sunLight, summerColor);

		ChangeWeather(Weather.SUNNY);
		if (Array.Exists(winterMonths, element=>element == this.month.Value))
		{
		   ChangeSeason(Season.WINTER);
		   this.seasonTime = this.winterTime;
		}

	}
/*	else if (this.currentSeason == Season.AUTUMN)
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
	*/
	else if (this.currentSeason == Season.WINTER)
	{
		LerpSunIntensity(this.sunLight, winterLightIntensity);
		LerpLightColor(this.sunLight, winterColor);

		ChangeWeather(Weather.RAINY);
		if (Array.Exists(summerMonths, element=>element == this.month.Value))
		{
		   this.currentYear++;
		   ChangeSeason(Season.SUMMER);
		   this.seasonTime = this.summerTime;
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
