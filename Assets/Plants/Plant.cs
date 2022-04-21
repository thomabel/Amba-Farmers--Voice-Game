using UnityEngine;
using System.Collections.Generic;

public class Plant: MonoBehaviour
{
    // The position of the sun
    private Transform sunPosition;
    // Renderer for changing material

    // Variables for the timed function
    private float timeAccumulator = 0.0f;
    private float waitTime = 5.0f;

    // List of possible health statuses for the plant
    private string[] plantStatusList = { "Dead", "Very Unhealthy", "Unhealthy", "Fine", "Healthy", "Optimal" };
    // Current status of the plant
    public int plantStatus = 3; //"Fine"
    // Steps to next plant status
    public int plantTransitionTracker = 3;
    // Threshold for plant to shift to new state
    private int plantStatusChangeThreshold = 5;
    

    // Optimal ranges for this plant
    private int[] goodLightRange = { 80, 100 };
    private int[] goodWaterRange = { 25, 75 };
    private int[] goodNutrientRange = { 25, 75 };

    // Current levels for this plant
    public int lightLevel;
    public int waterLevel;
    public int nutrientLevel;
    
    void Start()
    {
        sunPosition = GameObject.Find("Directional Light").transform;
        CheckEnvironment();
    }

    void Update()
    {
        timeAccumulator += Time.deltaTime;
        if(timeAccumulator >= waitTime)
        {
            TimedUpdate();
            timeAccumulator -= waitTime;
        }
    }

    void TimedUpdate()
    {
        CheckEnvironment();
        UpdateHealthStatus();
    }

    void ApplyHealthStatusVisuals()
    {
        Material newMaterial;

        if (plantStatus == 0)
        {
            newMaterial = Resources.Load<Material>("Dead Plant");
            gameObject.GetComponent<Renderer>().material = newMaterial;
        }
        else if (plantStatus <= 3)
        {
            newMaterial = Resources.Load<Material>("Unhealthy Plant");
            gameObject.GetComponent<Renderer>().material = newMaterial;
        }
        else if (plantStatus >= 4)
        {
            newMaterial = Resources.Load<Material>("Healthy Plant");
            gameObject.GetComponent<Renderer>().material = newMaterial;
        }

    }

    void UpdateHealthStatus()
    {
        bool goodLight;
        bool goodWater;
        bool goodNutrient;

        // If lightLevel within goodLightRange
        if ((lightLevel >= goodLightRange[0]) && (lightLevel <= goodLightRange[1]))
        {
            goodLight = true;
        } 
        else
        {
            goodLight = false;
        }

        // If waterLevel within goodWaterRange
        if ((waterLevel >= goodWaterRange[0]) && (waterLevel <= goodWaterRange[1]))
        {
            goodWater = true;
        } 
        else
        {
            goodWater = false;
        }

        // If nutrientLevel within goodNutrientRange
        if ((nutrientLevel >= goodNutrientRange[0]) && (nutrientLevel <= goodNutrientRange[1]))
        {
            goodNutrient = true;
        } 
        else
        {
            goodNutrient = false;
        }

        // If all environmental conditions are good, +1 to state change. Otherwise, -1
        if (goodLight && goodNutrient && goodWater)
        {
            plantTransitionTracker += 1;
        }
        else
        {
            plantTransitionTracker -= 1;
        }

        // Transition Tracker greater than status change threshold
        if (plantTransitionTracker >= plantStatusChangeThreshold)
        {
            // In "Optimal" State
            if (plantStatus >= 5)
            {
                plantStatus = 5;
                plantTransitionTracker = plantStatusChangeThreshold;
            }
            else
            {
                plantStatus += 1;
                plantTransitionTracker = 1;
            }
        }
        else if (plantTransitionTracker <= 0)
        {
            // In "dead" state
            if (plantStatus <= 0)
            {
                plantStatus = 0;
                plantTransitionTracker = 0;
            }
            else
            {
                plantStatus -= 1;
                plantTransitionTracker = 1;
            }
        }
        ApplyHealthStatusVisuals();
    }

    void CheckEnvironment()
    {
        GetSunLevel();
        GetWaterLevel();
        GetNutrientLevel();
    }

    // Get level of sunlight. 
    void GetSunLevel()
    {
        Ray plantToSun = new Ray(transform.position, sunPosition.position - transform.position);
        RaycastHit obstruction;
        if (Physics.Raycast(plantToSun, out obstruction))
        {
            lightLevel = 50;
        }
        else
        {
            lightLevel = 100;
        }
    }

    //Water Level of area 
    // 0 - 100, 0 = dry, 100 = innundated
    void GetWaterLevel()
    {
        waterLevel = 50;
    }

    //Nutrient Level of area 
    // 0 - 100, 0 = none, 100 = max concentration
    void GetNutrientLevel()
    {
        nutrientLevel = 50;
    }
}
