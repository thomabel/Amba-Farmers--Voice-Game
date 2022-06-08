using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompareInventory : MonoBehaviour
{
    //Player Inventory
    public Inventory InventoryOne;

    //House Inventory
    public Inventory InventoryTwo;

    private VisualElement root;

    //Player Inv Scroll view
    private ScrollView ScrollViewOne;

    //House Inv Scroll view
    private ScrollView ScrollViewTwo;

    private Button EquipButton;

    private Button ToolInfoContainerButton;

    private VisualElement ItemContainerButtons;
    private Button ItemPlayerInventory;
    private Button ItemHouseInventory;

    private VisualElement ToolInfoContainer;

    [SerializeField]
    private Market market;

    [SerializeField]
    private GameObject phone;

    [SerializeField]
    private Equipment PlayerEquipment;


    //Holds information of item/tool boxes that have
    //been clicked.
    public struct InvAndIndexInfo
    {
        public Inventory inventory;
        public int index;
        public int InvNum;
        public Base.GoodType type;
    };

    //First item clicked by user
    InvAndIndexInfo Item1;

    //Second item clicked by user
    //and will be swapped by first item pressed
    InvAndIndexInfo Item2;


    bool equipItemPressed;
    string currentEquip;


    Button EquipToolButton;
    Button EquipItemButton;

    void OnEnable()
    {
        Item1 = new InvAndIndexInfo();

        Item2 = new InvAndIndexInfo();

        //If Item.index = -1 then nothing has been Pressed
        Item1.index = -1;

        Item2.index = -1;
        equipItemPressed = false;
        currentEquip = "empty";

        assignUItoVariables();
        assignButtonsToFunctions();

        root.Q<Label>("Label1").text = InventoryOne.name;
        root.Q<Label>("Label2").text = InventoryTwo.name;

        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");

        EquipToolButton.name = "Tool";
        EquipItemButton.name = "Item";

        DisplayEquippable();

        ItemPlayerInventory.text = "Add\n <";
        ItemHouseInventory.text = "Add\n >";

        root.Focus();
        phone.SetActive(false);


    }


    //Assign UI builder variables to
    //script variables
    void assignUItoVariables()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ScrollViewOne = root.Q<ScrollView>("ScrollView1");
        ScrollViewTwo = root.Q<ScrollView>("ScrollView2");
        EquipToolButton = root.Q<Button>("ToolButton");
        EquipItemButton = root.Q<Button>("ItemButton");
        EquipButton = root.Q<Button>("Equip");
        ToolInfoContainer = root.Q<VisualElement>("ToolContainerButtons");
        ToolInfoContainerButton = root.Q<Button>("ToolButtonSend");
        ItemContainerButtons = root.Q<VisualElement>("ItemContainerButtons");
        ItemPlayerInventory = root.Q<Button>("ItemPlayerInventory");
        ItemHouseInventory = root.Q<Button>("ItemHouseInventory");

    }
    void assignButtonsToFunctions()
    {
        EquipToolButton.clickable.clickedWithEventInfo += equipPressed;
        EquipItemButton.clickable.clickedWithEventInfo += equipPressed;
        EquipButton.clicked += equip;
        ToolInfoContainerButton.clicked += SendtoInventoryTwo;
        ItemPlayerInventory.clickable.clickedWithEventInfo += AddtoInventory;
        ItemHouseInventory.clickable.clickedWithEventInfo += AddtoInventory;
        root.Q<Button>("BackButton").clicked += GoBack;
    }

    //Back button, Go back to phone UI display
    void GoBack()
    {
        phone.SetActive(true);
        this.gameObject.SetActive(false);
    }
    //Add item to either player or house inventory
    //depending on button pressed.
    void AddtoInventory(EventBase obj)
    {
        var button = (Button)obj.target;

        //Player wants to send to player inventory
        if (button.name.Equals("ItemPlayerInventory"))
        {
            Item itemObj = PlayerEquipment.eitem;
            if (itemObj != null)
            {
                //Check player inventory if it has room to add
                //If -1 no room exists so we cannot add
                if (InventoryOne.Add(PlayerEquipment.eitem) == -1) return;
                PlayerEquipment.eitem = null;
                PlayerEquipment.Item = null;

            }
        }
        //Player wants to send to House inventory
        else
        {
            Item itemobj = PlayerEquipment.eitem;
            if (itemobj != null)
            {
                //Check House inventory if it has room to add
                //If -1 no room exists so we cannot add
                if (InventoryTwo.Add(PlayerEquipment.eitem) == -1) return;
                PlayerEquipment.eitem = null;
                PlayerEquipment.Item = null;
            }

        }
        //Reset Equip Item button display
        EquipItemButton.style.opacity = (StyleFloat).6;
        EquipItemButton.style.backgroundImage = null;
        equipItemPressed = false;

        //Display new info
        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");
        DisplayEquippable();
        //If nothing is pressed remove quantity info
        if (Item1.index == -1)
            root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;

    }
    //Send Tools directly to House Inventory (Tools cannot be in player Inv)
    void SendtoInventoryTwo()
    {
        //If tool is equipped
        if (PlayerEquipment.etool != null)
        {
            if (InventoryTwo.Add(PlayerEquipment.etool) == -1) return;
            PlayerEquipment.etool = null;
            PlayerEquipment.Tool = null;
            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");
            DisplayEquippable();
        }
        //Reset Equip Tool button display
        EquipToolButton.style.opacity = (StyleFloat).6;
        EquipToolButton.style.backgroundImage = null;

        equipItemPressed = false;
        if (Item1.index == -1)
            root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
    }
    //Equip Item or tool button Pressed
    void equipPressed(EventBase obj)
    {
        var button = (Button)obj.target;
        string name = button.name;

        //Equip Item button pressed, Display info box
        if (equipItemPressed && currentEquip.Equals(name))
        {
            button.style.opacity = (StyleFloat).6;
            equipItemPressed = false;
            currentEquip = "empty";
            if (Item1.index != -1)
                InfoboxDisplay(Item1.inventory.Retrieve(Item1.index));
            else InfoboxDisplay(null);
        }
        //Equip Tool button pressed, Display info box
        else
        {
            currentEquip = name;
            button.style.opacity = 1;
            equipItemPressed = true;
            if (name.Equals("Tool"))
            {
                InfoboxDisplay(PlayerEquipment.etool, DisplayStyle.Flex, DisplayStyle.None, DisplayStyle.None);
                EquipItemButton.style.opacity = (StyleFloat).6;
            }
            else
            {
                InfoboxDisplay(PlayerEquipment.eitem, DisplayStyle.None, DisplayStyle.Flex, DisplayStyle.None);
                EquipToolButton.style.opacity = (StyleFloat).6;
            }
        }

    }
    //Display equipped Item or Tool buttons with new info
    void DisplayEquippable()
    {
        Debug.Log(PlayerEquipment.eitem);
        Debug.Log(PlayerEquipment.etool);
        if (PlayerEquipment.etool != null)
        {
            DisplayEquipItemOrTool(ref PlayerEquipment.etool.obj, ref EquipToolButton);
        }
        if (PlayerEquipment.eitem != null)
        {
            DisplayEquipItemOrTool(ref PlayerEquipment.eitem.obj, ref EquipItemButton);
        }

    }
    //Display equipped Item or Tool buttons with new info
    void DisplayEquipItemOrTool(ref GameObject equip, ref Button equipButton)
    {
        MarketWrapper value;
        if (market.Comparator.TryGetValue(equip.GetComponent<TypeLabel>().Type, out value))
        {
            equipButton.style.backgroundImage = new StyleBackground(value.picture);
        }
        else
            equipButton.style.backgroundImage = new StyleBackground();
    }


    //Display all inventory materials.
    void Display(Inventory inventory, ScrollView viewScroller, string InvNum)
    {
        viewScroller.Clear();

        VisualElement Current = new VisualElement();
        Current.AddToClassList("Row");

        int count = 0;
        //Display inventory
        for (int i = 0; i < inventory.Size; ++i)
        {
            if (i != 0 && count % 2 == 0)
            {
                Current = new VisualElement();
                Current.AddToClassList("Row");
            }

            Button InventoryItem = new Button();

            InventoryItem.name = InvNum + i.ToString();
            InventoryItem.AddToClassList("SlotIcon");
            InventoryItem.AddToClassList("ItemButton");
            InventoryItem.AddToClassList("SmallerItemButton");

            InventoryItem.clickable.clickedWithEventInfo += ItemClicked;

            if (inventory.Retrieve(i) != null)
            {
                MarketWrapper value = null;
                market.Comparator.TryGetValue(inventory.Retrieve(i).obj.GetComponent<TypeLabel>().Type, out value);
                InventoryItem.style.backgroundImage = value.picture;
                InventoryItem.text = inventory.Retrieve(i).quantity.ToString();

            }
            else InventoryItem.style.backgroundImage = null;

            Current.Add(InventoryItem);
            viewScroller.Add(Current);

            count += 1;

        }
        //Add extra buttons to ensure that buttons are the same size.
        //Since size fluctuates with # of buttons displayed
        int extra = 0;
        if (inventory.Size % 2 != 0)
            extra = 2 - (inventory.Size % 2);

        for (int i = 0; i < extra; ++i)
        {

            Button InventoryItem = new Button();

            InventoryItem.AddToClassList("SlotIcon");
            InventoryItem.AddToClassList("ItemButton");

            InventoryItem.style.visibility = Visibility.Hidden;
            Current.Add(InventoryItem);
        }

        if (Item1.index != -1)
        {
            InfoboxDisplay(Item1.inventory.Retrieve(Item1.index));
            Button PressedInventoryCard = root.Q<Button>(Item1.InvNum.ToString() + Item1.index.ToString());
            PressedInventoryCard.style.opacity = 1;
        }
    }
    //Convert type label to string
    string getTypeLabel(Base.GoodType typeOfItem)
    {
        if (typeOfItem > Base.GoodType.Tool_Start && typeOfItem < Base.GoodType.Tool_End)
        {
            return "Tool";
        }
        else if (typeOfItem > Base.GoodType.Seed_Start && typeOfItem < Base.GoodType.Seed_End)
        {
            return "Seed";
        }
        else if (typeOfItem > Base.GoodType.Animal_Start && typeOfItem < Base.GoodType.Animal_End)
        {
            return "Animal";
        }
        return "Fruit";

    }

    //Equip Item or tool to equipment slots
    void equip()
    {
        string typeOfItem = getTypeLabel(Item1.type);
        //If seed gets equipped to item slot
        if (typeOfItem == "Seed")
        {
            Item PreviousEquipItem = PlayerEquipment.eitem;
            PlayerEquipment.inventory = Item1.inventory;
            //Equip new item and replace item that had been equipped
            //with new item that needs to be equipped
            PlayerEquipment.EquipItem(Item1.index);
            Item1.inventory.Remove(Item1.index);
            Item1.inventory.Insert(Item1.index, PreviousEquipItem);
            Item1.index = -1;
            Item1.InvNum = 0;

            currentEquip = "Item";
            InfoboxDisplay(PlayerEquipment.eitem, DisplayStyle.None, DisplayStyle.Flex, DisplayStyle.None);

            EquipToolButton.style.opacity = (StyleFloat).6;
            EquipItemButton.style.opacity = 1;

        }
        //If Tool or Fruit gets equipped to Tool slot
        else if (typeOfItem == "Tool" || typeOfItem == "Fruit")
        {

            Item PreviousEquipTool = PlayerEquipment.etool;


            if (PreviousEquipTool == null|| (PreviousEquipTool.obj.GetComponent<TypeLabel>().Type != Item1.inventory.Retrieve(Item1.index).obj.GetComponent<TypeLabel>().Type))
            {
                Item PressedTool = Item1.inventory.Retrieve(Item1.index);
                //Make a new item onto the scene that holds 1 quanitity
                //Tool in scene MUST ONLY HAVE QUANTITY = 1
                Item toolEquip = new Item();
                MarketWrapper marketWrapper = market.Comparator[PressedTool.obj.GetComponent<TypeLabel>().Type];
                toolEquip.obj = Instantiate(marketWrapper.item_prefab);
                toolEquip.obj.AddComponent<TypeLabel>();
                TypeLabel tmpLabel = toolEquip.obj.GetComponent<TypeLabel>();
                tmpLabel.Type = marketWrapper.type;
                toolEquip.quantity = 1;
                //decrement quantity since we are removing a tool
                 --PressedTool.quantity;

                //If no more tools then we don't need tool that we allocated above
                //remove existent tool from inventory and equip it to tool slot
                if (PressedTool.quantity==0)
                {
                    Destroy(toolEquip.obj);
                    toolEquip = Item1.inventory.Remove(Item1.index);
                }
                

                InventoryTwo.Add(PreviousEquipTool);
                PlayerEquipment.EquipTool(toolEquip.obj);


                Debug.Log(PlayerEquipment.etool.obj);
                Debug.Log(PlayerEquipment.Tool);

                //reset pressed variables
                Item1.index = -1;
                Item1.InvNum = 0;

                currentEquip = "Tool";
                InfoboxDisplay(PlayerEquipment.etool, DisplayStyle.Flex, DisplayStyle.None, DisplayStyle.None);

                EquipToolButton.style.opacity = 1;
                EquipItemButton.style.opacity = (StyleFloat).6;
            }

        }

        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");

        DisplayEquippable();
        equipItemPressed = true;
    }

    //Display info box when item is Pressed
    void InfoboxDisplay(Item itemClicked, DisplayStyle toolInfoDisplayMode = DisplayStyle.None, DisplayStyle ItemContainerDisplayMode = DisplayStyle.None, DisplayStyle EquipButtonDisplayMode = DisplayStyle.Flex)
    {
        if (itemClicked != null)
        {
            root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.Flex;
            MarketWrapper value;
            market.Comparator.TryGetValue(itemClicked.obj.GetComponent<TypeLabel>().Type, out value);
            root.Q<Label>("NameOfItem").text = value.display_name;
            root.Q<Label>("TypeLabel").text = getTypeLabel(value.type);
            root.Q<Label>("QuantityValue").text = itemClicked.quantity.ToString();//itemClicked.obj.GetComponent<Quantity>().Value.ToString();

            ToolInfoContainer.style.display = toolInfoDisplayMode; //DisplayStyle.None;
            ItemContainerButtons.style.display = ItemContainerDisplayMode; //DisplayStyle.None;
            EquipButton.style.display = EquipButtonDisplayMode; //DisplayStyle.Flex;


        }
        else root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
    }

    void ItemClicked(EventBase obj)
    {
        var button = (Button)obj.target;

        int InvNum = int.Parse(button.name[0].ToString());

        int IndexNum = int.Parse(button.name.Substring(1));

        //First item pressed
        if (Item1.index == -1)
        {
            //Set the index within inventory
            Item1.index = IndexNum;
            button.style.opacity = 1;

            //Find what inventory it belongs to
            Item1.inventory = findInventory(InvNum, ref Item1);
            Item itemClicked = Item1.inventory.Retrieve(IndexNum);
            if (itemClicked != null)
            {
                Item1.type = itemClicked.obj.GetComponent<TypeLabel>().Type;
                InfoboxDisplay(itemClicked);

            }
            else root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;

        }
        //If we click on same item then unActivate it, and display equippable item if pressed
        else if (Item1.index == IndexNum && Item1.InvNum == InvNum)
        {
            Item1.index = -1;
            Item1.InvNum = 0;
            button.style.opacity = (StyleFloat).5;
            DisplayPressedEquipped();

        }
        //If second item pressed then swap Item1 with Item2
        else if (Item2.index == -1)
        {

            Item2.index = IndexNum;
            Item2.inventory = findInventory(InvNum, ref Item2);
            Item itemClicked = Item2.inventory.Retrieve(IndexNum);
            if (itemClicked != null)
            {
                Item2.type = itemClicked.obj.GetComponent<TypeLabel>().Type;
            }
            CheckAcceptableTransfer();
            swapItems();
            DisplayPressedEquipped();

        }
        //Shouldn't ever be here but just safe case
        else if (Item2.index == IndexNum && Item2.InvNum == InvNum)
        {
            Item2.index = -1;
            Item2.InvNum = 0;
            button.style.opacity = (StyleFloat).5;

        }

    }
    //Find what inventory item belonsg to and assign it to correct Inventory
    Inventory findInventory(int InvNum, ref InvAndIndexInfo Item)
    {
        if (InvNum == 1)
        {
            Item.InvNum = 1;
            return InventoryOne;
        }
        Item.InvNum = 2;
        return InventoryTwo;

    }

    //Displays equipped item that has been pressed
    void DisplayPressedEquipped()
    {
        //If user isn't pressing items inside inventory but had previously pressed
        //an equipped item go back to the pressed equipped and show its info.
        if (currentEquip.Equals("Tool"))
        {
            InfoboxDisplay(PlayerEquipment.etool, DisplayStyle.Flex, DisplayStyle.None, DisplayStyle.None);
            EquipItemButton.style.opacity = (StyleFloat).6;
        }
        else if (currentEquip.Equals("Item"))
        {
            InfoboxDisplay(PlayerEquipment.eitem, DisplayStyle.None, DisplayStyle.Flex, DisplayStyle.None);
            EquipToolButton.style.opacity = (StyleFloat).6;
        }
        else InfoboxDisplay(null);
    }

    //Make sure we have an acceptable transfer
    //Cannot MOVE tool to PLAYER INVENTORY
    bool CheckAcceptableTransfer()
    {

        if ((Item1.InvNum == 1 || Item2.InvNum == 1) &&
            (Item1.type > Base.GoodType.Tool_Start && Item1.type < Base.GoodType.Tool_End ||
            (Item2.type > Base.GoodType.Tool_Start && Item2.type < Base.GoodType.Tool_End)))

        {
            Item1.index = -1;
            Item2.index = -1;
            Item1.InvNum = 0;
            Item2.InvNum = 0;

            Item1.type = Base.GoodType.Tool_Start;
            Item2.type = Base.GoodType.Tool_Start;

            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");
            return false;

        }
        return true;
    }

    //Swap two items pressed
    void swapItems()
    {
        //If two things have been pressed then enter
        //and swap the items
        if (Item1.index != -1 && Item2.index != -1)
        {
            Item swap1 = Item1.inventory.Remove(Item1.index);
            Item swap2 = Item2.inventory.Remove(Item2.index);
            Item1.inventory.Insert(Item1.index, swap2);
            Item2.inventory.Insert(Item2.index, swap1);

            Item1.index = -1;
            Item2.index = -1;
            Item1.InvNum = 0;
            Item2.InvNum = 0;

            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");

        }
    }
}
