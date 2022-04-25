using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Inventory inventory;
    public Vector3 tool_offset;

    public GameObject item_obj;
    public IStorable Item;
    public GameObject tool_obj;
    public IEquippable Tool;

    /// <summary>
    /// General method for picking up inventory items.
    /// </summary>
    /// <param name="thing"></param>
    public void Pickup(GameObject thing)
    {
        if (thing != null)
        {
            var store = thing.GetComponent<IStorable>();
            if (store != null)
            {
                int i = inventory.Add(thing);
                thing.SetActive(false);
                if (item_obj == null)
                {
                    EquipItem(i);
                }

            }
            else
            {
                EquipTool(thing);
            }
        }
    }

    /// <summary>
    /// Equips the item at index into the item equip slot.
    /// </summary>
    /// <param name="index"></param>
    public void EquipItem(int index)
    {
        if (inventory.check_index(index))
        {
            item_obj = inventory.Retrieve(index);
            Item = item_obj.GetComponent<IStorable>();
            item_obj.transform.parent = transform;
        }
    }


    /// <summary>
    /// Assign the given equipment into the slot. Must have IEquippable.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Success of equip.</returns>
    public bool EquipTool(GameObject item)
    {
        if (item != null)
        {
            var equip = item.GetComponent<IEquippable>();
            if (equip != null)
            {
                if (item != tool_obj)
                {
                    DropTool();
                }
                tool_obj = item;
                Tool = equip;
                equip_position(transform, true, tool_offset);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Drops the equipment on the ground.
    /// </summary>
    /// <returns>Success of drop.</returns>
    public bool DropTool()
    {
        if (tool_obj != null)
        {
            equip_position(null, false, transform.position + tool_offset);
            tool_obj = null;
            Tool = null;
            return true;
        }
        return false;
    }

    // Sets up the positioning of the tool.
    private void equip_position(Transform parent, bool kinematic, Vector3 position)
    {
        // Disables collision.
        var col = tool_obj.GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            c.isTrigger = kinematic;
        }

        // Controls if item is moving with physics.
        var rig = tool_obj.GetComponent<Rigidbody>();
        rig.isKinematic = kinematic;

        // Changes position of tool.
        var trn = tool_obj.transform;
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
