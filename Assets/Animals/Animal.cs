using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IInteractable
{
    [SerializeField] public AnimalSO animalData;
    [SerializeField] public AnimalDataSO.Sex sex;
    [SerializeField] public int age;
    [SerializeField] public float hunger;
    [SerializeField] public float thirst;
    [SerializeField] public float health;
    [SerializeField] public float happiness;
    [SerializeField] public float weight;
    //GameObject shelter;
    [SerializeField] public int id;

    void IInteractable.Interact() 
    {
        Debug.Log(id + " - Hunger: " + GetHungerLevel().ToString());
    }

    public AnimalDataSO.Hunger GetHungerLevel()
    {
        return (AnimalDataSO.Hunger) (hunger / 25);
    }
    
    public AnimalDataSO.Thirst GetThirstLevel()
    {
        return (AnimalDataSO.Thirst) (thirst / 25);
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