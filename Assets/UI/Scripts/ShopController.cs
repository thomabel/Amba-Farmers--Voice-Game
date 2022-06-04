using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class ShopController : MonoBehaviour
{
    private MainUI mainUI;

    private VisualElement ScrollViewSection;
    private Button CardButton;
    private VisualElement PictureContainer;
    private VisualElement InfoContainer;
    private VisualElement StatusContainer;
    private Label Picture;
    private Label Name;
    private Label Price;
    private Label InventoryName;

    private Label inCartLabel;
    private Label StatusPicture;

    [SerializeField]
    private Texture2D Checkmark;
    [SerializeField]
    private Texture2D X;

    private VisualElement root;

    private Button checkout;

    private Button CheckoutBackButton;

    [SerializeField]
    private Account player;


    private TextField quantityField;

    private int total = 0;
    private int SellTotal = 0;

    [SerializeField]
    private GameObject PhoneGameObject;
    [SerializeField]
    private GameObject controls;

    private List<int> PlantBuyList;
    private List<int> ToolBuyList;
    private List<int> LivestockBuyList;

    private List<int> SellList;


    private Dictionary<int, float> PlantQuantity;
    private Dictionary<int, float> ToolQuantity;
    private Dictionary<int, float> LiveStockQuantity;
    private Dictionary<int, float> SellQuantity;

    [SerializeField]
    Market market;

    private List<MarketWrapper> Plants;
    private List<MarketWrapper> Animals;
    private List<MarketWrapper> Tools;


    [SerializeField]
    private Inventory PlayerInv;


    private string currentTab;

    private bool isBuy;

    private Button buyButton;
    private Button sellButton;

    private Label MoneyLabel;
    private Label CheckoutMoneyLabel;
    private Label SubtotalLabel;

    private Label SoldSubtotalLabel;
    private VisualElement SuccessPurchase;

    private VisualElement ItemsInCheckout;

    private VisualElement emptySellMessage;

    [SerializeField]
    private ShelterHandler shelters;


    // Start is called before the first frame update
    void OnEnable()
    {
        Initialize();
        AddToLists();

        root = GetComponent<UIDocument>().rootVisualElement;
        assignUItoVariables();
        assignButtonsToFunctions();

        currentTab = "ToolTab";
        root.Q<Button>("ToolTab").style.opacity = 1;
        sellButton.name = "Sell";
        buyButton.name = "Buy";
        root.Q<Label>("MoneyLabel").text = player.Balance().ToString();

        DisplayCards("T");

        isBuy = true;

        root.Focus();

    }
    void Initialize()
    {
        mainUI = new MainUI();
        PlantQuantity = new Dictionary<int, float>();
        ToolQuantity = new Dictionary<int, float>();
        LiveStockQuantity = new Dictionary<int, float>();
        SellQuantity = new Dictionary<int, float>();

        PlantBuyList = new List<int>();
        ToolBuyList = new List<int>();
        LivestockBuyList = new List<int>();
        SellList = new List<int>();

        Plants = new List<MarketWrapper>();
        Animals = new List<MarketWrapper>();
        Tools = new List<MarketWrapper>();

    }
    void assignUItoVariables()
    {
        MoneyLabel = root.Q<Label>("MoneyLabel");
        SubtotalLabel = root.Q<Label>("SubtotalLabel");
        SoldSubtotalLabel = root.Q<Label>("SoldSubtotalLabel");
        CheckoutMoneyLabel = root.Q<Label>("CheckoutPlayerBalance");

        SuccessPurchase = root.Q<VisualElement>("ItemsPurchased");
        ItemsInCheckout = root.Q<VisualElement>("ItemsListContainer");
        sellButton = root.Q<Button>("SellButton");
        buyButton = root.Q<Button>("BuyButton");
        ScrollViewSection = root.Q<VisualElement>("ScrollView");
        checkout = root.Q<Button>("CheckoutButton");
        CheckoutBackButton = root.Q<Button>("CheckoutBackButton");
        emptySellMessage = root.Q<VisualElement>("EmptySellListContainer");

    }

    void assignButtonsToFunctions()
    {
        root.Q<Button>("ToolTab").clickable.clickedWithEventInfo += ClickedTabs;

        root.Q<Button>("PlantTab").clickable.clickedWithEventInfo += ClickedTabs;

        root.Q<Button>("LivestockTab").clickable.clickedWithEventInfo += ClickedTabs;
        sellButton.clicked += SellButtonPressed;
        buyButton.clicked += BuyButtonPressed;
        root.Q<Button>("CheckoutActualButton").clicked += CheckoutOperation;

        root.Q<Button>("BackButtonToFarm").clicked += BackButtonToFarm;
        checkout.clicked += CheckoutButtonPressed;
        CheckoutBackButton.clicked += CheckoutBackButtonPressed;

    }

    //Add the buy market to the lists with UI to display later
    void AddToLists()
    {
        for (int i = 0; i < market.Reference.Count; ++i)
        {
            if (market.Reference[i].type > Base.GoodType.Tool_Start && market.Reference[i].type < Base.GoodType.Tool_End)
            {
                Tools.Add(market.Reference[i]);
            }
            else if (market.Reference[i].type > Base.GoodType.Seed_Start && market.Reference[i].type < Base.GoodType.Seed_End)
            {
                Plants.Add(market.Reference[i]);
            }
            else if (market.Reference[i].type > Base.GoodType.Animal_Start && market.Reference[i].type < Base.GoodType.Animal_End)
            {
                Animals.Add(market.Reference[i]);
            }

        }
        market.Sellables.Clear();
        market.PopulateSellables();

        Debug.Log(market.Sellables.Count);
    }


    //Buy or sell tabs clicked, set active/unactive styles and display appropriate data
    void BuyOrSellTabClicked(Visibility VisibleOrHidden, Button hideButton, Button ShowButton, string TypeOfCardToDisplay)
    {
        root.Q<VisualElement>("TabButtonContainer").style.visibility = VisibleOrHidden;
        DisplayCards(TypeOfCardToDisplay);
        ShowButton.RemoveFromClassList("ButtonUnActive");
        hideButton.AddToClassList("ButtonUnActive");

    }

    void BuyButtonPressed()
    {
        if (isBuy) return;

        isBuy = true;
        BuyOrSellTabClicked(Visibility.Visible, sellButton, buyButton, currentTab[0].ToString());

    }
    void SellButtonPressed()
    {
        if (!isBuy) return;

        isBuy = false;

        market.Sellables.Clear();
        market.PopulateSellables();

        BuyOrSellTabClicked(Visibility.Hidden, buyButton, sellButton, "S");
    }

    void DisplayCards(string ItemsToDisplay)
    {
        ScrollViewSection.Clear();
        if (ItemsToDisplay.Equals("P")) DisplayCards(Plants, PlantBuyList, true);
        else if (ItemsToDisplay.Equals("T")) DisplayCards(Tools, ToolBuyList, true);
        else if (ItemsToDisplay.Equals("L")) DisplayCards(Animals, LivestockBuyList, true);
        else DisplayCards(null, SellList, false);
    }

    void DisplayCards(List<MarketWrapper> ItemCards, List<int> BuyOrSellList, bool IsBuyMode)
    {
        int length;
        if (IsBuyMode) length = ItemCards.Count;
        else
        {
            length = market.Sellables.Count;
            //Empty sellable list so display empty list message
            if (length == 0)
            {

                mainUI.ShowOrHideVisualElements(ref emptySellMessage, ref ScrollViewSection);
                return;
            }
        }

        mainUI.ShowOrHideVisualElements(ref ScrollViewSection, ref emptySellMessage);
        Label SellableQuantity = null;

        //Display Sellable or Buy cards
        for (int i = 0; i < length; ++i)
        {
            MarketWrapper item = null;
            if (IsBuyMode) item = ItemCards[i];
            else
            {
                item = market.Sellables[i].wrap;
                if (market.Sellables[i].Animal == null)
                {
                    Item itemTmp = market.Sellables[i].inv.Retrieve(market.Sellables[i].index);
                    //itemTmp.quantity = (float).453221;
                    if (itemTmp.quantity >= 100)
                        itemTmp.quantity = (float) Decimal.Round((Decimal)itemTmp.quantity, 1);
                    else if(itemTmp.quantity >= 10 && itemTmp.quantity < 100)
                        itemTmp.quantity = (float)Decimal.Round((Decimal)itemTmp.quantity, 2);
                    else
                        itemTmp.quantity = (float)Decimal.Round((Decimal)itemTmp.quantity, 3);
                    mainUI.createLabel(ref SellableQuantity, "InventoryName", null, null, "Q: " + itemTmp.quantity.ToString());
                    
                }
                else
                    mainUI.createLabel(ref SellableQuantity, "InventoryName", null, null, "W: " + market.Sellables[i].Animal.GetComponent<Animal>().weight);
                /*
                InventoryName = new Label();
                InventoryName.text = market.Sellables[i].inv.name + "\n" + "Inventory";
                InventoryName.AddToClassList("InventoryName");
                */

            }


            mainUI.createButton(ref CardButton, "CardButton", null, i.ToString(), null);

            CardButton.clickable.clickedWithEventInfo += ShopItemPressed;


            mainUI.createVisualElement(ref PictureContainer, "PictureContainer", null, null, null);

            mainUI.createLabel(ref Picture, "Picture", item.picture, null, null);

            PictureContainer.Add(Picture);


            mainUI.createVisualElement(ref InfoContainer, "InfoContainer", null, null, null);

            mainUI.createLabel(ref Name, "Name", null, null, item.display_name);
            mainUI.createLabel(ref Price, "Price", null, null, "$" + item.PriceOf().ToString());


            InfoContainer.Add(Name);
            InfoContainer.Add(Price);

            if (!IsBuyMode) InfoContainer.Add(SellableQuantity);//InfoContainer.Add(InventoryName);

            mainUI.createVisualElement(ref StatusContainer, "Status", null, null, null);
            mainUI.createLabel(ref inCartLabel, "InCartLabel", null, null, "In Cart");
            mainUI.createLabel(ref StatusPicture, "StatusPicture", null, "Status" + i.ToString(), null);


            if (BuyOrSellList.Contains(i))
                StatusPicture.style.backgroundImage = Checkmark;
            else StatusPicture.style.backgroundImage = X;

            StatusContainer.Add(inCartLabel);
            StatusContainer.Add(StatusPicture);


            CardButton.Add(PictureContainer);
            CardButton.Add(InfoContainer);
            CardButton.Add(StatusContainer);


            ScrollViewSection.Add(CardButton);
            //ItemCards[i].quantity = 1;

        }
    }
    //Updates text fields with balance changes
    void UpdateMoneyBalance()
    {
        /*
        SellTotal = Decimal.Floor(SellTotal);
        total = Decimal.Floor(total);
        */

        if (MoneyLabel != null && SubtotalLabel != null &&
            SoldSubtotalLabel != null && CheckoutMoneyLabel != null)
        {
            MoneyLabel.text = player.Balance().ToString();
            SubtotalLabel.text = total.ToString();
            SoldSubtotalLabel.text = SellTotal.ToString();
            CheckoutMoneyLabel.text = player.Balance().ToString();
        }
    }

    void ShopItemPressed(EventBase obj)
    {
        var button = (Button)obj.target;

        if (!isBuy)
        {
            ShopItemPressed(SellList, button.name, SellQuantity, 'S');
            return;
        }

        if (currentTab.Equals("PlantTab")) ShopItemPressed(PlantBuyList, button.name, PlantQuantity,'B');
        else if (currentTab.Equals("ToolTab")) ShopItemPressed(ToolBuyList, button.name, ToolQuantity, 'B');
        else ShopItemPressed(LivestockBuyList, button.name, LiveStockQuantity, 'B');

    }

    void ShopItemPressed(List<int> ItemType, string itemName, Dictionary<int, float> QuantityMap, char BuyMode)
    {
        int index = int.Parse(itemName);
        Label Status = root.Q<Label>("Status" + index);

        //Change Status Image
        if (Status.style.backgroundImage == X)
        {
            Status.style.backgroundImage = Checkmark;
            ItemType.Add(index);
            float quantity = findQuantity(BuyMode, index, QuantityMap);
            QuantityMap.Add(index, quantity);
        }
        else
        {
            Status.style.backgroundImage = X;
            ItemType.Remove(index);
            QuantityMap.Remove(index);
        }
    }

    float findQuantity(char BuyMode, int index, Dictionary<int, float> QuantityMap)
    {
        if (BuyMode.Equals('S'))
        {
            float quantity = 1;
            if (market.Sellables[index].Animal == null)
                quantity = market.Sellables[index].inv.Retrieve(market.Sellables[index].index).quantity;
            return quantity;
            //QuantityMap.Add(index, quantity);
        }
        else
        {
            float quantity = 0;
            if (currentTab.Equals("PlantTab"))
                quantity = 99 - market.TotalNumberOfItems(Plants[index].type);

            else if (currentTab.Equals("ToolTab"))
                quantity = 99 - market.TotalNumberOfItems(Tools[index].type);
            else
            {
                quantity = shelters.GetCapacity(Animals[index].type) - shelters.GetPopulation(Animals[index].type);

            }

            if(quantity <= 0)
                return 0;

            return 1;
        }
    }
    void CheckNoItems()
    {
        VisualElement CheckoutContent = root.Q<VisualElement>("CheckoutContent");
        VisualElement NoItemMessage = root.Q<VisualElement>("NoItems");

        if (PlantBuyList.Count == 0 && ToolBuyList.Count == 0
            && LivestockBuyList.Count == 0 && SellList.Count == 0)
        {
            mainUI.ShowOrHideVisualElements(ref NoItemMessage, ref CheckoutContent);
            CheckoutMoneyLabel.text = player.Balance().ToString();

        }
        else
        {
            mainUI.ShowOrHideVisualElements(ref CheckoutContent, ref NoItemMessage);
        }
    }
    //Button to go to checkout
    void CheckoutButtonPressed()
    {
        //Display Checkout Content
        root.Q<VisualElement>("MainContainer").style.display = DisplayStyle.None;
        root.Q<VisualElement>("CheckoutPage").style.display = DisplayStyle.Flex;

        CheckNoItems();

        total = 0;
        SellTotal = 0;
        CheckoutItemWrapper();
        UpdateMoneyBalance();
        /*
        SubtotalLabel.text = total.ToString();
        SoldSubtotalLabel.text = SellTotal.ToString();
        */


    }

    //Back Button to go back to Main shop
    void CheckoutBackButtonPressed()
    {

        root.Q<VisualElement>("CheckoutPage").style.display = DisplayStyle.None;
        root.Q<VisualElement>("MainContainer").style.display = DisplayStyle.Flex;


        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        Debug.Log(total);
        CheckoutScrollView.Clear();

        if (isBuy) DisplayCards(currentTab[0].ToString());
        else DisplayCards("S");

        mainUI.ShowOrHideVisualElements(ref ItemsInCheckout, ref SuccessPurchase);

    }

    //Display checkout items either sellable or buy cards
    void checkoutItemsDisplay(List<int> ListType, string BuyOrSell, string typeofItem, List<MarketWrapper> items, Dictionary<int, float> QuantityMap)
    {
        ListType.Sort();
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        string ChosenType = "";
        bool isBuyMode;
        if (BuyOrSell.Equals("S"))
        {
            ChosenType = "Selling";
            isBuyMode = false;

        }
        else
        {
            ChosenType = "Buying";
            isBuyMode = true;

        }

        for (int i = 0; i < ListType.Count; ++i)
        {
            MarketWrapper item = null;
            if (isBuyMode) item = items[ListType[i]];
            else item = market.Sellables[ListType[i]].wrap;

            CheckoutScrollView.AddToClassList("CheckoutScrollViewList");

            VisualElement checkoutCard = null;
            mainUI.createVisualElement(ref checkoutCard, "CheckoutCard", null, null, null);

            Button RemoveItem = null;
            mainUI.createButton(ref RemoveItem, "RemoveItemButton", null, BuyOrSell + typeofItem + ListType[i], "X");
            RemoveItem.clickable.clickedWithEventInfo += CancelCheckoutItem;
            checkoutCard.Add(RemoveItem);

            VisualElement CheckoutItemInfoContainer = null;
            mainUI.createVisualElement(ref CheckoutItemInfoContainer, "CheckoutItemNameContainer", null, null, null);

            Label CheckoutItemInfoLabel = null;
            string quantityValue = "";

            if (!isBuyMode)
            {
                bool isAnimal = market.Sellables[ListType[i]].Animal != null;
                if (!isAnimal)
                {
                    Item itemTmp = market.Sellables[ListType[i]].inv.Retrieve(market.Sellables[ListType[i]].index);
                    quantityValue = "Q: " + itemTmp.quantity.ToString();
                }
                else
                {
                    quantityValue = "W: " + market.Sellables[ListType[i]].Animal.
                    GetComponent<Animal>().weight;
                }
            }
            mainUI.createLabel(ref CheckoutItemInfoLabel, "CheckoutItemInfoLabel", null, null, ChosenType + " " + item.display_name
                + " " + "$" + item.PriceOf().ToString() + "\n" + quantityValue);

            CheckoutItemInfoContainer.Add(CheckoutItemInfoLabel);
            checkoutCard.Add(CheckoutItemInfoContainer);

            if (isBuyMode || (!isBuyMode && market.Sellables[ListType[i]].Animal == null))
            {
                VisualElement QuantityContainer = null;
                mainUI.createVisualElement(ref QuantityContainer, "QuantityContainer", null, null, null);


                TextField Quantity = null;
                
                mainUI.createTextField(ref Quantity, "Quantity", 5, QuantityMap[ListType[i]].ToString(), "Q" + typeofItem + ListType[i]);

                //When user presses quantity button and changes it then we enter this callback
                Quantity.RegisterValueChangedCallback((evt) =>
                {

                    int totaltmp = 0;
                    float maxQuantity = 0;


                    TextField tmp = (TextField)evt.target;

                    VisualElement parent1 = tmp.GetFirstAncestorOfType<VisualElement>();
                    VisualElement parent2 = parent1.GetFirstAncestorOfType<VisualElement>();

                    float n = 0;

                    int num = int.Parse(tmp.name.Substring(2));
                    bool intType = false;

                    if (isBuyMode)
                    {
                        totaltmp = total;
                        if (items[num].type > Base.GoodType.Animal_Start && items[num].type < Base.GoodType.Animal_End)
                        {
                            maxQuantity = shelters.GetCapacity(items[num].type) - shelters.GetPopulation(items[num].type);
                        }
                        else
                        {
                            if (market.TotalNumberOfItems(items[num].type) != -1)
                                maxQuantity = 99 - market.TotalNumberOfItems(items[num].type);
                            else maxQuantity = 99;
                            //maxQuantity = 99;
                        }

                        intType = items[num].qty_type == Base.QuantityType.Integer;

                    }
                    else
                    {
                        totaltmp = SellTotal;

                        Item obj = market.Sellables[num].inv.Retrieve(market.Sellables[num].index);
                        maxQuantity = obj.quantity;
                        MarketWrapper value;
                        market.Comparator.TryGetValue(obj.obj.GetComponent<TypeLabel>().Type, out value);
                        intType = value.qty_type == Base.QuantityType.Integer;
                    }

                    if (tmp.value != null && evt.previousValue.Contains("."))
                    {
                        char ch = '.';
                        //https://www.techiedelight.com/count-occurrences-of-character-within-string-csharp/
                        int freq = evt.newValue.Count(x => (x == ch));

                        if (freq > 1)
                        {
                            tmp.value = evt.previousValue;
                        }
                    }

                    //Check valid float
                    if (float.TryParse(tmp.value, out n))
                    {
                        //Quantity type is an Int
                        if (intType)
                        {
                            n = (float)Math.Floor(n);
                            tmp.value = n.ToString();
                        }

                        totaltmp -= (int)(QuantityMap[num] * item.PriceOf());
                        if (n > maxQuantity)
                        {
                            QuantityMap[num] = maxQuantity;
                            tmp.value = maxQuantity.ToString();
                        }
                        //Quantity entered less than min
                        else if (n < 0)
                        {
                            QuantityMap[num] = 0;
                            tmp.value = "0";
                        }
                        // In between min and max
                        else
                        {
                            QuantityMap[num] = n;
                        }

                        totaltmp += (int)(QuantityMap[num] * item.PriceOf());
                    }
                    //Not valid float then reset to 0
                    else
                    {
                        Debug.Log("total = " + QuantityMap[num]);
                        totaltmp -= (int)(QuantityMap[num] * item.PriceOf());
                        Debug.Log("total = " + totaltmp);
                        QuantityMap[num] = 0;
                        if (!tmp.value.Equals(""))
                            tmp.value = "0";

                    }

                    //Dependent on if item is sellable or buy card then add
                    //to appropriate total
                    if (!BuyOrSell.Equals("S")) total = totaltmp;
                    else SellTotal = totaltmp;

                    UpdateMoneyBalance();
                });

                QuantityContainer.Add(Quantity);

                checkoutCard.Add(QuantityContainer);

                if (!BuyOrSell.Equals("S")) total += (int)(item.PriceOf() * QuantityMap[ListType[i]]);
                else SellTotal += (int)(item.PriceOf() * QuantityMap[ListType[i]]);
                //UpdateMoneyBalance();
            }

            else
            {
                if (!BuyOrSell.Equals("S")) total += (int)(item.PriceOf());
                else SellTotal += (int)(item.PriceOf());
            }
            CheckoutScrollView.Add(checkoutCard);
            UpdateMoneyBalance();
        }

    }

    void CheckoutItemWrapper()
    {
        checkoutItemsDisplay(PlantBuyList, "B", "P", Plants, PlantQuantity);
        checkoutItemsDisplay(ToolBuyList, "B", "T", Tools, ToolQuantity);
        checkoutItemsDisplay(LivestockBuyList, "B", "L", Animals, LiveStockQuantity);
        checkoutItemsDisplay(SellList, "S", "S", null, SellQuantity);

    }
    //Cancel icon pressed in checkout
    void CancelCheckoutItem(Button button, List<int> buyorSellList, List<MarketWrapper> typeOfItem, char type, Dictionary<int, float> QuantityMap)
    {
        int num = int.Parse(button.name.Substring(2));

        //If it already exists within previous display make sure to change the display
        //so it shows cancel status.
        if ((currentTab[0].Equals(type) && isBuy) || (button.name[0].Equals('S') && !isBuy))
        {
            Label Change = root.Q<Label>("Status" + button.name.Substring(2));
            Debug.Log(Change);
            Change.style.backgroundImage = X;
        }

        buyorSellList.Remove(num);

        //Remove the item's checkout box within the scrollview section
        VisualElement tmp = button.GetFirstAncestorOfType<VisualElement>();
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        CheckoutScrollView.Remove(tmp);

        //Remove the item from total
        if (type.Equals('S')) SellTotal -= (int)(market.Sellables[num].wrap.PriceOf() * QuantityMap[num]);
        else total -= (int)(typeOfItem[num].PriceOf() * QuantityMap[num]);

        QuantityMap.Remove(num);

        CheckNoItems();

    }
    void CancelCheckoutItem(EventBase obj)
    {
        var button = (Button)obj.target;

        Debug.Log(button.name);

        char purchaseType = button.name[0];
        char ItemType = button.name[1];


        if (purchaseType.Equals('B'))
        {
            if (ItemType.Equals('P')) CancelCheckoutItem(button, PlantBuyList, Plants, 'P', PlantQuantity);
            else if (ItemType.Equals('T')) CancelCheckoutItem(button, ToolBuyList, Tools, 'T', ToolQuantity);
            else CancelCheckoutItem(button, LivestockBuyList, Animals, 'L', LiveStockQuantity);
        }
        else
        {
            CancelCheckoutItem(button, SellList, null, 'S', SellQuantity);
        }


    }

    //Bought Items checkout
    void Checkout(List<int> BoughtList, List<MarketWrapper> BoughtCardInfo, Dictionary<int, float> QuantityMap)
    {
        foreach (int element in BoughtList)
            market.BuyItem(BoughtCardInfo[element], QuantityMap[element]);
    }
    void CheckoutLiveStock(List<int> BoughtList, List<MarketWrapper> BoughtCardInfo, Dictionary<int, float> QuantityMap)
    {
        foreach (int element in BoughtList)
        {
            int NumOfAnimalsBought = (int)QuantityMap[element];
            for(int i = 0; i< NumOfAnimalsBought;++i)
                shelters.PlaceAnimal(BoughtCardInfo[element].type);

            player.Debit(BoughtCardInfo[element].value * NumOfAnimalsBought);
        }
            //shelters.PlaceAnimal((int)QuantityMap[element]);
    }
    void checkoutSell()
    {
        for (int i = SellList.Count - 1; i >= 0; i--)
        {
            int element = SellList[i];
            market.SellItem(market.Sellables[element], SellQuantity[element]);
        }
    }

    void CheckoutOperation()
    {

        if (total > player.Balance())
        {
            root.Q<Label>("CheckoutMessage").text = "No Sufficient Funds";
            root.Q<Label>("CheckoutMessage").style.display = DisplayStyle.Flex;
        }
        else
        {
            root.Q<Label>("CheckoutMessage").style.display = DisplayStyle.None;


            Checkout(PlantBuyList, Plants, PlantQuantity);
            Checkout(ToolBuyList, Tools, ToolQuantity);

            CheckoutLiveStock(LivestockBuyList,Animals, LiveStockQuantity);
            checkoutSell();
            //addtoInventory(LivestockBuyList, Animals,LiveStockQuantity);

            UpdateMoneyBalance();

            mainUI.ShowOrHideVisualElements(ref SuccessPurchase, ref ItemsInCheckout);
            /*
            if (total != 0 || SellTotal != 0)
            {
                mainUI.ShowOrHideVisualElements(ref SuccessPurchase, ref ItemsInCheckout);
            }

            */

            clearAllLists();
        }
    }

    void clearAllLists()
    {
        PlantBuyList.Clear();
        ToolBuyList.Clear();
        LivestockBuyList.Clear();
        SellList.Clear();
        LiveStockQuantity.Clear();
        PlantQuantity.Clear();
        ToolQuantity.Clear();
        SellQuantity.Clear();

    }

    void BackButtonToFarm()
    {
        controls.SetActive(true);
        PhoneGameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void ClickedTabs(EventBase obj)
    {
        var button = (Button)obj.target;
        Debug.Log(button.name);

        if (!(currentTab.Equals(button.name)))
        {
            root.Q<Button>(currentTab).style.opacity = (StyleFloat).5;
            root.Q<Button>(button.name).style.opacity = 1;
            currentTab = button.name;

            if (currentTab.Equals("PlantTab")) DisplayCards("P");
            else if (currentTab.Equals("ToolTab")) DisplayCards("T");
            else DisplayCards("L");

        }
    }
}
