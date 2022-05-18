using UnityEngine;
using System.Collections.Generic;

public class Plant: MonoBehaviour, IInteractable
{
    //The fruit that this plant produces
    public Base.GoodType fruitType;

    //The text name of the plant
    public string plantName;

    // The position of the sun
    private Transform sunPosition;
    // Terrain Data
    private TerrainData terrain;

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
    private float healthAccumulator = 0.5f;
    private float fruitAccumulator = 0f;
    public float fruitWeightScale = 1f;

    // -- ENVIRONMENTAL CONDITIONS --
    public float lightLevel;
    public bool lightLoving;

    public float waterLevel;
    public float waterUpperLimit;
    public float waterLowerLimit;
    public float waterHourlyConsumption;

    public float nutrientLevel;
    public float nutrientUpperLimit;
    public float nutrientLowerLimit;
    public float nutrientHourlyConsumption;

    // -- PLANT GROWTH -- 
    // Plant Growth Stages
    public enum growthStages
    {
        Seedling,
        Vegetative1,
        Vegetative2,
        Flowering,
        Fruiting,
        Harvest
    };

    public growthStages currentGrowthStage = growthStages.Seedling;
    private int daysSincePlanting = 0;
    public int daysUntilMaturity;

    // -- DEMO VARIABLES --
    public bool activateGrowthDemo = false;
    private float demoGrowthTimeAccumulator = 0;
    public float demoGrowthTimeModifier;
    // Variables for the time delay functionality
    private float demoDelayTimeAccumulator = 0.0f;
    private float demoSecondsBetweenUpdates = 5.0f;
    // Water/Nutrient self-initialization
    public bool activateDemoTerrainInit = false;


    void Start()
    {
        sunPosition = GameObject.Find("Directional Light").transform;
        terrain = GameObject.Find("Terrain").GetComponent<TerrainData>();

        // Demo Terrain Initialization
        if (activateDemoTerrainInit)
        {
            DemoTerrainInit();
        }
        // Growth Demo Mode
        if (activateGrowthDemo)
        {
            DemoDelayedUpdate();
        }

    }

    void Update()
    {
        // Growth Demo Mode
        if (activateGrowthDemo)
        {
            demoDelayTimeAccumulator += Time.deltaTime;
            if(demoDelayTimeAccumulator  >= demoSecondsBetweenUpdates)
            {
                DemoDelayedUpdate();
                demoDelayTimeAccumulator = 0;
            }
        }
    }

    // For use in the accelerated growtrh demo mode
    // Fires every <waitTime> seconds
    void DemoDelayedUpdate()
    {
        CheckEnvironment();
        HealthUpdate();
        ConsumeTerrainResources();
        DemoAcceleratedGrowthUpdate();
    }

    private void HealthUpdate()
    {
        bool goodWater = false;
        bool goodNutrients = false;
        bool goodLight = false;

        float hitAmount = .1f;

        float healthIncrement = 0f;

        // Determine if this plant likes current water conditions
        if (waterLevel >= waterLowerLimit)
        {
            if (waterLevel <= waterUpperLimit)
            {
                goodWater = true;
            }
        }

        // Determine if this plant likes current nutrient conditions
        if (nutrientLevel >= nutrientLowerLimit)
        {
            if (nutrientLevel <= nutrientUpperLimit)
            {
                goodNutrients = true;
            }
        }

        // Determine if this plant likes the current sunlight
        if (lightLoving)
        {
            if (lightLevel > .75f)
            {
                goodLight = true;
            }
        }
        else
        {
            if (lightLevel < .75f && lightLevel > .25f)
            {
                goodLight = true;
            }
        }

        // how much to increment health based on conditions
        healthIncrement += goodWater ? hitAmount : -hitAmount; 
        healthIncrement += goodNutrients ? hitAmount : -hitAmount; 
        healthIncrement += goodLight ? hitAmount : -hitAmount;

        // effect plant's health and fruit weight
        healthAccumulator += healthIncrement;
        fruitAccumulator += healthIncrement;

        // check if status needs changing
        if (currentHealthStatus != healthStatus.Dead)
        {
            if (healthAccumulator < 0f)
            {
                currentHealthStatus--;
                healthAccumulator = 0.9f;
            }
            else if (healthAccumulator > 1f)
            {
                if (currentHealthStatus != healthStatus.Optimal)
                {
                    currentHealthStatus++;
                    healthAccumulator = 0.1f;
                }
            }
        }
    } 
         
    // -- Demo Accelerated Growth Update Function 
    // Plants will grow at a rapid pace to demonstrate growth functionality
    void DemoAcceleratedGrowthUpdate()
    {
        demoGrowthTimeAccumulator += 1;

        if (demoGrowthTimeAccumulator >= demoGrowthTimeModifier)
        {
            if (currentGrowthStage != growthStages.Harvest)
            {
                currentGrowthStage++;
                demoGrowthTimeAccumulator = 0f;
            }
        }

    }

    // To be called daily
    void GrowthUpdate()
    {
        daysSincePlanting += 1;

        //Update growth stage every (daysUntilMaturity/ 5) days
        if (daysSincePlanting % (daysUntilMaturity / 5) == 0)
        {
            if (currentGrowthStage != growthStages.Harvest)
            {
                currentGrowthStage += 1;
            }
        }
    }

    void CheckEnvironment()
    {
        GetSunLevel();
        GetTerrainData();
    }

    // Get level of sunlight. 
    void GetSunLevel()
    {
        Ray plantToSun = new Ray(transform.position, sunPosition.position - transform.position);
        RaycastHit obstruction;
        if (Physics.Raycast(plantToSun, out obstruction))
        {
            lightLevel = .5f;
        }
        else
        {
            lightLevel = 1f;
        }
    }

    //Water Level of area 
    void GetTerrainData()
    {
        TerrainData.Data currentConditions = terrain.GetTileData(gameObject.transform.position);
        waterLevel = currentConditions.water;
        nutrientLevel = currentConditions.nutrients;
    }

    void ConsumeTerrainResources()
    {
        terrain.SetNutrients(gameObject.transform.position, waterLevel - waterHourlyConsumption);
        terrain.SetWater(gameObject.transform.position, nutrientLevel - nutrientHourlyConsumption);
    }

    void DemoTerrainInit()
    {
        terrain.SetNutrients(gameObject.transform.position, 1);
        terrain.SetWater(gameObject.transform.position, 1);
    }

    void IInteractable.Interact(GameObject with)
    {
        Harvest();
    }

    void Harvest()
    {
        if (currentGrowthStage != growthStages.Harvest)
        {
            return;
        }
        Debug.Log("Harvest");
        Vector3 fruitPlacement;
        fruitPlacement = transform.position;
        fruitPlacement += new Vector3(-1.0F, 0F, -1.0F);
        Fruit newFruitbasket = Instantiate(Resources.Load("Fruit", typeof(Fruit)), fruitPlacement, Quaternion.identity) as Fruit;
        newFruitbasket.qty.Value = fruitAccumulator;
        newFruitbasket.tl.Type = fruitType;
        newFruitbasket.name = plantName; 
        Destroy(gameObject);
    }

    public void HourlyUpdate()
    {
        // If event calls this function with Growth Demo Mode activated, just quit
        if (activateGrowthDemo) return; 

        CheckEnvironment();
        HealthUpdate();
        ConsumeTerrainResources();
    }

    public void DailyUpdate()
    {
        // If event calls this function with Growth Demo Mode activated, just quit
        if (activateGrowthDemo) return; 

        GrowthUpdate();
    }
}


