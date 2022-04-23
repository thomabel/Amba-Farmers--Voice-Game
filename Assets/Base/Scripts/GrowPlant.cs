using UnityEngine;

public class GrowPlant : MonoBehaviour, IInteractable
{
    public FloatReference scale_factor;

    void IInteractable.Interact()
    {
        
        var manip = transform.localScale;
        manip.y += scale_factor.Value;
        transform.localScale = manip;
        
    }
}

