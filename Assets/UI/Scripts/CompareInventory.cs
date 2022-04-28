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

    Button EquipToolButton;
    Button EquipItemButton;


    MarketWrapper findReference(Financials.GoodType tmp)
    {
        for (int i = 0; i < market.Reference.Count; ++i)
        {
            if(market.Reference[i].type == tmp)
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
        EquipToolButton.name = "3Tool";
        //EquipToolButton.clickable.clickedWithEventInfo += ItemClicked;


        EquipItemButton = root.Q<Button>("ItemButton");
        EquipItemButton.name = "3Item";

        EquipButton = root.Q<Button>("Equip");
        EquipButton.clicked += equip;
        DisplayEquippable();

        //EquipItemButton.clickable.clickedWithEventInfo += ItemClicked;

        /*
        if (PlayerEquipment.tool_obj != null)
        {
            MarketWrapper Value;
            market.Comparator.TryGetValue(PlayerEquipment.tool_obj.obj.GetComponent<TypeLabel>().Type, out Value);
            EquipToolButton.style.backgroundImage = Value.picture;
        }
        else
        {
            EquipToolButton.style.backgroundImage = null;
        }


        //PlayerEquipment.EquipItem(0);

        if (PlayerEquipment.item_obj != null)
        {
            

            MarketWrapper Value2;
            market.Comparator.TryGetValue(PlayerEquipment.item_obj.obj.GetComponent<TypeLabel>().Type, out Value2);
            EquipItemButton.style.backgroundImage = Value2.picture;
        }
        else
        {
            EquipItemButton.style.backgroundImage = null;
        }
        */



    }

    void DisplayEquippable()
    {
        Debug.Log(PlayerEquipment.item_obj);
        if (PlayerEquipment.tool_obj != null)
        {
            MarketWrapper toolvalue;
            if (market.Comparator.TryGetValue(PlayerEquipment.tool_obj.obj.GetComponent<TypeLabel>().Type, out toolvalue))
                EquipToolButton.style.backgroundImage = toolvalue.picture;
            else
                EquipToolButton.style.backgroundImage = null;
        }
        if (PlayerEquipment.item_obj != null)
        {
            MarketWrapper itemvalue;
            if (market.Comparator.TryGetValue(PlayerEquipment.item_obj.obj.GetComponent<TypeLabel>().Type, out itemvalue))
                EquipItemButton.style.backgroundImage = itemvalue.picture;
            else
                EquipItemButton.style.backgroundImage = null;
        }

    }

    void Display(Inventory inventory, ScrollView viewScroller, string InvNum)
    {
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
            DisplayEquippable();
            Item PreviousEquipItem = PlayerEquipment.item_obj;
            PlayerEquipment.EquipItem(Item1.index);
            Item1.inventory.Remove(Item1.index);
            Item1.inventory.Insert(Item1.index, PreviousEquipItem);
            Item1.index = -1;
        }
        else if (getTypeLabel(Item1.type) == "Tool" || getTypeLabel(Item1.type) == "Fruit")
        {
            
            DisplayEquippable();
            Item PreviousEquipTool = PlayerEquipment.tool_obj;
            PlayerEquipment.EquipTool(Item1.inventory.Retrieve(Item1.index).obj);
            Debug.Log("ToolEquip = ");
            Debug.Log(PlayerEquipment.tool_obj);
            Item1.inventory.Remove(Item1.index);
            Item1.inventory.Insert(Item1.index, PreviousEquipTool);
            Item1.index = -1;
        }
        ScrollViewOne.Clear();
        ScrollViewTwo.Clear();
        Display(InventoryOne, ScrollViewOne, "1");
        Display(InventoryTwo, ScrollViewTwo, "2");
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
            Item1.inventory = findInventory(InvNum, ref Item1);
            button.style.opacity = 1;
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
            button.style.opacity = (StyleFloat).5;

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
        }
        else if(Item1.index == -1)
        {
            root.Q<VisualElement>("QuanInfoContainer").style.display = DisplayStyle.None;

        }
    }
}
