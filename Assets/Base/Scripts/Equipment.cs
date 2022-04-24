using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Inventory inventory;
    public Vector3 tool_offset;

    public GameObject item;
    public IStorable Item;
    public GameObject tool;
    public IEquippable Tool;

    /// <summary>
    /// General method for picking up inventory items.
    /// </summary>
    /// <param name="item"></param>
    public void Pickup(GameObject item)
    {
        if (item != null)
        {
            var store = item.GetComponent<IStorable>();
            if (store != null)
            {
                int i = inventory.Add(item);
                item.SetActive(false);
                if (item == null)
                {
                    EquipItem(i);
                }

            }
            else
            {
                EquipTool(item);
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
            item = inventory.Retrieve(index);
            Item = item.GetComponent<IStorable>();
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
                if (item != tool)
                {
                    DropTool();
                }
                tool = item;
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
        if (tool != null)
        {
            equip_position(null, false, transform.position + tool_offset);
            tool = null;
            Tool = null;
            return true;
        }
        return false;
    }

    // Sets up the positioning of the tool.
    private void equip_position(Transform parent, bool kinematic, Vector3 position)
    {
        // Disables collision.
        var col = tool.GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            c.isTrigger = kinematic;
        }

        // Controls if item is moving with physics.
        var rig = tool.GetComponent<Rigidbody>();
        rig.isKinematic = kinematic;

        // Changes position of tool.
        var trn = tool.transform;
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
