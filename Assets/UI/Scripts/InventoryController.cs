using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;





public class InventoryController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement ScrollViewSection;


    [SerializeField]
    private Inventory player;

    private Button currentPressedItem;
    [SerializeField]
    private Market market;


    private Button EquippedItem;

    [SerializeField]
    private Equipment PlayerEquipment;

    private VisualElement ItemInfo;

    private Button addInvButton;

    //private Button FocusedButton;
    // Start is called before the first frame update

    void OnEnable ()
    {
        //int length = inventory.length();
        currentPressedItem = null;
        VisualElement Current = new VisualElement();
        Current.AddToClassList("Row");

        root = GetComponent<UIDocument>().rootVisualElement;
        root.Focus();

        ItemInfo = root.Q<VisualElement>("ItemInfo");
        root.Q<Button>("EquipButton").clickable.clickedWithEventInfo += EquipButtonClicked;
        
        EquippedItem = root.Q<Button>("EquippedItem");
        EquippedItem.clickable.clickedWithEventInfo += ItemClicked;

        findItemAndDisplay(PlayerEquipment.item_obj, ref EquippedItem);

        addInvButton = root.Q<Button>("AddInvButton");
        addInvButton.clicked += addInvClicked;
        /*
        if(value != null)
            EquippedItem.style.backgroundImage = new StyleBackground(value.picture);
        */


        ScrollViewSection = root.Q<VisualElement>("InventoryScrollView");
        int count = 0;
        int length = player.Size;
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
            Item tmpobj = player.Retrieve(i);
            if(tmpobj == null) InventoryItem.style.backgroundImage = null;
            else InventoryItem.style.backgroundImage = findReference(tmpobj.obj.GetComponent<TypeLabel>().Type).picture;
            //InventoryItem.style.backgroundImage = null;
            Current.Add(InventoryItem);
            ScrollViewSection.Add(Current);

            count += 1;

            //PlayerInventory.Insert(i, inventory.FindCardIndex(i).item_prefab);


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

    void addInvClicked()
    {
        int index = player.Add(PlayerEquipment.item_obj);
        if (index == -1)
        {
            root.Q<VisualElement>("SpaceErrorWarning").style.display = DisplayStyle.Flex;
            addInvButton.style.display = DisplayStyle.None;
            return;
        }
        EquippedItem.style.backgroundImage = null;
        PlayerEquipment.item_obj = null;
        EquippedItem.style.opacity = (StyleFloat).5;

        Button InvIndexToChange = root.Q<Button>(index.ToString());

        findItemAndDisplay(player.Retrieve(index), ref InvIndexToChange);
        ItemInfo.style.display = DisplayStyle.None;

    }
    MarketWrapper findReference(Base.GoodType tmp)
    {
        for (int i = 0; i < market.Reference.Count; ++i)
        {
            if (market.Reference[i].type == tmp)
            {
                return market.Reference[i];
            }
        }
        return null;
    }

    void findItemAndDisplay(Item FindObj, ref Button button)
    {
        if (FindObj == null)
        {
            button.style.backgroundImage = null;
            return;
        }

        MarketWrapper value;
        bool checkIfItemExists = market.Comparator.TryGetValue(FindObj.obj.GetComponent<TypeLabel>().Type, out value);
        if (checkIfItemExists) button.style.backgroundImage = new StyleBackground(value.picture);
        else button.style.backgroundImage = null;
    }
    /*
    void findItemAndDisplay(Item FindObj, ref VisualElement button)
    {
        if (FindObj == null)
        {
            button.style.backgroundImage = null;
            return;
        }

        MarketWrapper value;
        bool checkIfItemExists = market.Comparator.TryGetValue(FindObj.obj.GetComponent<TypeLabel>().Type, out value);
        if (checkIfItemExists) button.style.backgroundImage = new StyleBackground(value.picture);
        else button.style.backgroundImage = null;
    }

    */


    void EquipButtonClicked(EventBase obj)
    {
        var button = (Button)obj.target;
        int index = int.Parse(currentPressedItem.name);
        //PlayerEquipment.EquipTool(Instantiate(inventory.FindCardIndex(int.Parse(currentPressedItem.name)).item_prefab));

        Item previousItem = PlayerEquipment.item_obj;
        PlayerEquipment.inventory = player;
        PlayerEquipment.EquipItem(index);
        player.Remove(index);
        player.Insert(index, previousItem);


        
        findItemAndDisplay(player.Retrieve(index), ref currentPressedItem);
        //currentPressedItem.style.backgroundImage = null;

        findItemAndDisplay(PlayerEquipment.item_obj, ref EquippedItem);

        currentPressedItem.style.opacity = (StyleFloat).5;
        currentPressedItem = null;
        ItemInfo.style.display = DisplayStyle.None;

    }
    void ItemClicked(EventBase obj)
    {
        Debug.Log(obj);
        ItemInfo.style.display = DisplayStyle.Flex;
        //FocusedButton = (Button)obj.target;
        var button = (Button)obj.target;

        //Debug.Log(player.Add(inventory[int.Parse(button.name)]));

        //Instantiate(inventory[int.Parse(button.name)].gameobject, player.transform);
        //player.Equip(Inventory[int.Parse(button.name)].gameobject);

        if (currentPressedItem == null)
            currentPressedItem = button;
        else if (currentPressedItem.name.Equals(button.name))
        {
            currentPressedItem.style.opacity = (StyleFloat).5;
            ItemInfo.style.display = DisplayStyle.None;
            currentPressedItem = null;
            return;

        }
        else
        {
            currentPressedItem.style.opacity = (StyleFloat).5;
            currentPressedItem = button;
        }

        int index;
        bool isequippable = button.name.Equals("EquippedItem");

        Item CurrentCard;
        if (isequippable) CurrentCard = PlayerEquipment.item_obj;
        else
        {
            index = int.Parse(currentPressedItem.name);
            CurrentCard = player.Retrieve(index);           
        }

        ItemInfo = root.Q<VisualElement>("ItemInfo");
        if (CurrentCard != null)
        {
            ItemInfo.style.display = DisplayStyle.Flex;
            MarketWrapper value;
            bool checkIfItemExists = market.Comparator.TryGetValue(CurrentCard.obj.GetComponent<TypeLabel>().Type, out value);
            if (checkIfItemExists)
            {
                currentPressedItem.style.opacity = 1;
                Label QuantityNum = root.Q<Label>("QuantityNum");
                Label ItemName = root.Q<Label>("ItemName");
                ItemName.text = value.display_name;
                QuantityNum.text = CurrentCard.obj.GetComponent<Quantity>().Value.ToString();
                if (isequippable)
                {
                    root.Q<VisualElement>("ContainerButton").style.display = DisplayStyle.None;
                    root.Q<VisualElement>("AddInventoryContainer").style.display = DisplayStyle.Flex;
                    root.Q<VisualElement>("SpaceErrorWarning").style.display = DisplayStyle.None;
                    addInvButton.style.display = DisplayStyle.Flex;
                }
                else
                {
                    root.Q<VisualElement>("ContainerButton").style.display = DisplayStyle.Flex;
                    root.Q<VisualElement>("AddInventoryContainer").style.display = DisplayStyle.None;
                }
            }
        }
        else ItemInfo.style.display = DisplayStyle.None;

    }



}
