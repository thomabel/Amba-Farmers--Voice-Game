using UnityEngine;
using System.Collections.Generic;

public class Market : MonoBehaviour
{
    [SerializeField] List<MarketWrapper> inventory;

    public List<MarketWrapper>.Enumerator GetEnumerator()
    {
        return inventory.GetEnumerator();
    }
    public MarketWrapper GetItem(int index)
    {
        return inventory[index];
    }
}
