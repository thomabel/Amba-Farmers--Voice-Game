using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New InventoryList", menuName = "InventoryList")]
public class InventoryList : ScriptableObject
{
    [SerializeField]
    private List<MarketWrapper> Inventory;

    public void Add(MarketWrapper BoughtCard)
    {
        /*
        Card tmp = CreateInstance<Card>();
        tmp.name = BoughtCard.name;
        tmp.quantity = BoughtCard.quantity;
        tmp.cost = 0;
        tmp.picture = null;
        tmp.gameobject = null;
        */
        if (!Inventory.Contains(BoughtCard))
            Inventory.Add(BoughtCard);
        
    }
    public int length()
    {
        return Inventory.Count;
    }
    public MarketWrapper FindCardIndex(int i)
    {
        return Inventory[i];
    }
    public bool CardExists(MarketWrapper checkCard)
    {
        return Inventory.Contains(checkCard);
    }
}
