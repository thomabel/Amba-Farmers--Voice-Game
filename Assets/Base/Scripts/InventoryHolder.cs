using UnityEngine;

/// <summary>
/// Class that is strictly for holding an inventory on a game object.
/// </summary>
public class InventoryHolder : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    public GameEvent swap;
    public InventorySwap swap_storage;

    void IInteractable.Interact(GameObject with)
    {
        Debug.Log("Swap");
        swap_storage.second = inventory;
        swap.raise();
    }
}
