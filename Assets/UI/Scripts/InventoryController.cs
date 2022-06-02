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

    [SerializeField]
    private GameObject Phone;

    [SerializeField]
    private GameObject InfoUI;

    //private Button FocusedButton;
    // Start is called before the first frame update

    void OnEnable()
    {
        InfoUI.SetActive(false);
        currentPressedItem = null;

        assignUItoVariables();
        root.Focus();
        assignButtonsToFunctions();

        findItemAndDisplay(PlayerEquipment.eitem, ref EquippedItem);
        DisplayInventory();

    }
    void assignUItoVariables()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        ItemInfo = root.Q<VisualElement>("ItemInfo");
        EquippedItem = root.Q<Button>("EquippedItem");
        addInvButton = root.Q<Button>("AddInvButton");
        ScrollViewSection = root.Q<VisualElement>("InventoryScrollView");
    }
    void assignButtonsToFunctions()
    {
        root.Q<Button>("EquipButton").clicked += EquipButtonClicked;
        EquippedItem.clickable.clickedWithEventInfo += ItemClicked;
        addInvButton.clicked += addInvClicked;
        root.Q<Button>("BackButton").clicked += backbutton;

    }
    void DisplayInventory()
    {
        VisualElement Current = new VisualElement();
        Current.AddToClassList("Row");

        int count = 0;
        int length = player.Size;
        for (int i = 0; i < length; ++i)
        {

            if (i != 0 && count % 5 == 0)
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

            if (tmpobj == null) InventoryItem.style.backgroundImage = null;
            else
            {
                MarketWrapper value;
                market.Comparator.TryGetValue(tmpobj.obj.GetComponent<TypeLabel>().Type, out value);

                InventoryItem.style.backgroundImage = value.picture;
            }
            //InventoryItem.style.backgroundImage = null;
            Current.Add(InventoryItem);
            ScrollViewSection.Add(Current);

            count += 1;

        }
        int extra = 0;
        if (length % 5 != 0)
            extra = 5 - (length % 5);

        addExtraBoxes(extra, ref Current);
    }
    void addExtraBoxes(int extra, ref VisualElement Current)
    {
        for (int i = 0; i < extra; ++i)
        {

            Button InventoryItem = new Button();

            InventoryItem.AddToClassList("SlotIcon");
            InventoryItem.AddToClassList("ItemButton");

            InventoryItem.style.visibility = Visibility.Hidden;
            Current.Add(InventoryItem);
        }

    }
    void backbutton()
    {
        InfoUI.SetActive(true);
        Phone.SetActive(true);
        this.gameObject.SetActive(false);
    }
    void addInvClicked()
    {
        int index = player.Add(PlayerEquipment.eitem);
        if (index == -1)
        {
            root.Q<VisualElement>("SpaceErrorWarning").style.display = DisplayStyle.Flex;
            addInvButton.style.display = DisplayStyle.None;
            return;
        }
        EquippedItem.style.backgroundImage = null;
        PlayerEquipment.eitem = null;
        PlayerEquipment.Item = null;
        EquippedItem.style.opacity = (StyleFloat).5;

        Button InvIndexToChange = root.Q<Button>(index.ToString());

        findItemAndDisplay(player.Retrieve(index), ref InvIndexToChange);
        ItemInfo.style.display = DisplayStyle.None;

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

    void EquipButtonClicked()
    {
        int index = int.Parse(currentPressedItem.name);

        Item previousItem = PlayerEquipment.eitem;
        PlayerEquipment.inventory = player;
        PlayerEquipment.EquipItem(index);
        player.Remove(index);
        player.Insert(index, previousItem);

        findItemAndDisplay(player.Retrieve(index), ref currentPressedItem);

        findItemAndDisplay(PlayerEquipment.eitem, ref EquippedItem);

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
        if (isequippable) CurrentCard = PlayerEquipment.eitem;
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
                QuantityNum.text = CurrentCard.quantity.ToString();
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
