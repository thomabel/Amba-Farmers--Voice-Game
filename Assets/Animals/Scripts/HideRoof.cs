using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoof : MonoBehaviour
{
    [SerializeField] public GameObject roof;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("I'm in!");
            roof.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("I'm out!");
            roof.SetActive(true);
        }
    }
}
