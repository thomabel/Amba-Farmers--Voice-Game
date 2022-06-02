using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Inventory inventory;
    public ColliderContainer container;

    public Vector3 tool_offset;
    private Item _etool;
    public Item etool
    {
        get
        {
            return _etool;
        }
        set
        {
            _etool = value;
            if (_etool != null) hudbuttons.AddToolImage(_etool.obj);
            else hudbuttons.AddToolImage(null);
        }
    }
    public IEquippable Tool;

    private Item _eitem;
    public Item eitem
    {
        get
        {
            return _eitem;
        }
        set
        {
            _eitem = value;
            if (_eitem != null) hudbuttons.AddItemImage(_eitem.obj);
            else hudbuttons.AddItemImage(null);
        }
    }
    public IStorable Item;

    public HUDButtons hudbuttons;

    private void Start()
    {
        etool = null;
        Tool = null;
        eitem = null;
        Item = null;
    }

    /// <summary>
    /// General method for picking up inventory items.
    /// </summary>
    /// <param name="thing"></param>
    public void Pickup(GameObject thing)
    {
        if (thing == null)
        {
            return;
        }

        if (thing.GetComponent<IStorable>() != null)
        {
            pickup_item(thing);
        }
        else
        {
            EquipTool(thing);
        }
        return;
    }

    /// <summary>
    /// Equips the item at index into the item equip slot.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Success of equip</returns>
    public bool EquipItem(int index)
    {
        // Check to see if that item exists.
        var check = inventory.Retrieve(index);
        if (check == null)
        {
            return false;
        }
        // Remove from inventory
        inventory.Remove(index);

        // Place current equip in inventory.
        if (eitem != null)
        {
            inventory.Add(eitem);
        }

        // Equip item.
        eitem = check;
        Item = eitem.obj.GetComponent<IStorable>();
        eitem.obj.transform.parent = transform;
        eitem.obj.transform.localPosition = Vector3.zero;
        eitem.obj.SetActive(false);
        return true;
    }

    /// <summary>
    /// Drops the equipped item on to the ground.
    /// </summary>
    /// <returns>Success of drop.</returns>
    public bool DropItem()
    {
        if (eitem == null)
        {
            return false;
        }

        eitem.obj.transform.localPosition = Vector3.up;
        eitem.obj.transform.parent = null;
        eitem.obj.SetActive(true);

        return true;
    }

    /// <summary>
    /// Assign the given equipment into the slot. Must have IEquippable.
    /// </summary>
    /// <param name="tool"></param>
    /// <returns>Success of equip.</returns>
    public bool EquipTool(GameObject tool)
    {
        if (tool == null)
        {
            return false;
        }

        var equip = tool.GetComponent<IEquippable>();
        if (equip == null)
        {
            return false;
        }

        if (etool != null)
        {
            DropTool();
        }

        etool = new Item(tool);
        Tool = equip;
        position_tool(transform, true, tool_offset);

        return true;
    }
   
    /// <summary>
    /// Drops the equipment on the ground.
    /// </summary>
    /// <returns>Success of drop.</returns>
    public bool DropTool()
    {
        if (etool == null)
        {
            return false;
        }

        position_tool(null, false, transform.position + tool_offset);
        etool = null;
        Tool = null;

        return true;
    }


    private void pickup_item(GameObject item)
    {
        int i = inventory.Add(item);
        item.transform.parent = transform;
        var col = item.GetComponents<Collider>();
        container.Remove(col);
        Debug.Log("add to inv " + i + " " + item.name);

        if (eitem == null && i >= 0)
        {
            EquipItem(i);
        }
    }

    // Sets up the positioning of the tool.
    private void position_tool(Transform parent, bool kinematic, Vector3 position)
    {
        // Controls if item is moving with physics.
        etool.obj.GetComponent<Rigidbody>().isKinematic = kinematic;

        // Disables collision.
        var col = etool.obj.GetComponents<Collider>();
        foreach (Collider c in col)
        {
            c.enabled = !kinematic;
            if (kinematic)
            {
                container.Remove(c);
            }
            else
            {
                container.Add(c);
            }
        }


        // Changes position of tool.
        var tool = etool.obj.transform;
        tool.parent = parent;
        if (parent != null)
        {
            tool.localPosition = position;
            tool.rotation = parent.rotation;
        }
    }
}
