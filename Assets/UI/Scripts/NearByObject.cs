using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NearByObject : MonoBehaviour
{
    VisualElement root;
    public StringVariable Name;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
       
    }
    private void Update()
    {
        if (Name.Value != null)
        {
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.Flex;
            root.Q<Label>("ObjectName").text = Name.Value;
            
        }

        else
        {
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.None;
        }

    }

}
