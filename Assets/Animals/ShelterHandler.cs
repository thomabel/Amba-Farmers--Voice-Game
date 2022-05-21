using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterHandler : MonoBehaviour
{
    List<Shelter> shelters;
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
        UpdateShelterNumbers();
    }

    void UpdateShelterNumbers()
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

    public int GetCapacity(Base.GoodType animal)
    {
        switch(animal)
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

    public int GetPopulation(Base.GoodType animal)
    {
        switch(animal)
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

    public int GetSpace(Base.GoodType animal)
    {
        switch(animal)
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

    public bool PlaceAnimal(Base.GoodType animal)
    {
        bool success = false;

        foreach(Shelter s in shelters)
        {
            if (s.GetShelterSpace() > 0 && s.species == animal)
            {
                if (s.AddAnimal())
                {
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

        foreach(Shelter s in shelters)
        {
            if(RemoveAnimal(animal))
            {
                success = true;
                break;
            }
        }
        if (success == false)
            Debug.Log("RemoveAnimal(): Animal not found.");

        return success;
    }
}
