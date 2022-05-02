using UnityEngine;

[RequireComponent(
    typeof(Quantity), 
    typeof(TypeLabel)
    )]
public class Fruit : MonoBehaviour, IInteractable, IEquippable
{
    public Quantity qty;
    public TypeLabel tl;

    private void Start()
    {
        qty = GetComponent<Quantity>();
        tl = GetComponent<TypeLabel>();
    }
    void IInteractable.Interact()
    {
        Debug.Log("Interact with " + name);
    }
    void IEquippable.Use()
    {
        Debug.Log("Equipped " + name);
    }
}
