using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryTransfer", menuName = "InventoryTransfer")]
public class InventoryTransfer : ScriptableObject
{
    [SerializeField]
    private Inventory playerInv;
    [SerializeField]
    private Inventory buildingInv;
}
