using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] Vector3 tool_offset;

    [SerializeField] IStorable[] items;
    [SerializeField] IStorable item;

    [SerializeField] IEquippable tool;
    [SerializeField] GameObject tool_object;

    public int Size
    {
        get { return size; }
    }
    public IStorable Item
    {
        get { return item; }
    }
    public IEquippable Tool
    {
        get { return tool; }
    }

    private void Start()
    {
        if (items == null)
        {
            items = new IStorable[size];
        }
    }



    /// <summary>
    /// Opens the UI inventory menu.
    /// </summary>
    public void Open()
    {

    }

    // IEquippable
    /// <summary>
    /// Assign the given equipment into the slot. Must have IEquippable.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Success of equip.</returns>
    public bool Equip(GameObject item)
    {
        if (item != null)
        {
            var equip = item.GetComponent<IEquippable>();
            if (equip != null)
            {
                tool = equip;
                tool_object = item;
                equip_position(transform, false, tool_offset);
                return true;
            }
        }
        return false;
    }
   /// <summary>
   /// Drops the equipment on the ground.
   /// </summary>
   /// <returns>Success of drop.</returns>
    public bool Drop()
    {
        if (tool_object != null)
        {
            equip_position(null, true, transform.position + tool_offset);
            tool_object = null;
            tool = null;
            return true;
        }
        return false;
    }
    
    // Sets up the positioning of the tool.
    private void equip_position(Transform parent, bool kinematic, Vector3 position)
    {
        tool_object.transform.parent = parent;
        tool_object.GetComponent<Rigidbody>().isKinematic = kinematic;
        tool_object.transform.localPosition = position;
    }

    // IStorable Methods
    /// <summary>
    /// Insert the item into storage at the specified location.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns>Success of insertion.</returns>
    public bool Insert(int index, IStorable item)
    {
        if (check_index(index) && items[index] == null)
        {
            items[index] = item;
            return true;
        }
        return false;
    }
    /// <summary>
    /// Remove the item from the specified location.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>The removed item.</returns>
    public IStorable Remove(int index)
    {
        if (check_index(index))
        {
            var item = items[index];
            items[index] = null;
            return item;
        }
        return null;
    }
    /// <summary>
    /// Returns the item in the given slot.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>The stored item.</returns>
    public IStorable Retrieve(int index)
    {
        if (check_index(index))
        {
            return items[index];
        }
        return null;
    }
    
    // Make sure the index is correct.
    private bool check_index(int index)
    {
        return index >= 0 && index < size;
    }
}
