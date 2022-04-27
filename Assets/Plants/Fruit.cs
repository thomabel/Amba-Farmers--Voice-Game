using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, IEquippable
{
    public float fruitMass;
    public string fruitName;

    void IEquippable.Use()
    {
        Debug.Log("Use Fruit Box");
    }
}
