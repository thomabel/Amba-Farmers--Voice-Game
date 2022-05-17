using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NearByObject : MonoBehaviour
{
    VisualElement root;
    public StringVariable Name;

    [SerializeField]
    private Interact FindClosest;

    private GameObject closestObject;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        closestObject = null;


    }
    private void FixedUpdate()
    {
        //Debug.Log(FindClosest.get_closest());
        closestObject = FindClosest.get_closest();
        if (closestObject != null && closestObject.tag.Equals("Plant"))
        {
            Debug.Log(closestObject);
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.Flex;
            root.Q<Label>("ObjectName").text = closestObject.name;
            
        }

        else
        {
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.None;
        }

    }

}
