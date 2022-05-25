using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour, IInteractable
{
    public enum Cleanliness { Filthy, Poor, Okay, Good, Clean };
    
    [SerializeField] public List<GameObject> spawnPositions;
    [SerializeField] public GameObject foodTrough;
    [SerializeField] public GameObject waterTrough;
    private Trough food;
    private Trough water;

    [SerializeField] public Base.GoodType species;      // Type of animal housed in this shelter
    [SerializeField] public GameObject animalPrefab;    // Prefab for animal
    [SerializeField] public int animalCapacity;         // Number of animals this shelter can hold
    [SerializeField] public float foodCapacity;         // Amount of food this shelter can store
    [SerializeField] public float waterCapacity;        // Amount of water this shelter can store

    [SerializeField] public Inventory shelterInv;       // Inventory for this shelter

    [SerializeField] public int freeSpace;
    [SerializeField] public int currentPopulation;
    [SerializeField] public List<GameObject> population;    // List of animals currently occupying this shelter
    
    void Start()
    {
        population = new List<GameObject>();
        currentPopulation = GetShelterPop();
        freeSpace = GetShelterSpace();
        food = foodTrough.GetComponent<Trough>();
        water = waterTrough.GetComponent<Trough>();
        food.SetCapacity(foodCapacity);
        water.SetCapacity(waterCapacity);
    }

    void IInteractable.Interact(GameObject other)
    {

    }

    public List<GameObject> GetPopulationList()
    {
        return population;
    }

    public float RemoveFood(float amount)
    {
        if (food)
            return food.EmptyAmount(amount);
        else
        {
            Debug.Log("No food trough found for shelter " + this.GetInstanceID());
            return 0;
        }
    }

    public float RemoveWater(float amount)
    {
        if (water)
            return water.EmptyAmount(amount);
        else
        {
            Debug.Log("No water trough found for shelter " + this.GetInstanceID());
            return 0;
        }
    }

    public int GetShelterPop()
    {
        currentPopulation = 0;

        foreach(GameObject animal in population)
        {
            currentPopulation += 1;
        }

        return currentPopulation;
    }

    public int GetShelterSpace() 
    {
        return animalCapacity - currentPopulation;
    }

    public bool AddAnimal()
    {
        bool success = false;

        if (GetShelterSpace() > 0)
        {
            foreach (GameObject spawner in spawnPositions)
            {
                if (spawner.active == false)
                {
                    spawner.SetActive(true);
                    GameObject newAnimal = Instantiate(animalPrefab, spawner.transform.position, Quaternion.Euler(0, 90, 0));
                    newAnimal.GetComponent<Animal>().InitAnimal(0);
                    newAnimal.transform.SetParent(spawner.transform);
                    population.Add(newAnimal);
                    currentPopulation += 1;
                    freeSpace -= 1;
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
            GameObject spawner = animal.transform.parent.gameObject;
            //animal.transform.SetParent(null);
            Destroy(animal);
            spawner.SetActive(false);
            currentPopulation -= 1;
            freeSpace += 1;
            success = true;
        }
        
        return success;
    }
}