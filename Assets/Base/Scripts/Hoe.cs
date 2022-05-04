using UnityEngine;

public class Hoe : MonoBehaviour, IEquippable, IInteractable
{
    void IEquippable.Use(GameObject with)
    {
        var land = with.GetComponent<TerrainData>();
        if (land != null)
        {
            land.SetTilled(transform.position, true);
        }
    }

    void IInteractable.Interact(GameObject with)
    {

    }
}
