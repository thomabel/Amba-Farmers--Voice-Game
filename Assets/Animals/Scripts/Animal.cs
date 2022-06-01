using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animal : MonoBehaviour, IInteractable
{
    public enum Sex { Female, Male };
    public enum Age { Baby, Adolescent, Adult };
    public enum Hunger { Starving, Hungry, Good, Full };
    public enum Thirst { Dehydrated, Thirsty, Good, Full };
    public enum Health { Ill, Poor, Good, Healthy };
    public enum Happiness { Depressed, Unhappy, Content, Happy };
    
    public static float STATUS_MAX = 100f;
    public static float STATUS_HIGH = 80f;
    public static float STATUS_MID = 40f;
    public static float STATUS_LOW = 20f;

    [SerializeField] public float weightMin;                // Lower bound for weight of this animal
    [SerializeField] public float weightMax;                // Upper bound for weight of this animal

    [SerializeField] public float decayRateFood;            // How quickly does this animal get hungry?
    [SerializeField] public float decayRateWater;           // How quickly does this animal get thirsty?
    [SerializeField] public float consumptionRateFood;      // How much does this animal eat per day in kg?
    [SerializeField] public float consumptionRateWater;     // How much does this animal drink per day in liters?

    [SerializeField] public Base.GoodType species;          // Species of this animal
    [SerializeField] public Sex sex;                        // Sex of this animal
    [SerializeField] public int age;                        // Animal's current age in days
    [SerializeField] public float hunger;                   // Animal's current hunger
    [SerializeField] public float thirst;                   // Animal's current thirst
    [SerializeField] public float health;                   // Animal's current health
    [SerializeField] public float happiness;                // Animal's current happiness
    [SerializeField] public float weight;                   // Animal's current weight in kg

    [SerializeField] public Shelter shelter;                // Shelter this animal belongs to

    // InitAnimal is used to set the starting values of the animal.
    // givenAge sets the animal's age in days
    // givenSex sets the animal's sex. This is optional and will be randomly assigned if not selected.
    public void InitAnimal(int givenAge, int givenSex = -1)
    {
        if (givenSex == 0 || givenSex == 1)
            sex = (Sex) givenSex;
        else
            sex = (Sex) Random.Range(0, 1);
        age = givenAge;
        hunger = Random.Range(50f, 100f);
        thirst = Random.Range(50f, 100f);
        health = 100f;
        happiness = 100f;
    }

    void IInteractable.Interact(GameObject x) 
    {
        Debug.Log("You pet the pig.");
    }

    private void Eat()
    {
        if (shelter != null)
        {
            float amountConsumed = shelter.RemoveFood(consumptionRateFood);     // amount of food actually eaten
            if (amountConsumed > 0)
            {
                if (amountConsumed == consumptionRateFood)
                    hunger = STATUS_MAX;
                else
                    hunger += (STATUS_MAX - hunger) * (amountConsumed / consumptionRateFood);
                weight += amountConsumed;
            }
            else
                if (GetHungerStatus() == Hunger.Starving)
                {
                    weight -= consumptionRateFood / 2;
                }
        }
    }

    private void Drink()
    {
        if (shelter != null)
        {
            float amountConsumed = shelter.RemoveWater(consumptionRateWater);     // amount of food actually eaten
            if (amountConsumed > 0)
            {
                if (amountConsumed == consumptionRateWater)
                    thirst = STATUS_MAX;
                else
                    thirst += (STATUS_MAX - thirst) * (amountConsumed / consumptionRateWater);
            }
        }
    }

    public void DecayHungerAndThirst()
    {
        hunger -= decayRateFood;
        if (hunger < 0f)
            hunger = 0f;
        thirst -= decayRateWater;
        if (thirst < 0f)
            thirst = 0f;
    }

    public void HungerThirstCheck()
    {
        if (GetHungerStatus() <= Hunger.Hungry)
            Eat();
        if (GetThirstStatus() <= Thirst.Thirsty)
            Drink();
    }

    public void AgeUp()
    {
        age += 1;
    }

    void Grow()
    {
        // change consumption rate based on current age and weight
    }

    public float GetWeight()
    {
        return weight;
    }

    public Hunger GetHungerStatus()
    {
        if (hunger >= STATUS_HIGH)
            return Hunger.Full;
        else if (hunger >= STATUS_MID)
            return Hunger.Good;
        else if (hunger >= STATUS_LOW)
            return Hunger.Hungry;
        else
            return Hunger.Starving;
    }
    
    public Thirst GetThirstStatus()
    {
        if (thirst >= STATUS_HIGH)
            return Thirst.Full;
        else if (thirst >= STATUS_MID)
            return Thirst.Good;
        else if (thirst >= STATUS_LOW)
            return Thirst.Thirsty;
        else
            return Thirst.Dehydrated;
    }

    public Health GetHealthStatus()
    {
        if (health >= STATUS_HIGH)
            return Health.Healthy;
        else if (health >= STATUS_MID)
            return Health.Good;
        else if (health >= STATUS_LOW)
            return Health.Poor;
        else
            return Health.Ill;
    }

    public Happiness GetHappinessStatus()
    {
        if (happiness >= STATUS_HIGH)
            return Happiness.Happy;
        else if (happiness >= STATUS_MID)
            return Happiness.Content;
        else if (happiness >= STATUS_LOW)
            return Happiness.Unhappy;
        else
            return Happiness.Depressed;
    }
}