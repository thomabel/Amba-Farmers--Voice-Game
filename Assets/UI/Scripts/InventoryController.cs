using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;





public class InventoryController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement ScrollViewSection;

    [SerializeField]
    private InventoryList inventory;

    [SerializeField]
    private Inventory player;

    [SerializeField]
    GameObject playerObject;
    private Button currentPressedItem;
    //private Button FocusedButton;
    // Start is called before the first frame update

    void OnEnable ()
    {
        int length = inventory.length();
        currentPressedItem = null;
        VisualElement Current = new VisualElement();
        Current.AddToClassList("Row");

        root = GetComponent<UIDocument>().rootVisualElement;
        root.Focus();


        ScrollViewSection = root.Q<VisualElement>("InventoryScrollView");
        int count = 0;
        for(int i = 0; i < length; ++i)
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

            InventoryItem.style.backgroundImage = inventory.FindCardIndex(i).picture;

            Current.Add(InventoryItem);
            ScrollViewSection.Add(Current);

            count += 1;
        }

        int extra = 0;
        if (length % 5 != 0)
            extra = 5 - (length % 5);

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
        Debug.Log(obj);
        root.Q<VisualElement>("ItemInfo").style.display = DisplayStyle.Flex;
        //FocusedButton = (Button)obj.target;
        var button = (Button)obj.target;

        //Debug.Log(player.Add(inventory[int.Parse(button.name)]));

        //Instantiate(inventory[int.Parse(button.name)].gameobject, player.transform);
        //player.Equip(Inventory[int.Parse(button.name)].gameobject);

        if (currentPressedItem == null)
            currentPressedItem = button;
        else
        {
            currentPressedItem.style.opacity = (StyleFloat).5;
            currentPressedItem = button;
        }
        currentPressedItem.style.opacity = 1;

        Label QuantityNum = root.Q<Label>("QuantityNum");
        Label ItemName = root.Q<Label>("ItemName");

        Card CurrentCard = inventory.FindCardIndex(int.Parse(button.name));

        Debug.Log(CurrentCard.gameobject);

        Debug.Log(player.transform.position);
        //Instantiate(CurrentCard.gameobject);
        Instantiate(CurrentCard.gameobject, playerObject.transform.position + new Vector3(0, 2, 2), Quaternion.identity);
        ItemName.text = CurrentCard.name;
        QuantityNum.text = CurrentCard.InventoryQuantity.ToString();
        player.Add(CurrentCard.gameobject);
    }



}
