using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(
    menuName = "SO Variables/Inventory",
    fileName = "inventory"
    )]
public class Inventory : ScriptableObject, IEnumerable
{
    public int size;
    private Item[] items;

    // PRIVATE
    private void OnEnable()
    {
        if (items == null)
        {
            items = new Item[size];
        }
    }
    private bool check_index(int index)
    {
        return index >= 0 && index < items.Length;
    }
    private bool check_duplicates(GameObject item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].obj == item)
            {
                return true;
            }
        }
        return false;
    }


    // PUBLIC
    public int Size { get { return items.Length; } }
    public Item this[int i] { get { return items[i] == null ? null : items[i]; } }
    IEnumerator IEnumerable.GetEnumerator() { return items.GetEnumerator(); }
    /// <summary>
    /// Returns the amount of free spaces in the inventory.
    /// </summary>
    public int FreeSpace
    {
        get {
            int free = 0;
            foreach (Item i in items)
            {
                if (i == null || i.obj == null)
                {
                    free++;
                }
            }
            return free;
        }
    }
   
    /// <summary>
    /// Creates an Item from a GameObject and adds to the array.
    /// </summary>
    /// <returns>>0 as an index, -1 if not valid.</returns>
    public int Add(GameObject item)
    {
        return Add(new Item(item));
    }
    /// <summary>
    /// Adds an Item to the first available free spot.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>>0 as an index, -1 if not valid.</returns>
    public int Add(Item item)
    {
        if (FreeSpace > 0 && item != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    item.obj.SetActive(false);
                    return i;
                }
            }
        }
        return -1;
    }
    /// <summary>
    /// Adds to array at specified location.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Insert(int index, Item item)
    {
        if (check_index(index) && items[index] == null)
        {
            items[index] = item;
            if (item != null) item.obj.SetActive(false);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Removes the Item at the indexed location.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item Remove(int index)
    {
        if (check_index(index))
        {
            var item = items[index];
            items[index] = null;
            if(item != null) item.obj.SetActive(true);
            return item;
        }
        return null;
    }
    /// <summary>
    /// Returns the Item object from specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item Retrieve(int index)
    {
        if (check_index(index))
        {
            return items[index];
        }
        return null;
    }
    /// <summary>
    /// Checks for duplicate items when adding.
    /// </summary>
    /// <param name="ItemType"></param>
    /// <param name="Quantity"></param>
    /// <returns></returns>
    public bool DuplicateItems(Base.GoodType ItemType, float Quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            var type = items[i].obj.GetComponent<TypeLabel>().Type;

            if (items[i] != null && type == ItemType)
            {
                items[i].quantity += Quantity;
                items[i].obj.GetComponent<Quantity>().Value += Quantity;
                return true;
            }
        }
        return false;

    }

    // Build editor to show array.
    //private void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    ClipArray.isExpanded = EditorGUILayout.Foldout(ClipArray.isExpanded, ClipArray.name);
    //    if (ClipArray.isExpanded)
    //    {
    //        EditorGUI.indentLevel++;

    //        // The field for item count
    //        ClipArray.arraySize = EditorGUILayout.IntField("size", ClipArray.arraySize);

    //        // draw item fields
    //        for (var i = 0; i < ClipArray.arraySize; i++)
    //        {
    //            var item = ClipArray.GetArrayElementAtIndex(i);
    //            EditorGUILayout.PropertyField(item, new GUIContent($"Element {i}");
    //        }

    //        EditorGUI.indentLevel--;
    //    }
    //    serializedObject.ApplyModifiedProperties();
    //}
}
