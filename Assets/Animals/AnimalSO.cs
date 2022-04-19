using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Animals/Animal", order = 0)]
public class AnimalSO : ScriptableObject
{
    public AnimalDataSO.Species _species { get; }
    public AnimalDataSO.Diet _diet { get; }
    public float _weightMin { get; }
    public float _weightMax {  get; }

    public float METER_MAX = 100f;
    public float METER_HIGH = 75f;
    public float METER_MID = 50f;
    public float METER_LOW = 25f;
    public float METER_MIN = 0f;
}