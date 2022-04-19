using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShelterData", menuName = "Animals/Shelter", order = 0)]
public class ShelterSO : ScriptableObject
{
    public enum Cleanliness { Filthy, Poor, Okay, Good, Clean };

    public AnimalSO.Species _species;
    public int _animalCapacity;
    public float _foodCapacity;
    public float _waterCapacity;
}
