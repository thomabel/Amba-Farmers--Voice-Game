using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Animals/Animal", order = 1)]
public class AnimalSO : ScriptableObject
{
    public enum Species { Pig, Goat, Chicken, Rabbit };
    public enum Diet { Herbivore, Omnivore }
    public enum Sex { Male, Female };
    public enum Age { Baby, Adolescent, Adult };
    public enum Hunger { Starving, Hungry, Good, Full };
    public enum Thirst { Dehydrated, Thirsty, Good, Full };
    public enum Health { Ill, Poor, Good, Healthy };
    public enum Happiness { Sad, Unhappy, Content, Good, Happy };

    public Species _species { get; }    // What species is this animal?
    public Diet _diet { get; }          // What kind of diet does this animal have (will change once food gets implemented)?
    public float _weightMin { get; }    // Lower bound for weight of this animal
    public float _weightMax { get; }    // Upper bound for weight of this animal

    public float _consumptionRateFood { get; }      // How much does this animal eat per day in kg?
    public float _consumptionRateWater { get; }     // How much does this animal drink per day in liters?

    public float METER_MAX = 100f;
    public float METER_HIGH = 75f;
    public float METER_MID = 50f;
    public float METER_LOW = 25f;
    public float METER_MIN = 0f;
}