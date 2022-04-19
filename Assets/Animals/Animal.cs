using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IInteractable
{
    [SerializeField] public AnimalSO animalData;
    [SerializeField] public AnimalSO.Sex sex;       // Sex of this animal
    [SerializeField] public int age;                // Animal's current age in days
    [SerializeField] public float hunger;           // Animal's current hunger
    [SerializeField] public float thirst;           // Animal's current thirst
    [SerializeField] public float health;           // Animal's current health
    [SerializeField] public float happiness;        // Animal's current happiness
    [SerializeField] public float weight;           // Animal's current weight in kg
    [SerializeField] Shelter shelter;               // Shelter the animal belongs to
    [SerializeField] public int id;                 // ID of this animal's GameObject

    void IInteractable.Interact() 
    {
        Debug.Log(id + " - Hunger: " + GetHungerStatus().ToString());
        Debug.Log(id + " - Thirst: " + GetThirstStatus().ToString());
        Debug.Log(id + " - Health: " + GetHealthStatus().ToString());
    }

    public AnimalSO.Hunger GetHungerStatus()
    {
        return (AnimalSO.Hunger) (hunger / 25);
    }
    
    public AnimalSO.Thirst GetThirstStatus()
    {
        return (AnimalSO.Thirst) (thirst / 25);
    }

    public AnimalSO.Health GetHealthStatus()
    {
        if (health >= animalData.METER_HIGH)
            return AnimalSO.Health.Healthy;
        else if (health >= animalData.METER_MID)
            return AnimalSO.Health.Good;
        else if (health >= animalData.METER_LOW)
            return AnimalSO.Health.Poor;
        else
            return AnimalSO.Health.Ill;
    }

    void Start() 
    {
        id = this.gameObject.GetInstanceID();
        Debug.Log(id + " - I am a " + animalData._species.ToString() + "!");
    }

    void Update() 
    {
        
    }
}