using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement ScrollViewSection;
    [SerializeField]
    private Card [] Inventory;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement Current = new VisualElement();
        Current.AddToClassList("Row");

        root = GetComponent<UIDocument>().rootVisualElement;
        ScrollViewSection = root.Q<VisualElement>("InventoryScrollView");
        int count = 0;
        for(int i = 0; i < Inventory.Length; ++i)
        {

            if(i != 0 && count % 5 == 0)
            {
                Current = new VisualElement();
                Current.AddToClassList("Row");
            }

            Button InventoryItem = new Button();
            InventoryItem.clickable.clickedWithEventInfo += ItemClicked;
            InventoryItem.name = i.ToString();
            InventoryItem.AddToClassList("SlotIcon");
            InventoryItem.AddToClassList("ItemButton");

            InventoryItem.style.backgroundImage = Inventory[i].picture;

            Current.Add(InventoryItem);
            ScrollViewSection.Add(Current);

            count += 1;
        }

        int extra = 0;
        if (Inventory.Length % 5 != 0)
            extra = 5 - (Inventory.Length % 5);

        for (int i = 0; i < extra; ++i)
        {

            Button InventoryItem = new Button();

            InventoryItem.AddToClassList("SlotIcon");
            InventoryItem.AddToClassList("ItemButton");

            InventoryItem.style.visibility = Visibility.Hidden;
            Current.Add(InventoryItem);
        }

    }
    void ItemClicked(EventBase obj)
    {
        var button = (Button)obj.target;
        Label QuantityNum = root.Q<Label>("QuantityNum");
        Label ItemName = root.Q<Label>("ItemName");
        ItemName.text = Inventory[int.Parse(button.name)].name;
        QuantityNum.text = Inventory[int.Parse(button.name)].quantity.ToString();
    }

}
