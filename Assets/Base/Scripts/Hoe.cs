using UnityEngine;

public class Hoe : MonoBehaviour, IInteractable, IEquippable
{
    
    void IInteractable.Interact()
    {
        Debug.Log("Interact with a hoe!");
    }
    void IEquippable.Use()
    {
        
        Debug.Log("Used a hoe!");
    }
}
