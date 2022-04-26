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

    public Item this[int i]
    {
        get { return items[i]; }
        set { items[i] = value; }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }
    void IInteractable.Interact()
    {
        return;
    }

    public int FreeSpace()
    {
        int free = 0;
        for (int i = 0; i < size; i++)
        {
            if (items[i] == null)
                free++;
        }
        return free;
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
    
    private bool check_index(int index)
    {
        return index >= 0 && index < size;
    }

    public bool DuplicateItems(Item item)
    {
        for (int i = 0; i < size; i++)
        {
            if (items[i] != null && items[i].obj.GetComponent<TypeLabel>().Type == item.obj.GetComponent<TypeLabel>().Type)
            {
                items[i].quantity += item.quantity;
                return true;
            }
        }
        return false;

    }
}
