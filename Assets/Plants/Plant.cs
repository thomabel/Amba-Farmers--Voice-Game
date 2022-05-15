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
    private GameObject terrain;

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
    private float healthAccumulator = 0.5f;
    private float fruitAccumulator = 0f;
    public float fruitWeightScale = 1f;

    // -- ENVIRONMENTAL CONDITIONS --
    public float lightLevel;
    public bool lightLoving;

    public float waterLevel;
    public float waterUpperLimit;
    public float waterLowerLimit;

    public float nutrientLevel;
    public float nutrientUpperLimit;
    public float nutrientLowerLimit;

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
    public int growthTimeModifier = 5;
    private float growthTimeAccumulator = 0f;


    void Start()
    {
        sunPosition = GameObject.Find("Directional Light").transform;
        terrain = GameObject.Find("Terrain");
        TimedUpdate();
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

    // Fires every <waitTime> seconds
    void TimedUpdate()
    {
        CheckEnvironment();
        HealthUpdate();
        GrowthUpdate();
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
         
    // TODO: link growth to time event system 
    // Temporary: GrowthUpdate() increments the currentGrowthStage of the plant
    // This function should be called from TimedUpdate() which fires after <waitTime> seconds
    // <growthTimeModifier> will further scale the time it takes to transition between growth phases
    void GrowthUpdate()
    {
        growthTimeAccumulator += 1;

        if (growthTimeAccumulator >= growthTimeModifier)
        {
            if (currentGrowthStage != growthStages.Harvest)
            {
                currentGrowthStage++;
                growthTimeAccumulator = 0f;
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
        TerrainData.Data currentConditions = terrain.GetComponent<TerrainData>().GetTileData(gameObject.transform.position);
        waterLevel = currentConditions.water;
        nutrientLevel = currentConditions.nutrients;
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

    public void YellAtHour()
    {
        Debug.Log("HOURRRR INVOKE");
    }
}


