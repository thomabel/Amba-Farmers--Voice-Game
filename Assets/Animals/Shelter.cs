using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour, IInteractable
{
    [SerializeField] public ShelterSO shelterData;
    [SerializeField] public Inventory shelterInv;       // Inventory for this shelter

    [SerializeField] public float foodStock;            // Food in shelter in kg
    [SerializeField] public float waterStock;           // Water in shelter in liters

    [SerializeField] public List<Animal> population;    // List of animals currently occupying this shelter

    void Start()
    {
        population = new List<Animal>();
        foodStock = 0f;
        waterStock = 0f;
    }

    void IInteractable.Interact()
    {
        
    }

    void UpdateFoodWaterStock() {

    }

    public int GetShelterPop()
    {
        return population.Capacity;
    }

    public bool CheckShelterFull() 
    {
        if (population.Capacity == shelterData._animalCapacity)
            return true;
        else
            return false;
    }

    public bool AddAnimal(Animal animal)
    {
        bool success = false;

        if (CheckShelterFull() == true)
        {
            if (animal.animalData._species == shelterData._species)
            {
                population.Add(animal);
                success = true;
            }
        }

        return success;
    }

    public bool RemoveAnimal(Animal animal)
    {
        return population.Remove(animal);
    }
}
