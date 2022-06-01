using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trough : MonoBehaviour
{
    public float capacity = 100f;
    public float currentStock = 0f;

    public void SetCapacity(float amount)
    {
        capacity = amount;
    }

    public float GetStock()
    {
        return currentStock;
    }

    public float FillAmount(float amount)
    {
        float amountAdded = amount;

        if (currentStock + amount > capacity)
        {
            amountAdded = capacity - currentStock;
            currentStock = capacity;
        }
        else
            currentStock += amount;
        
        return amountAdded;
    }

    public float EmptyAmount(float amount)
    {
        float amountRemoved = 0f;

        if (currentStock - amount < 0)
        {
            amountRemoved = currentStock;
            currentStock = 0f;
        }
        else
            currentStock -= amount;

        return amountRemoved;
    }
}