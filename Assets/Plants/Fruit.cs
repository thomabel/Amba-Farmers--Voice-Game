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
    void IInteractable.Interact(GameObject with)
    {
        Debug.Log("Interact with " + name);
    }
    void IEquippable.Use(GameObject with)
    {
        Debug.Log("Equipped " + name);
    }
}
