using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneButtonController : MonoBehaviour
{
    private VisualElement root;
    public Button Phone;
    public Button PhoneButton;
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Phone = root.Q<Button>("Button");

        Phone.clicked += Pressed;

        PhoneButton = root.Q<Button>("phoneButton");
        PhoneButton.clicked += Pressed2;

    }
    void Pressed()
    {
        Phone.style.display = DisplayStyle.None;
    }
    void Pressed2()
    {
        Phone.style.display = DisplayStyle.Flex;
    }

}
