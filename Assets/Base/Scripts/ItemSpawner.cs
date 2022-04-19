using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] List<Inventory> options;

    public bool Spawn(GameObject item)
    {
        var store = item.GetComponent<IStorable>();

        if (store != null)
        {
            return check_storage(item);
        }

        return false;
    }

    private bool check_storage(GameObject item)
    {
        foreach (Inventory inven in options)
        {
            if (inven.Add(item) >= 0)
            {
                return true;
            }
        }
        return false;
    }
}
