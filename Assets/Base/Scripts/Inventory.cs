using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour, IEnumerable, IInteractable
{
    [SerializeField] int size;
    Item[] items;

    public int Size
    {
        get { return size; }
    }

    private void Start()
    {
        if (items == null)
        {
            items = new Item[size];
        }
        
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }
    void IInteractable.Interact()
    {

    }

    public int Add(Item item)
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

    public bool Insert(int index, Item item)
    {
        if (check_index(index) && items[index] == null)
        {
            items[index] = item;
            return true;
        }
        return false;
    }

    public Item Remove(int index)
    {
        if (check_index(index))
        {
            var item = items[index];
            items[index] = null;
            return item;
        }
        return null;
    }

    public Item Retrieve(int index)
    {
        if (check_index(index))
        {
            return items[index];
        }
        return null;
    }
    
    public bool check_index(int index)
    {
        return index >= 0 && index < size;
    }
}
