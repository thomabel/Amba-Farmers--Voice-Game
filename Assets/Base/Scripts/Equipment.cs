using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Inventory inventory;
    public Vector3 tool_offset;

    public Item item_obj;
    public IStorable Item;
    public int item_index;

    public Item tool_obj;
    public IEquippable Tool;

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

        var store = thing.GetComponent<IStorable>();
        if (store != null)
        {
            int i = inventory.Add(thing);
            if (item_obj == null)
            {
                EquipItem(i);
            }

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
        item_obj = inventory.Retrieve(index);
        if (item_obj == null)
        {
            return false;
        }

        Item = item_obj.obj.GetComponent<IStorable>();
        item_obj.obj.transform.parent = transform;
        item_index = index;
        return true;
    }

    /// <summary>
    /// Drops the equipped item on to the ground.
    /// </summary>
    /// <returns>Success of drop.</returns>
    public bool DropItem()
    {
        if (item_obj == null)
        {
            return false;
        }

        var item = inventory.Remove(item_index);
        item.obj.transform.parent = null;
        item.obj.SetActive(true);

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

        if (tool_obj != null && tool != tool_obj.obj)
        {
            DropTool();
        }

        tool_obj = inventory.create_item(tool);
        Tool = equip;
        equip_position(transform, true, tool_offset);

        return true;
    }
   
    /// <summary>
    /// Drops the equipment on the ground.
    /// </summary>
    /// <returns>Success of drop.</returns>
    public bool DropTool()
    {
        if (tool_obj != null)
        {
            return false;
        }
        
        equip_position(null, false, transform.position + tool_offset);
        tool_obj = null;
        Tool = null;

        return true;
    }

    // Sets up the positioning of the tool.
    private void equip_position(Transform parent, bool kinematic, Vector3 position)
    {
        // Disables collision.
        var col = tool_obj.obj.GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            c.isTrigger = kinematic;
        }

        // Controls if item is moving with physics.
        var rig = tool_obj.obj.GetComponent<Rigidbody>();
        rig.isKinematic = kinematic;

        // Changes position of tool.
        var trn = tool_obj.obj.transform;
        if (parent != null)
        {
            trn.parent = parent;
            trn.localPosition = position;
            trn.rotation = parent.rotation;
        }
        else
        {
            trn.parent = parent;
        }

    }
}
