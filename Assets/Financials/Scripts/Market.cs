using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "market",
    menuName = "Financials/Market"
    )]
public class Market : ScriptableObject
{
    public List<MarketWrapper> Items;
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
