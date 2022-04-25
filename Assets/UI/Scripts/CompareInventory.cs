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

    [SerializeField]
    private Market market;


    public struct InvAndIndexInfo
    {
        public Inventory inventory;
        public int index;
        public int InvNum;
    };

    /*
    int Item1 = -1;
    int Item2 = -1;
    */

    InvAndIndexInfo Item1;

    InvAndIndexInfo Item2;


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
    void OnEnable()
    {
        Item1 = new InvAndIndexInfo();

        Item2 = new InvAndIndexInfo();

        Item1.index = -1;

        Item2.index = -1;


        allocateInventory(PlayerInvList, InventoryOne);
        allocateInventory(HouseInvList, InventoryTwo);


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
    void ItemClicked(EventBase obj)
    {
        var button = (Button)obj.target;

        int InvNum = int.Parse(button.name[0].ToString());
        int IndexNum = int.Parse(button.name.Substring(1));



        
        if (Item1.index == -1)
        {
            Item1.index = IndexNum;
            Item1.inventory = findInventory(InvNum, Item1);
            button.style.opacity = 1;
        }
        else if (Item1.index == InvNum && Item1.InvNum == IndexNum)
        {
            Item1.index = -1;
            button.style.opacity = (StyleFloat).5;

        }
        else if (Item2.index == -1)
        {
            Item2.index = IndexNum;
            Item2.inventory = findInventory(InvNum, Item2);
        }
        else if (Item2.index == InvNum && Item2.InvNum == IndexNum)
        {
            Item2.index = -1;
            button.style.opacity = (StyleFloat).5;

        }

    }
    Inventory findInventory(int InvNum, InvAndIndexInfo Item)
    {
        if(InvNum == 1)
        {
            Item.InvNum = 1;
            return InventoryOne;
        }
        Item.InvNum = 2;
        return InventoryTwo;
    }

    private void Update()
    {
        root.Focus();
        if (Item1.index != -1 && Item2.index != -1)
        {

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
    }
}
