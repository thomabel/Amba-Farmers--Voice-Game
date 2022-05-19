using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour, IInteractable
{
    public enum Cleanliness { Filthy, Poor, Okay, Good, Clean };
    [SerializeField] public Animal.Species species;
    [SerializeField] public int animalCapacity;
    [SerializeField] public float foodCapacity;
    [SerializeField] public float waterCapacity;

    [SerializeField] public Inventory shelterInv;       // Inventory for this shelter
    [SerializeField] public List<Animal> population;    // List of animals currently occupying this shelter
    [SerializeField] public float foodStock;            // Food in shelter in kg
    [SerializeField] public float waterStock;           // Water in shelter in liters

    void Start()
    {
        population = new List<Animal>();
        foodStock = 0f;
        waterStock = 0f;
    }

    void IInteractable.Interact(GameObject other)
    {

    }

    public List<Animal> GetPopulationList()
    {
        return population;
    }

    public float ConsumeFood(float amountConsumed)
    {
        float foodLeftover = foodStock - amountConsumed;
        // check inventory for food type and amount
        // remove required amount or as much as possible
        // just using number in foodStock for now
        if (foodLeftover >= 0)
        {
            foodStock -= amountConsumed;
            return amountConsumed;
        }
        else
        {
            foodStock = 0;
            return amountConsumed + foodLeftover;
        }
    }

    public float ConsumeWater(float amountConsumed)
    {
        float waterLeftover = waterStock - amountConsumed;
        // check inventory for food type and amount
        // remove required amount or as much as possible
        // just using number in foodStock for now
        if (waterLeftover >= 0)
        {
            waterStock -= amountConsumed;
            return amountConsumed;
        }
        else
        {
            waterStock = 0;
            return amountConsumed + waterLeftover;
        }
    }

    public int GetShelterPop()
    {
        return population.Capacity;
    }

    public int GetShelterSpace() 
    {
        return population.Capacity - animalCapacity;
    }

    public bool AddAnimal(Animal animal)
    {
        bool success = false;

        if (GetShelterSpace() > 0)
        {
            if (animal.species == species)
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
