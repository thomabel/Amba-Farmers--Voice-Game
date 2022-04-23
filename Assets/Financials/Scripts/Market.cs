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

}
