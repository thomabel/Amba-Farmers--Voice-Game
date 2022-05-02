using UnityEngine;

[CreateAssetMenu(
    menuName = "SO Variables/Inventory Swap",
    fileName = "inventory swap"
    )]
public class InventorySwap : ScriptableObject
{
    public Inventory first;
    public Inventory second;
}
