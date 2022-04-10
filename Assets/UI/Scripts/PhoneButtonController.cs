using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneButtonController : MonoBehaviour
{
    private VisualElement root;
    public Button Phone;
    public Button PhoneButton;

    private Button ShopButton;

    public GameObject ShopApp;

    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Phone = root.Q<Button>("RealPhoneButtonContainer");

        Phone.clicked += Pressed;

        PhoneButton = root.Q<Button>("phoneButton");
        PhoneButton.clicked += Pressed2;

        ShopButton = root.Q<Button>("ShopApp");
        ShopButton.clicked += ShopButtonPressed;

    }

    void Pressed()
    {
        Phone.style.display = DisplayStyle.None;
    }
    void Pressed2()
    {
        Phone.style.display = DisplayStyle.Flex;
    }
    void ShopButtonPressed()
    {
        
        Debug.Log(ShopApp);
        ShopApp.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
