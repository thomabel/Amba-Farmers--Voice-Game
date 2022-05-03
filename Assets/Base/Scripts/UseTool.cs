using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTool : MonoBehaviour
{
    public Equipment equipment;
    public FullWater fullWater;
    // Start is called before the first frame update
    public void use()
    {
        if (equipment.ifHasItem())
        {
            if (equipment.ifHasBucket())
            {
                fullWater.Full();
            }
            Debug.Log("Using Tools");
        }
        else
        {
            Debug.Log("No Tool Holding");
        }
    }
}
