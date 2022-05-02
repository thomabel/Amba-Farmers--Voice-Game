using UnityEngine;

public class InventoryHolder : MonoBehaviour, IInteractable
{
    public Inventory inventory;
    public GameEvent swap;
    public InventorySwap swap_storage;

    void IInteractable.Interact()
    {
        Debug.Log("Swap");
        swap_storage.second = inventory;
        swap.raise();
    }
}
