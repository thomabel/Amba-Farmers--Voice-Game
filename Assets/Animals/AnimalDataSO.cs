using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "AnimalData", menuName = "Animals/Animal", order = 0)]
public class AnimalDataSO : ScriptableObject
{
    public enum Species { Pig, Goat, Chicken, Rabbit };
    public enum Diet { Herbivore, Omnivore }
    public enum Sex { Male, Female };
    public enum Age { Baby, Adolescent, Adult };
    public enum Hunger { Starving, Hungry, Good, Full };
    public enum Thirst { Dehydrated, Thirsty, Good, Full };
    public enum Health { Ill, Poor, Good, Healthy };
    public enum Happiness { Sad, Unhappy, Content, Good, Happy };
}