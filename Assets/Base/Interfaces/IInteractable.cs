/// <summary>
/// For objects that the player can interact with.
/// Does not necessarily have to be able to be picked up.
/// </summary>
public interface IInteractable
{
    public void Interact(UnityEngine.GameObject with);
}
