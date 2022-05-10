using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantVisuals : MonoBehaviour
{
    public Material seedlingMaterial;
    public Material vegetation1Material;
    public Material vegetation2Material;
    public Material floweringMaterial;
    public Material fruitingMaterial;
    public Material harvestMaterial;

    private Plant.growthStages currentGrowthStage = Plant.growthStages.Seedling;
    private bool firstTime = true;
    
    // Update is called once per frame
    void Update()
    {
        if(firstTime)
        {
            currentGrowthStage = transform.parent.gameObject.GetComponent<Plant>().currentGrowthStage;
            UpdateVisuals(currentGrowthStage);
            firstTime = false;
        }

        Plant.growthStages newGrowthStage = transform.parent.gameObject.GetComponent<Plant>().currentGrowthStage;

        if (!newGrowthStage.Equals(currentGrowthStage))
        {
            UpdateVisuals(newGrowthStage);
            currentGrowthStage = newGrowthStage;
        }
    }

    private void UpdateVisuals(Plant.growthStages growthStage)
    {
        switch (growthStage)
        {
            case Plant.growthStages.Seedling:
                {
                    GetComponent<Billboard>().ChangeMaterial(seedlingMaterial);
                    break;
                }
            case Plant.growthStages.Vegetative1:
                {
                    GetComponent<Billboard>().ChangeMaterial(vegetation1Material);
                    break;
                }
            case Plant.growthStages.Vegetative2:
                {
                    GetComponent<Billboard>().ChangeMaterial(vegetation2Material);
                    break;
                }
            case Plant.growthStages.Flowering:
                {
                    GetComponent<Billboard>().ChangeMaterial(floweringMaterial);
                    break;
                }
            case Plant.growthStages.Fruiting:
                {
                    GetComponent<Billboard>().ChangeMaterial(fruitingMaterial);
                    break;
                }
            case Plant.growthStages.Harvest:
                {
                    GetComponent<Billboard>().ChangeMaterial(harvestMaterial);
                    break;
                }
        }

    }

}
