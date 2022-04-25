using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "market",
    menuName = "Financials/Market"
    )]
public class Market : ScriptableObject
{
    public List<MarketWrapper> Plants;
    public List<MarketWrapper> Animals;
    public List<MarketWrapper> Tools;

    public List<Inventory> Inventories;
    public List<GameObject> Sellables;

    public void PopulateSellables()
    {
        foreach (Inventory i in Inventories)
        {
            foreach (GameObject g in i)
            {
                var s = g.GetComponent<Plant>();
                if (s != null)
                {
                    Sellables.Add(g);

                    // Check enum here to match type?
                }
            }
        }
    }

    public struct a
    {
        float r;
    }
}
