using UnityEngine;
using System.Collections.Generic;

public class Market : MonoBehaviour
{
    public List<Inventory> Inventories;
    public List<MarketWrapper> Reference;
    public Dictionary<Financials.GoodType, MarketWrapper> Comparator;

    public struct Sellable
    {
        public MarketWrapper wrap;
        public Inventory inv;
        public int index;
    }
    public List<Sellable> Sellables;

    public void PopulateSellables()
    {
        foreach (Inventory i in Inventories)
        {
            foreach (GameObject g in i)
            {
                var s = g.GetComponent<Plant>();
                if (s != null)
                {
                    //Sellables.Add(g);

                    // Check enum here to match type?
                }
            }
        }
    }
}
