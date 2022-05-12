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

    private Plant parentPlant;
    private Billboard billboard;
    private Plant.growthStages currentGrowthStage = Plant.growthStages.Seedling;
    private bool firstTime = true;

    void Start()
    {
        billboard = GetComponent<Billboard>();
        parentPlant = transform.parent.gameObject.GetComponent<Plant>();
    }
    // Update is called once per frame
    void Update()
    {
        if(firstTime)
        {
            currentGrowthStage = parentPlant.currentGrowthStage;
            UpdateVisuals(currentGrowthStage);
            firstTime = false;
        }

        Plant.growthStages newGrowthStage = parentPlant.currentGrowthStage;

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
                    billboard.ChangeMaterial(seedlingMaterial);
                    break;
                }
            case Plant.growthStages.Vegetative1:
                {
                    billboard.ChangeMaterial(vegetation1Material);
                    break;
                }
            case Plant.growthStages.Vegetative2:
                {
                    billboard.ChangeMaterial(vegetation2Material);
                    break;
                }
            case Plant.growthStages.Flowering:
                {
                    billboard.ChangeMaterial(floweringMaterial);
                    break;
                }
            case Plant.growthStages.Fruiting:
                {
                    billboard.ChangeMaterial(fruitingMaterial);
                    break;
                }
            case Plant.growthStages.Harvest:
                {
                    billboard.ChangeMaterial(harvestMaterial);
                    break;
                }
        }

    }

}
