using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int size;
    private IStorable[] items;
    private IEquippable equip;

    public int Size
    {
        get { return size; }
    }

    private void Start()
    {
        if (items == null)
        {
            items = new IStorable[size];
        }
    }

    public bool Equip(IEquippable equipment)
    {
        if (equipment != null)
        {
            equip = equipment;
        }
        return false;
    }
    public IEquippable Drop()
    {
        var equ = equip;
        equip = null;
        return equ;
    }

    public bool Insert(int index, IStorable item)
    {
        if (check_index(index) && items[index] == null)
        {
            items[index] = item;
            return true;
        }
        return false;
    }
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
    public IStorable Retrieve(int index)
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
}
