using UnityEngine;
using Currency = System.Int32;

[CreateAssetMenu(
    fileName = "marketwrap", 
    menuName = "Financials/MarketWrapper"
    )]
public class MarketWrapper : ScriptableObject
{
    public string display_name;
    public GameObject item_prefab;
    public Currency value;
    public int quantity = 1;
    public Texture2D picture;
    public float supply, demand;

    public Currency GetPrice()
    {
        return 1;
    }
}
