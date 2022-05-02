using UnityEngine;
using Currency = System.Int32;

[CreateAssetMenu(
    fileName = "marketwrap", 
    menuName = "Financials/MarketWrapper"
    )]
public class MarketWrapper : ScriptableObject
{
    public Base.GoodType type;
    public string display_name;
    public GameObject item_prefab;
    public Currency value;
    public Base.QuantityType qty_type;
    public Texture2D picture;
    public float supply, demand;

    public Currency PriceOf()
    {
        return value;
    }
}
