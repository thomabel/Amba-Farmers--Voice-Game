using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IInteractable
{
    public enum Species { Pig, Goat, Chicken, Rabbit };
    public enum Diet { Herbivore, Omnivore }
    public enum Sex { Male, Female };
    public enum Age { Baby, Adolescent, Adult };
    public enum Hunger { Starving, Hungry, Good, Full };
    public enum Thirst { Dehydrated, Thirsty, Good, Full };
    public enum Health { Ill, Poor, Good, Healthy };
    public enum Happiness { Depressed, Unhappy, Content, Happy };
    public enum Stats { Species, Sex, Age, Hunger, Thirst, Health, Happiness, Weight };

    public static float STATUS_MAX = 100f;
    public static float STATUS_HIGH = 80f;
    public static float STATUS_MID = 40f;
    public static float STATUS_LOW = 20f;

    public Diet diet;          // What kind of diet does this animal have (will change once food gets implemented)?
    [SerializeField] public float weightMin;    // Lower bound for weight of this animal
    [SerializeField] public float weightMax;    // Upper bound for weight of this animal

    [SerializeField] public float consumptionRateFood;     // How much does this animal eat per day in kg?
    [SerializeField] public float consumptionRateWater;     // How much does this animal drink per day in liters?
    [SerializeField] public int timesToEatPerDay;
    [SerializeField] public int timesToDrinkPerDay;

    [SerializeField] public Species species;        // Species of this animal
    [SerializeField] public Sex sex;                // Sex of this animal
    [SerializeField] public int age;                // Animal's current age in days
    [SerializeField] public float hunger;           // Animal's current hunger
    [SerializeField] public float thirst;           // Animal's current thirst
    [SerializeField] public float health;           // Animal's current health
    [SerializeField] public float happiness;        // Animal's current happiness
    [SerializeField] public float weight;           // Animal's current weight in kg
    [SerializeField] public Shelter shelter;        // Shelter this animal belongs to
    [SerializeField] public int id;                 // ID of this animal's GameObject
    [SerializeField] public int lastTimeEaten;      // Time when animal last ate

    [SerializeField] public FloatVariable seconds;

    void Start()
    {
        id = this.GetInstanceID();
        sex = (Sex) Random.Range(0,1);
        lastTimeEaten = (int) seconds.Value;
    }

    void Update()
    {
        if (seconds.Value - lastTimeEaten > 2)
        {
            this.AccumulateHungerThirst();
            if (GetHungerStatus() == Hunger.Hungry)
                this.Eat();
            if (GetThirstStatus() == Thirst.Thirsty)
                this.Drink();
        }
    }

    void IInteractable.Interact() 
    {
        float[] stats = GetStats();
        Debug.Log(id + " - Species: " + stats[(int) Stats.Species]);
        Debug.Log(id + " - Sex: " + stats[(int) Stats.Sex]);
        Debug.Log(id + " - Age: " + stats[(int) Stats.Age]);
        Debug.Log(id + " - Hunger: " + stats[(int) Stats.Hunger] + " " + GetHungerStatus().ToString());
        Debug.Log(id + " - Thirst: " + stats[(int) Stats.Thirst] + " " + GetThirstStatus().ToString());
        Debug.Log(id + " - Health: " + stats[(int) Stats.Health] + " " + GetHealthStatus().ToString());
        Debug.Log(id + " - Species: " + stats[(int) Stats.Happiness] + " " + GetHappinessStatus().ToString());
        Debug.Log(id + " - Weight: " + stats[(int) Stats.Weight] + "kg");
    }

    void Eat()
    {
        float foodValue = 20f;
        float fillValue = 40f;
        Debug.Log(id + " - Attempting to eat from shelter");
        if (shelter != null)
        {
            Debug.Log(id + " - Eating from shelter " + shelter.GetInstanceID());
            float eaten = shelter.ConsumeFood(consumptionRateFood / 2);     // amount of food actually eaten
            if (eaten > 0)
            {
                hunger += fillValue;
                if (hunger > 100f)
                    hunger = 100f;
                weight += foodValue;
            }
        }
    }

    void Drink()
    {
        float fillValue = 20f;
        Debug.Log(id + " - Attempting to drink from shelter");
        if (shelter != null)
        {
            Debug.Log(id + " - Drinking from shelter " + shelter.GetInstanceID());
            float drank = shelter.ConsumeWater(consumptionRateWater / 5);     // amount of food actually eaten
            if (drank > 0)
            {
                thirst += fillValue;
                if (thirst > 100f)
                    thirst = 100f;
            }
        }
    }

    void AccumulateHungerThirst()
    {
        float starveValue = 10f;
        float thirstValue = 10f;
        hunger -= starveValue;
        if (hunger < 0f)
            hunger = 0f;
        thirst -= thirstValue;
        if (thirst < 0f)
            thirst = 0f;
    }

    private float[] GetStats()
    {
        float[] stats = { (float) species, (float) sex, age, hunger, thirst, health, happiness, weight };
        return stats;
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