using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Inventory inventory;
    public ColliderContainer container;

    public Vector3 tool_offset;
    public Item etool;
    public IEquippable Tool;

    public int item_index;
    public Item eitem;
    public IStorable Item;

    private bool hasItem;
    private bool hasBucket;
    public BucketPickUp bucket;
    private void Awake()
    {
        hasItem = false;
    }
    /// <summary>
    /// General method for picking up inventory items.
    /// </summary>
    /// <param name="thing"></param>
    public void Pickup(GameObject thing)
    {

        if (hasItem)
        {
            if (hasBucket)
            {
                Debug.Log("Dropping");
                bucket.bucketDrop(thing);
                hasBucket = false;
                hasItem = false;
            }
            else
            {
                Debug.Log("Aleady Holding A Tool");
            }

            return;
        }

        if (thing == null)
        {
            return;
        }

        if (thing.GetComponent<IStorable>() != null)
        {
            pickup_item(thing);
        }
        else if(thing.gameObject.tag == "Bucket")
        {
            bucket.bucketPickup(thing);
            hasBucket = true;
            hasItem = true;
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
        eitem = inventory.Retrieve(index);
        if (eitem == null)
        {
            return false;
        }

        Item = eitem.obj.GetComponent<IStorable>();
        eitem.obj.transform.parent = transform;
        item_index = index;
        
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

        if (etool != null && tool != etool.obj)
        {
            DropTool();
        }

        etool = new Item(tool);
        Tool = equip;
        position_tool(transform, true, tool_offset);
        hasItem = true;
        Debug.Log("Picked");
        return true;
    }
   
    /// <summary>
    /// Drops the equipment on the ground.
    /// </summary>
    /// <returns>Success of drop.</returns>
    public bool DropTool()
    {
        if (etool != null)
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


    public bool ifHasItem()
    {
        return hasItem;

    }
    public bool ifHasBucket()
    {
        return hasBucket;

    }
}
