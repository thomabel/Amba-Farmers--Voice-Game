using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLight : MonoBehaviour
{
    public Transform lightSource;

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray plantToSun = new Ray(transform.position, lightSource.position - transform.position);
        RaycastHit obstruction;
        Debug.DrawRay(transform.position, lightSource.position - transform.position);
        if (Physics.Raycast(plantToSun, out obstruction))
        {
            Debug.Log("no light! " + obstruction.point);
        }
        else
        {
            Debug.Log("Light!!!!!");
        }
        
    }
}
