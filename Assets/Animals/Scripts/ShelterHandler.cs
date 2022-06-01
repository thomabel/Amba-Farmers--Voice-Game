using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterHandler : MonoBehaviour
{
    public List<Shelter> shelters;
    int populationCapacityPigs;
    int populationCurrentPigs;
    int shelterSpacesAvailablePigs;

    int populationCapacityGoats;
    int populationCurrentGoats;
    int shelterSpacesAvailableGoats;

    int populationCapacityChickens;
    int populationCurrentChickens;
    int shelterSpacesAvailableChickens;

    void Start()
    {
        shelters = new List<Shelter>(FindObjectsOfType<Shelter>());
        GetPopulationInfo();
    }

    void GetPopulationInfo()
    {
        foreach(Shelter s in shelters)
        {
            if (s.species == Base.GoodType.Animal_Pig)
            {
                populationCapacityPigs += s.animalCapacity;
                populationCurrentPigs += s.GetShelterPop();
                shelterSpacesAvailablePigs += s.GetShelterSpace();
            }
            else if (s.species == Base.GoodType.Animal_Goat)
            {
                populationCapacityGoats += s.animalCapacity;
                populationCurrentGoats += s.GetShelterPop();
                shelterSpacesAvailableGoats += s.GetShelterSpace();
            }
            else if (s.species == Base.GoodType.Animal_Chicken)
            {
                populationCapacityChickens += s.animalCapacity;
                populationCurrentChickens += s.GetShelterPop();
                shelterSpacesAvailableChickens += s.GetShelterSpace();
            }
        }
    }

    public int GetCapacity(Base.GoodType species)
    {
        switch(species)
        {
            case Base.GoodType.Animal_Pig:
                return populationCapacityPigs;
            case Base.GoodType.Animal_Goat:
                return populationCapacityGoats;
            case Base.GoodType.Animal_Chicken:
                return populationCapacityChickens;
            default:
                Debug.Log("ShelterHandler.GetCapacity(): No animal specified");
                return -1;
        }
    }

    public int GetPopulation(Base.GoodType species)
    {
        switch(species)
        {
            case Base.GoodType.Animal_Pig:
                return populationCurrentPigs;
            case Base.GoodType.Animal_Goat:
                return populationCurrentGoats;
            case Base.GoodType.Animal_Chicken:
                return populationCurrentChickens;
            default:
                Debug.Log("ShelterHandler.GetPopulation(): No animal specified");
                return -1;
        }
    }

    public int GetSpace(Base.GoodType species)
    {
        switch(species)
        {
            case Base.GoodType.Animal_Pig:
                return shelterSpacesAvailablePigs;
            case Base.GoodType.Animal_Goat:
                return shelterSpacesAvailableGoats;
            case Base.GoodType.Animal_Chicken:
                return shelterSpacesAvailableChickens;
            default:
                Debug.Log("ShelterHandler.GetSpace(): No animal specified");
                return -1;
        }
    }

    public bool PlaceAnimal(Base.GoodType species)
    {
        bool success = false;

        foreach(Shelter s in shelters)
        {
            if (s.GetShelterSpace() > 0 && s.species == species)
            {
                if (s.AddAnimal())
                {
                    success = true;
                    switch(s.species)
                {
                    case Base.GoodType.Animal_Pig:
                        populationCurrentPigs += 1;
                        shelterSpacesAvailablePigs -= 1;
                        break;
                    case Base.GoodType.Animal_Goat:
                        populationCurrentGoats += 1;
                        shelterSpacesAvailableGoats -= 1;
                        break;
                    case Base.GoodType.Animal_Chicken:
                        populationCurrentChickens += 1;
                        shelterSpacesAvailableChickens -= 1;
                        break;
                    default:
                        break;
                }
                    break;
                }
            }
        }
        if (success == false)
            Debug.Log("PlaceAnimal(): Unable to place animal.");

        return success;
    }

    public List<GameObject> GetEntirePopulationList()
    {
        List<GameObject> animals = new List<GameObject>();
        foreach(Shelter s in shelters)
        {
            foreach(GameObject a in s.GetPopulationList())
            animals.Add(a);
        }

        return animals;
    }

    public bool RemoveAnimal(GameObject animal)
    {
        bool success = false;

        foreach(Shelter s in shelters)
        {
            if(s.RemoveAnimal(animal))
            {
                success = true;
                switch(s.species)
                {
                    case Base.GoodType.Animal_Pig:
                        populationCurrentPigs -= 1;
                        shelterSpacesAvailablePigs += 1;
                        break;
                    case Base.GoodType.Animal_Goat:
                        populationCurrentGoats -= 1;
                        shelterSpacesAvailableGoats += 1;
                        break;
                    case Base.GoodType.Animal_Chicken:
                        populationCurrentChickens -= 1;
                        shelterSpacesAvailableChickens += 1;
                        break;
                    default:
                        break;
                }
                break;
            }
        }
        if (success == false)
            Debug.Log("RemoveAnimal(): Animal not found.");

        return success;
    }
}