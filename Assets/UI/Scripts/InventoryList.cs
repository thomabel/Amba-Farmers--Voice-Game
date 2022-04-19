using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class InventoryList : ScriptableObject
{
    [SerializeField]
    private List<Card> Inventory;

    public void Add(Card BoughtCard)
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
    public Card FindCardIndex(int i)
    {
        return Inventory[i];
    }
}
