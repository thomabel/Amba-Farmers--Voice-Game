using UnityEngine;
using System;

public class Item
{
    public GameObject obj;
    public float quantity;

    public Item(GameObject object_)
    {
        obj = object_;
        var qty = object_.GetComponent<Quantity>();
        quantity = qty == null ? 1 : qty.Value;
    }
    public Item()
    {
        obj = null;
        quantity = 0;
    }
}
