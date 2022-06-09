using UnityEngine;
using Currency = System.Int32;

/// <summary>
/// Stores information about a good type.
/// Used as a reference throughout game, but mostly for market transactions.
/// Useful for UI as well.
/// </summary>
[CreateAssetMenu(
    fileName = "marketwrap", 
    menuName = "Financials/MarketWrapper"
    )]
public class MarketWrapper : ScriptableObject
{
    public Base.GoodType type; // Stores all type in a list to label objects.
    public string display_name; // For UI to display the name.
    public GameObject item_prefab; // The object to spawn in.
    public Currency value; // The base monetary value of the object.
    public Base.QuantityType qty_type; // How the quantity is interpreted.
    public Texture2D picture; // A picture/icon for the UI to display.
    public float supply, demand; // For a future feature to change prices dynamically.

    // Used for changing prices dynamically.
    public Currency PriceOf()
    {
        return value;
    }
}
