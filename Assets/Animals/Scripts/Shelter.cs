using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour, IInteractable
{
    public enum Cleanliness { Filthy, Poor, Okay, Good, Clean };
    [SerializeField] public GameObject animalPrefab;    // Prefab for animal

    [SerializeField] public Base.GoodType species;      // Type of animal housed in this shelter
    [SerializeField] public int animalCapacity;         // Number of animals this shelter can hold
    [SerializeField] public float foodCapacity;         // Amount of food this shelter can store
    [SerializeField] public float waterCapacity;        // Amount of water this shelter can store
    [SerializeField] public List<(GameObject, bool)> spawnPositions;

    [SerializeField] public Inventory shelterInv;       // Inventory for this shelter
    [SerializeField] public List<GameObject> population;    // List of animals currently occupying this shelter
    [SerializeField] public float foodStock;            // Current food in shelter in kg
    [SerializeField] public float waterStock;           // Current water in shelter in liters

    void Start()
    {
        population = new List<GameObject>();
        foodStock = 0f;
        waterStock = 0f;
    }

    void IInteractable.Interact(GameObject other)
    {

    }

    public List<GameObject> GetPopulationList()
    {
        return population;
    }

    public float RemoveFood(float amountConsumed)
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

    public float RemoveWater(float amountConsumed)
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
        return animalCapacity - population.Capacity;
    }

    public bool AddAnimal()
    {
        bool success = false;

        if (GetShelterSpace() > 0)
        {
            foreach((GameObject spawnPos, bool occupied) in spawnPositions)
            {
                if (occupied == false)
                {
                    GameObject newAnimal = Instantiate(animalPrefab, spawnPos.transform.position, Quaternion.Euler(-90,0,0));
                    newAnimal.GetComponent<Animal>().InitAnimal(0);
                    success = true;
                    break;
                }
            }
        }

        return success;
    }

    public bool RemoveAnimal(GameObject animal)
    {
        bool success = false;
        
        if(population.Remove(animal))
        {
            Destroy(animal);
            success = true;
        }
        
        return success;
    }
}