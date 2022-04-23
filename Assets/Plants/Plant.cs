using UnityEngine;
using System.Collections.Generic;

public class Plant: MonoBehaviour
{
    // The position of the sun
    private Transform sunPosition;

    // Variables for the timed function
    private float timeAccumulator = 0.0f;
    private float waitTime = 5.0f;

    // -- PLANT HEALTH --
    // List of possible health statuses for the plant
    public enum healthStatus 
    { 
        Dead, 
        VeryUnhealthy, 
        Unhealthy, 
        Fine, 
        Healthy, 
        Optimal 
    };
    // Current status of the plant
    public healthStatus currentHealthStatus = healthStatus.Fine;
    // Steps to next plant status
    public int plantTransitionTracker = 3;
    // Threshold for plant to shift to new state
    private int plantStatusChangeThreshold = 5;

    // -- ENVIRONMENTAL CONDITIONS --
    // Optimal ranges for this plant
    private int[] goodLightRange = { 80, 100 };
    private int[] goodWaterRange = { 25, 75 };
    private int[] goodNutrientRange = { 25, 75 };
    // Current levels for this plant
    public int lightLevel;
    public int waterLevel;
    public int nutrientLevel;

    // -- PLANT GROWTH -- 
    // Plant Growth Stages
    public enum growthStages
    {
        Seedling,
        Vegetative,
        Flowering,
        Fruiting,
        Harvest
    };

    public growthStages currentGrowthStage = growthStages.Seedling;
    public int growthAccum = 0;
    public int growthLevelThreshold = 5;


    void Start()
    {
        sunPosition = GameObject.Find("Directional Light").transform;
        CheckEnvironment();
        UpdateHealthStatus();
        UpdateGrowthStage();
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
        UpdateGrowthStage();
    }

    void ApplyHealthStatusVisuals()
    {
        Material newMaterial;

        if (currentHealthStatus == healthStatus.Dead)
        {
            newMaterial = Resources.Load<Material>("Dead Plant");
            gameObject.GetComponent<Renderer>().material = newMaterial;
        }
        else if (currentHealthStatus <= healthStatus.Fine)
        {
            newMaterial = Resources.Load<Material>("Unhealthy Plant");
            gameObject.GetComponent<Renderer>().material = newMaterial;
        }
        else if (currentHealthStatus >= healthStatus.Healthy)
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
            if (currentHealthStatus >= healthStatus.Optimal)
            {
                currentHealthStatus = healthStatus.Optimal;
                plantTransitionTracker = plantStatusChangeThreshold;
            }
            else
            {
                ++currentHealthStatus;
                plantTransitionTracker = 1;
            }
        }
        else if (plantTransitionTracker <= 0)
        {
            // In "dead" state
            if (currentHealthStatus <= healthStatus.Dead)
            {
                currentHealthStatus = healthStatus.Dead;
                plantTransitionTracker = 1;
            }
            else
            {
                --currentHealthStatus;
                plantTransitionTracker = plantStatusChangeThreshold;
            }
        }
        ApplyHealthStatusVisuals();
    }

    void UpdateGrowthStage()
    {
        int incrementAmount = 0;
        // Increment growthAccum according to plant status
        switch (currentHealthStatus)
        {
            case healthStatus.Optimal:
                {
                    incrementAmount = 3;
                    break;
                }
            case healthStatus.Healthy:
                {
                    incrementAmount = 2;
                    break;
                }
            case healthStatus.Fine:
                {
                    incrementAmount = 1;
                    break;
                }
        }

        growthAccum += incrementAmount;

        if (growthAccum >= growthLevelThreshold)
        {
            ++currentGrowthStage;
            growthAccum = 0;
        }
        ApplyGrowthLevelVisuals();
    }

    void ApplyGrowthLevelVisuals()
    {
        switch (currentGrowthStage)
        {
            case growthStages.Seedling:
                {
                    float xPos = transform.position.x;
                    float zPos = transform.position.z;
                    float yPosNew = 0.1f;
                    transform.localScale = new Vector3(0.1f, yPosNew, 0.1f);
                    transform.position = new Vector3(xPos, yPosNew, zPos);
                    break;
                }
            case growthStages.Vegetative:
                {
                    float xPos = transform.position.x;
                    float zPos = transform.position.z;
                    float yPosNew = 0.1f;
                    transform.localScale = new Vector3(0.1f, yPosNew, 0.1f);
                    transform.position = new Vector3(xPos, yPosNew, zPos);
                    break;
                }
            case growthStages.Flowering:
                {
                    float xPos = transform.position.x;
                    float zPos = transform.position.z;
                    float yPosNew = 0.5f;
                    transform.localScale = new Vector3(0.1f, yPosNew, 0.1f);
                    transform.position = new Vector3(xPos, yPosNew, zPos);
                    break;
                }
            case growthStages.Fruiting:
                {
                    GameObject fruit = transform.GetChild(0).gameObject;
                    fruit.SetActive(true);
                    Material newMaterial = Resources.Load<Material>("Healthy Plant");
                    fruit.GetComponent<Renderer>().material = newMaterial;
                    float xPos = transform.position.x;
                    float zPos = transform.position.z;
                    float yPosNew = 0.9f;
                    transform.localScale = new Vector3(0.1f, yPosNew, 0.1f);
                    transform.position = new Vector3(xPos, yPosNew, zPos);
                    break;
                }
            case growthStages.Harvest:
                {
                    GameObject fruit = transform.GetChild(0).gameObject;
                    fruit.SetActive(true);
                    Material newMaterial = Resources.Load<Material>("Corn");
                    fruit.GetComponent<Renderer>().material = newMaterial;
                    float xPos = transform.position.x;
                    float zPos = transform.position.z;
                    float yPosNew = 1.0f;
                    transform.localScale = new Vector3(0.1f, yPosNew, 0.1f);
                    transform.position = new Vector3(xPos, yPosNew, zPos);
                    break;
                }
        }
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


