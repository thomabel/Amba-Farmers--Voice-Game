using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "market",
    menuName = "Financials/Market"
    )]
public class Market : ScriptableObject
{
    public List<MarketWrapper> Items;
}
