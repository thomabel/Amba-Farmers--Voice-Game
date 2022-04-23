using UnityEngine;

public class GrowBox : MonoBehaviour, IInteractable
{
    public FloatReference scale_factor;


    void IInteractable.Interact()
    {
        var manip = transform.localScale;
        manip *= scale_factor.Value;
        transform.localScale = manip;
    }
}
