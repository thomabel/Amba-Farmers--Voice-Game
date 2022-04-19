using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int size;
    GameObject[] items;

    public int Size
    {
        get { return size; }
    }

    private void Start()
    {
        if (items == null)
        {
            items = new GameObject[size];
        }
    }



    /// <summary>
    /// Opens the UI inventory menu.
    /// </summary>
    public void Open()
    {

    }

    /// <summary>
    /// Adds item to the inventory in the first free spot.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Success of the add.</returns>
    public int Add(GameObject item)
    {
        for (int i = 0; i < size; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Insert the item into storage at the specified location.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns>Success of insertion.</returns>
    public bool Insert(int index, GameObject item)
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
    public GameObject Remove(int index)
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
    public GameObject Retrieve(int index)
    {
        if (check_index(index))
        {
            return items[index];
        }
        return null;
    }
    
    // Make sure the index is correct.
    public bool check_index(int index)
    {
        return index >= 0 && index < size;
    }
}
