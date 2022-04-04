using UnityEngine;
using Currency = System.Int32;

[CreateAssetMenu(
    fileName = "marketwrap", 
    menuName = "Financials/MarketWrapper"
    )]
public class MarketWrapper : ScriptableObject
{
    public GameObject Item;
    public Currency value;

}
