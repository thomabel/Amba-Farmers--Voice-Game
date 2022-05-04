using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item", order = 0)]
public class GenericItemSO : ScriptableObject, IStorable
{
    public string _itemName;
    public Sprite _itemSprite;
    
    void IStorable.Use()
    {

    }
}
