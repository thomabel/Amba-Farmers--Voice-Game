using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompareInventory : MonoBehaviour
{
    public Inventory InventoryOne;
    public Inventory InventoryTwo;

    public InventoryList PlayerInvList;

    public InventoryList HouseInvList;

    private VisualElement root;

    private ScrollView ScrollViewOne;
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
    private Equipment PlayerEquipment;

    public struct InvAndIndexInfo
    {
        public Inventory inventory;
        public int index;
        public int InvNum;
        public Financials.GoodType type;
    };

    /*
    int Item1 = -1;
    int Item2 = -1;
    */

    InvAndIndexInfo Item1;

    InvAndIndexInfo Item2;

    bool equipItemPressed;
    string currentEquip;

    Button EquipToolButton;
    Button EquipItemButton;

    


    MarketWrapper findReference(Financials.GoodType tmp)
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

    //For testing... Inventories should most likely already be filled

    /*
    private void allocateInventory(InventoryList invList, Inventory inventory)
    {
        for (int i = 0; i < invList.length(); ++i)
        {
            Item tmp = new Item();


            tmp.obj = Instantiate(invList.FindCardIndex(i).item_prefab);

            tmp.obj.AddComponent<TypeLabel>();

            TypeLabel tmpLabel = tmp.obj.GetComponent<TypeLabel>();


            tmpLabel.Type = invList.FindCardIndex(i).type;


            tmp.quantity = 2;


            inventory.Add(tmp);
        }

    }
    */
    void OnEnable()
    {
        Item1 = new InvAndIndexInfo();

        Item2 = new InvAndIndexInfo();

        Item1.index = -1;

        Item2.index = -1;
        equipItemPressed = false;
        currentEquip = "empty";

        /*
        Item tmp = new Item();


        tmp.obj = Instantiate(market.Reference[4].item_prefab);

        tmp.obj.AddComponent<TypeLabel>();

        TypeLabel tmpLabel = tmp.obj.GetComponent<TypeLabel>();


        tmpLabel.Type = market.Reference[4].type;

        tmp.quantity = 2;
        tmp.obj.AddComponent<Quantity>();
        Quantity Quantmp = tmp.obj.GetComponent<Quantity>();
        Quantmp.Value = tmp.quantity;



        InventoryTwo.Add(tmp);
        */
        //allocateInventory(PlayerInvList, InventoryOne);
        //allocateInventory(HouseInvList, InventoryTwo);


        root = GetComponent<UIDocument>().rootVisualElement;


        ScrollViewOne = root.Q<ScrollView>("ScrollView1");
        ScrollViewOne.Clear();

        ScrollViewTwo = root.Q<ScrollView>("ScrollView2");
        ScrollViewTwo.Clear();

        Debug.Log(InventoryOne.Size);


        root.Q<Label>("Label1").text = InventoryOne.gameObject.name;
        root.Q<Label>("Label2").text = InventoryTwo.gameObject.name;

        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");


        EquipToolButton = root.Q<Button>("ToolButton");
        EquipToolButton.name = "Tool";
        EquipToolButton.clickable.clickedWithEventInfo += equipPressed;


        EquipItemButton = root.Q<Button>("ItemButton");
        EquipItemButton.name = "Item";
        EquipItemButton.clickable.clickedWithEventInfo += equipPressed;

        EquipButton = root.Q<Button>("Equip");
        EquipButton.clicked += equip;
        DisplayEquippable();

        ToolInfoContainer = root.Q<VisualElement>("ToolContainerButtons");

        ToolInfoContainerButton = root.Q<Button>("ToolButton");
        ToolInfoContainerButton.clicked += SendtoInventoryTwo;


        ItemContainerButtons = root.Q<VisualElement>("ItemContainerButtons");
        ItemPlayerInventory = root.Q<Button>("ItemPlayerInventory");
        //ItemPlayerInventory.text = "Add " + "\n" + InventoryOne.gameObject.name + " Inventory";
        ItemPlayerInventory.text = "Add\n <";
        ItemPlayerInventory.clickable.clickedWithEventInfo += AddtoInventory;

        ItemHouseInventory = root.Q<Button>("ItemHouseInventory");
        ItemHouseInventory.clickable.clickedWithEventInfo += AddtoInventory;
        //ItemHouseInventory.text = "Add " + "\n" + InventoryTwo.gameObject.name + " Inventory";
        ItemHouseInventory.text = "Add\n >";


    }
    void AddtoInventory(EventBase obj)
    {
        var button = (Button)obj.target;

        if (button.name.Equals("ItemPlayerInventory"))
        {
            Item itemObj = PlayerEquipment.item_obj;
            if (itemObj != null)
            {
                InventoryOne.Add(PlayerEquipment.item_obj);
                PlayerEquipment.item_obj = null;
            }
        }
        else
        {
            Item itemobj = PlayerEquipment.item_obj;
            if (itemobj != null)
            {
                InventoryTwo.Add(PlayerEquipment.item_obj);
                PlayerEquipment.item_obj = null;
            }

        }
        EquipItemButton.style.opacity = (StyleFloat).6;
        EquipItemButton.style.backgroundImage = null;
        equipItemPressed = false;

        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");
        DisplayEquippable();
        root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;

    }
    void SendtoInventoryTwo()
    {
        if (PlayerEquipment.tool_obj != null)
        {
            InventoryTwo.Add(PlayerEquipment.tool_obj);
            PlayerEquipment.tool_obj = null;
            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");
            DisplayEquippable();
        }
        EquipToolButton.style.opacity = (StyleFloat).6;
        EquipToolButton.style.backgroundImage = null;
        equipItemPressed = false;
        root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
    }
    void equipPressed(EventBase obj)
    {
        var button = (Button)obj.target;
        string name = button.name;

        if (equipItemPressed && currentEquip.Equals(name))
        {
            button.style.opacity = (StyleFloat).6;
            equipItemPressed = false;
        }
        else
        {
            currentEquip = name;
            button.style.opacity = 1;
            equipItemPressed = true;
            if (name.Equals("Tool"))
            {
                InfoboxDisplay(PlayerEquipment.tool_obj);
                EquipItemButton.style.opacity = (StyleFloat).6;
                ToolInfoContainer.style.display = DisplayStyle.Flex;
                ItemContainerButtons.style.display = DisplayStyle.None;
                EquipButton.style.display = DisplayStyle.None;
            }
            else
            {
                InfoboxDisplay(PlayerEquipment.item_obj);
                EquipToolButton.style.opacity = (StyleFloat).6;
                ToolInfoContainer.style.display = DisplayStyle.None;
                ItemContainerButtons.style.display = DisplayStyle.Flex;
                EquipButton.style.display = DisplayStyle.None;
            }
        }

    }
    void DisplayEquippable()
    {
        Debug.Log(PlayerEquipment.item_obj);
        if (PlayerEquipment.tool_obj != null)
        {
            MarketWrapper toolvalue;
            if (market.Comparator.TryGetValue(PlayerEquipment.tool_obj.obj.GetComponent<TypeLabel>().Type, out toolvalue))
            {
                EquipToolButton.style.backgroundImage = new StyleBackground(toolvalue.picture);
            }
            else
                EquipToolButton.style.backgroundImage = new StyleBackground();
        }
        if (PlayerEquipment.item_obj != null)
        {
            MarketWrapper itemvalue;
            if (market.Comparator.TryGetValue(PlayerEquipment.item_obj.obj.GetComponent<TypeLabel>().Type, out itemvalue))
            {
                EquipItemButton.style.backgroundImage = new StyleBackground(itemvalue.picture);
            }
            else
                EquipItemButton.style.backgroundImage = new StyleBackground();
        }

    }

    void Display(Inventory inventory, ScrollView viewScroller, string InvNum)
    {
        viewScroller.Clear();

        VisualElement Current = new VisualElement();
        Current.AddToClassList("Row");

        int count = 0;
        for (int i = 0; i < inventory.Size; ++i)
        {
            if (i != 0 && count % 2 == 0)
            {
                Current = new VisualElement();
                Current.AddToClassList("Row");
            }

            Button InventoryItem = new Button();
            //InventoryItem.clickable.clickedWithEventInfo += ItemClicked;

            InventoryItem.name = InvNum + i.ToString();
            InventoryItem.AddToClassList("SlotIcon");
            InventoryItem.AddToClassList("ItemButton");
            InventoryItem.AddToClassList("SmallerItemButton");

            InventoryItem.clickable.clickedWithEventInfo += ItemClicked;

            MarketWrapper tmpType = null;

            //Debug.Log(InventoryOne.Retrieve(i).obj);
            if (inventory.Retrieve(i) != null)
            {

                tmpType = findReference(inventory.Retrieve(i).obj.GetComponent<TypeLabel>().Type);
                InventoryItem.style.backgroundImage = tmpType.picture;

            }
            else InventoryItem.style.backgroundImage = null;


            Current.Add(InventoryItem);
            viewScroller.Add(Current);

            count += 1;

            //PlayerInventory.Insert(i, inventory.FindCardIndex(i).item_prefab);


        }
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
    }
    string getTypeLabel(Financials.GoodType typeOfItem)
    {
        if (typeOfItem > Financials.GoodType.Tool_Start && typeOfItem < Financials.GoodType.Tool_End)
        {
            return "Tool";
        }
        else if (typeOfItem > Financials.GoodType.Seed_Start && typeOfItem < Financials.GoodType.Seed_End)
        {
            return "Seed";
        }
        else if (typeOfItem > Financials.GoodType.Animal_Start && typeOfItem < Financials.GoodType.Animal_End)
        {
            return "Animal";
        }
        return "Fruit";

    }
    void equip()
    {
        if (getTypeLabel(Item1.type) == "Seed")
        {
            Item PreviousEquipItem = PlayerEquipment.item_obj;
            PlayerEquipment.inventory = Item1.inventory;
            PlayerEquipment.EquipItem(Item1.index);
            Item1.inventory.Remove(Item1.index);
            Item1.inventory.Insert(Item1.index, PreviousEquipItem);
            Item1.index = -1;
            Item1.InvNum = 0;

            currentEquip = "Item";
            InfoboxDisplay(PlayerEquipment.item_obj);
            ToolInfoContainer.style.display = DisplayStyle.None;
            ItemContainerButtons.style.display = DisplayStyle.Flex;
            EquipButton.style.display = DisplayStyle.None;

            EquipToolButton.style.opacity = (StyleFloat).6;
            EquipItemButton.style.opacity = 1;

        }
        else if (getTypeLabel(Item1.type) == "Tool" || getTypeLabel(Item1.type) == "Fruit")
        {

            Item PreviousEquipTool = PlayerEquipment.tool_obj;
            Debug.Log("ToolEquip = ");
            //Debug.Log(PlayerEquipment.tool_obj);
            //Debug.Log(Item1.inventory.Retrieve(Item1.index).obj);
            if (PreviousEquipTool == null)
            {
                PlayerEquipment.tool_obj = Item1.inventory.Retrieve(Item1.index);
                //PlayerEquipment.tool_obj.obj.SetActive(true);
            }
            else PlayerEquipment.EquipTool(Item1.inventory.Retrieve(Item1.index).obj);
            Debug.Log("ToolEquip = ");
            Debug.Log(PlayerEquipment.tool_obj);
            Item1.inventory.Remove(Item1.index);
            Item1.inventory.Insert(Item1.index, PreviousEquipTool);
            Item1.index = -1;
            Item1.InvNum = 0;

            currentEquip = "Tool";
            InfoboxDisplay(PlayerEquipment.tool_obj);
            ToolInfoContainer.style.display = DisplayStyle.Flex;
            ItemContainerButtons.style.display = DisplayStyle.None;
            EquipButton.style.display = DisplayStyle.None;

            EquipToolButton.style.opacity = 1;
            EquipItemButton.style.opacity = (StyleFloat).6;
            
        }
        ScrollViewOne.Clear();
        ScrollViewTwo.Clear();
        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");

        DisplayEquippable();
        equipItemPressed = true;
    }


   void InfoboxDisplay(Item itemClicked)
    {
        if (itemClicked != null)
        {
            root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.Flex;
            MarketWrapper value;
            market.Comparator.TryGetValue(itemClicked.obj.GetComponent<TypeLabel>().Type, out value);
            root.Q<Label>("NameOfItem").text = value.display_name;
            root.Q<Label>("TypeLabel").text = getTypeLabel(value.type);
            root.Q<Label>("QuantityValue").text = itemClicked.obj.GetComponent<Quantity>().Value.ToString();
            ToolInfoContainer.style.display = DisplayStyle.None;
            ItemContainerButtons.style.display = DisplayStyle.None;
            EquipButton.style.display = DisplayStyle.Flex;


        }
        else root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
    }

    void ItemClicked(EventBase obj)
    {
        var button = (Button)obj.target;

        int InvNum = int.Parse(button.name[0].ToString());

        int IndexNum = int.Parse(button.name.Substring(1));
        /*
        int IndexNum;
        if (InvNum == 3) {
            IndexNum = -2;
        }
        else IndexNum = int.Parse(button.name.Substring(1));
        */
        

        if (Item1.index == -1)
        {            
            Item1.index = IndexNum;
            button.style.opacity = 1;
            Item1.inventory = findInventory(InvNum, ref Item1);
            //button.style.opacity = 1;
            Item itemClicked = Item1.inventory.Retrieve(IndexNum);
            if (itemClicked != null)
            {
                Item1.type = itemClicked.obj.GetComponent<TypeLabel>().Type;
                root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.Flex;
                MarketWrapper value;
                market.Comparator.TryGetValue(Item1.inventory.Retrieve(IndexNum).obj.GetComponent<TypeLabel>().Type, out value);
                root.Q<Label>("NameOfItem").text = value.display_name;
                root.Q<Label>("TypeLabel").text = getTypeLabel(value.type);
                root.Q<Label>("QuantityValue").text = Item1.inventory.Retrieve(IndexNum).obj.GetComponent<Quantity>().Value.ToString();
                ToolInfoContainer.style.display = DisplayStyle.None;
                ItemContainerButtons.style.display = DisplayStyle.None;
                EquipButton.style.display = DisplayStyle.Flex;

            }
            else root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
            /*
            if (InvNum != 3)
            {
                Item itemClicked = Item1.inventory.Retrieve(IndexNum);
                if (itemClicked != null)
                {
                    Item1.type = itemClicked.obj.GetComponent<TypeLabel>().Type;
                    root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.Flex;
                    MarketWrapper value;
                    market.Comparator.TryGetValue(Item1.inventory.Retrieve(IndexNum).obj.GetComponent<TypeLabel>().Type, out value);
                    root.Q<Label>("NameOfItem").text = value.display_name;
                    root.Q<Label>("QuantityValue").text = Item1.inventory.Retrieve(IndexNum).obj.GetComponent<Quantity>().Value.ToString();
                }
                else root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
            }
            */
        }
        else if (Item1.index == IndexNum && Item1.InvNum == InvNum)
        {
            //root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;
            Item1.index = -1;
            Item1.InvNum = 0;
            button.style.opacity = (StyleFloat).5;
            if (currentEquip.Equals("Tool"))
            {
                InfoboxDisplay(PlayerEquipment.tool_obj);
                EquipItemButton.style.opacity = (StyleFloat).6;
            }
            else
            {
                InfoboxDisplay(PlayerEquipment.item_obj);
                EquipToolButton.style.opacity = (StyleFloat).6;
            }

        }
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


            if (currentEquip.Equals("Tool"))
            {
                InfoboxDisplay(PlayerEquipment.tool_obj);
                EquipItemButton.style.opacity = (StyleFloat).6;
            }
            else
            {
                InfoboxDisplay(PlayerEquipment.item_obj);
                EquipToolButton.style.opacity = (StyleFloat).6;
            }
            /*
            if (InvNum != 3)
            {
                Item itemClicked = Item2.inventory.Retrieve(IndexNum);
                if (itemClicked != null)
                {
                    Item2.type = itemClicked.obj.GetComponent<TypeLabel>().Type;
                }
                CheckAcceptableTransfer();
            }
            */
        }
        else if (Item2.index == IndexNum && Item2.InvNum == InvNum)
        {
            Item2.index = -1;
            Item2.InvNum = 0;
            button.style.opacity = (StyleFloat).5;

        }

    }
    Inventory findInventory(int InvNum, ref InvAndIndexInfo Item)
    {
        if (InvNum == 1)
        {
            Item.InvNum = 1;
            return InventoryOne;
        }
        Item.InvNum = 2;
        return InventoryTwo;
        /*
        else if (InvNum == 2)
        {
            Item.InvNum = 2;
            return InventoryTwo;
        }

        Item.InvNum = 3;
        return null;
        */
    }


    void changeToUnactive(Button clickedButton)
    {

    }
    bool CheckAcceptableTransfer()
    {
        Debug.Log("OUT");
        Debug.Log(Item1.InvNum);
        Debug.Log(Item1.type);
        Debug.Log(Item2.InvNum);
        Debug.Log(Item2.type);
        if ((Item1.InvNum == 1 || Item2.InvNum == 1) &&
            (Item1.type > Financials.GoodType.Tool_Start && Item1.type < Financials.GoodType.Tool_End ||
            (Item2.type > Financials.GoodType.Tool_Start && Item2.type < Financials.GoodType.Tool_End)))

        {
            Debug.Log("IN");
            Item1.index = -1;
            Item2.index = -1;
            Item1.InvNum = 0;
            Item2.InvNum = 0;

            Item1.type = Financials.GoodType.Tool_Start;
            Item2.type = Financials.GoodType.Tool_Start;

            ScrollViewOne.Clear();
            ScrollViewTwo.Clear();
            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");
            return false;
            
        }
        return true;
    }
    private void Update()
    {
        root.Focus();
        /*
        if (Item1.index != -1 && Item2.index != -1)
        {
            //if(Item1)

            Item swap1 = Item1.inventory.Remove(Item1.index);
            Item swap2 = Item2.inventory.Remove(Item2.index);
            Item1.inventory.Insert(Item1.index, swap2);
            Item2.inventory.Insert(Item2.index, swap1);

            ScrollViewOne.Clear();
            ScrollViewTwo.Clear();
            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");
            Item1.index = -1;
            Item2.index = -1;
            Item1.InvNum = 0;
            Item2.InvNum = 0;
        }
        */

        if (Item1.index == -1 && !equipItemPressed) //else if(Item1.index == -1)
        {
            root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;

        }
        
    }

    void swapItems() {
        if (Item1.index != -1 && Item2.index != -1)
        {
            //if(Item1)

            Item swap1 = Item1.inventory.Remove(Item1.index);
            Item swap2 = Item2.inventory.Remove(Item2.index);
            Item1.inventory.Insert(Item1.index, swap2);
            Item2.inventory.Insert(Item2.index, swap1);

            ScrollViewOne.Clear();
            ScrollViewTwo.Clear();
            Display(InventoryOne, ScrollViewOne, "1");
            Display(InventoryTwo, ScrollViewTwo, "2");
            Item1.index = -1;
            Item2.index = -1;
            Item1.InvNum = 0;
            Item2.InvNum = 0;
        }
    }
}
