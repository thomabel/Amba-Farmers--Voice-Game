using UnityEngine;

public class Seed : MonoBehaviour, IStorable, IInteractable
{
    public GameObject plant;

    void IStorable.Use()
    {
        Instantiate(plant, transform.position, Quaternion.identity, null);
    }

    void IInteractable.Interact()
    {

    }
}
