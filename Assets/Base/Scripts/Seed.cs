using UnityEngine;

public class Seed : MonoBehaviour, IStorable
{
    public GameObject plant;

    void IStorable.Use()
    {
        Instantiate(plant, transform.position, Quaternion.identity, null);
    }
}
