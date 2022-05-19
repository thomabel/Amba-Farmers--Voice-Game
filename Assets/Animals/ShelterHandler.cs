using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterHandler : MonoBehaviour
{
    List<Shelter> shelters;
    int populationCapacity;
    int populationCurrent;
    int shelterSpacesAvailable;

    void Start()
    {
        shelters = new List<Shelter>(FindObjectsOfType<Shelter>());
        UpdateShelterNumbers();
    }

    void UpdateShelterNumbers()
    {
        foreach(Shelter s in shelters)
        {
            populationCapacity += s.animalCapacity;
            populationCurrent += s.GetShelterPop();
            shelterSpacesAvailable += s.GetShelterSpace();
        }
    }

    public int GetCapacity()
    {
        return populationCapacity;
    }

    public int GetPopulation()
    {
        return populationCurrent;
    }

    public bool PlaceAnimal(Animal animal)
    {
        bool success = false;

        foreach(Shelter s in shelters)
        {
            if (s.GetShelterSpace() > 0)
            {
                if (s.AddAnimal(animal))
                {
                    success = true;
                    break;
                }
            }
        }

        return success;
    }

    public bool RemoveAnimal(Animal animal)
    {
        bool success = false;



        return success;
    }
}
