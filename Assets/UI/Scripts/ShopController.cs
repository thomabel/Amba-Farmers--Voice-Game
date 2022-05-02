using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
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

    //public ScriptableObject player = ScriptableObject.CreateInstance("Account");
    private List<int> PlantBuyList;
    private List<int> ToolBuyList;
    private List<int> LivestockBuyList;

    private List<int> SellList;


    private Dictionary<int, int> PlantQuantity;
    private Dictionary<int, int> ToolQuantity;
    private Dictionary<int, int> LiveStockQuantity;
    private Dictionary<int, int> SellQuantity;

    [SerializeField]
    Market market;

    private List<MarketWrapper> Plants;
    private List<MarketWrapper> Animals;
    private List<MarketWrapper> Tools;


    [SerializeField]
    private InventoryList Inventory;

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



    // Start is called before the first frame update
    void OnEnable()
    {

        PlantQuantity = new Dictionary<int, int>();
        ToolQuantity = new Dictionary<int, int>();
        LiveStockQuantity = new Dictionary<int, int>();
        SellQuantity = new Dictionary<int, int>();

        PlantBuyList = new List<int>();
        ToolBuyList = new List<int>();
        LivestockBuyList = new List<int>();
        SellList = new List<int>();

        Plants = new List<MarketWrapper>();
        Animals = new List<MarketWrapper>();
        Tools = new List<MarketWrapper>();

        AddToLists();

        root = GetComponent<UIDocument>().rootVisualElement;
        root.Focus();


        MoneyLabel = root.Q<Label>("MoneyLabel");
        SubtotalLabel = root.Q<Label>("SubtotalLabel");
        SoldSubtotalLabel = root.Q<Label>("SoldSubtotalLabel");
        CheckoutMoneyLabel = root.Q<Label>("CheckoutPlayerBalance");

        SuccessPurchase = root.Q<VisualElement>("ItemsPurchased");
        ItemsInCheckout = root.Q<VisualElement>("ItemsListContainer");

        currentTab = "ToolTab";

        root.Q<Button>("ToolTab").style.opacity = 1;
        root.Q<Button>("ToolTab").clickable.clickedWithEventInfo += ClickedTabs;

        root.Q<Button>("PlantTab").clickable.clickedWithEventInfo += ClickedTabs;

        root.Q<Button>("LivestockTab").clickable.clickedWithEventInfo += ClickedTabs;


        sellButton = root.Q<Button>("SellButton");
        sellButton.name = "Sell";
        //sellButton.clickable.clickedWithEventInfo += BuyOrSellTabClicked;
        sellButton.clicked += SellButtonPressed;

        buyButton = root.Q<Button>("BuyButton");
        buyButton.name = "Buy";
        //buyButton.clickable.clickedWithEventInfo += BuyOrSellTabClicked;

        buyButton.clicked += BuyButtonPressed;

        //Setting up clickedTabs

        root.Q<Button>("CheckoutActualButton").clicked += CheckoutOperation;

        root.Q<Button>("BackButtonToFarm").clicked += BackButtonToFarm;

        ScrollViewSection = root.Q<VisualElement>("ScrollView");

        root.Q<Label>("MoneyLabel").text = player.Balance().ToString();

        DisplayCards("T");
        isBuy = true;
        checkout = root.Q<Button>("CheckoutButton");
        checkout.clicked += CheckoutButtonPressed;

        CheckoutBackButton = root.Q<Button>("CheckoutBackButton");
        CheckoutBackButton.clicked += CheckoutBackButtonPressed;

    }
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
        if(market.Sellables.Count == 0)
            market.PopulateSellables();

        Debug.Log(market.Sellables.Count);
    }
    /*
    void BuyOrSellTabClicked(EventBase BuyOrSellModeClicked)
    {
        var button = (Button)BuyOrSellModeClicked.target;

        if (isBuy)
        {
            root.Q<VisualElement>("TabButtonContainer").style.visibility = Visibility.Hidden;
            DisplayCards("S");
            isBuy = false;
        }
        else
        {

        }

    }
    */

    void BuyButtonPressed()
    {
        if (isBuy) return;

        root.Q<VisualElement>("TabButtonContainer").style.visibility = Visibility.Visible;
        isBuy = true;
        DisplayCards(currentTab[0].ToString());

        buyButton.RemoveFromClassList("ButtonUnActive");
        sellButton.AddToClassList("ButtonUnActive");

    }
    void SellButtonPressed()
    {
        if (!isBuy) return;

        ScrollViewSection.Clear();
        root.Q<VisualElement>("TabButtonContainer").style.visibility = Visibility.Hidden;
        DisplayCards(null, SellList, false);
        isBuy = false;

        sellButton.RemoveFromClassList("ButtonUnActive");
        buyButton.AddToClassList("ButtonUnActive");
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
        else length = market.Sellables.Count;


        for (int i = 0; i < length; ++i)
        {
            MarketWrapper item = null;
            if (IsBuyMode) item = ItemCards[i];
            else
            {
                item = market.Sellables[i].wrap;
                InventoryName = new Label();
                InventoryName.text = market.Sellables[i].inv.gameObject.name + "\n" + "Inventory";
                InventoryName.AddToClassList("InventoryName");
            }

            CardButton = new Button();
            CardButton.name = i.ToString();
            CardButton.AddToClassList("CardButton");
            CardButton.clickable.clickedWithEventInfo += Pressed;

            PictureContainer = new VisualElement();
            PictureContainer.AddToClassList("PictureContainer");

            Picture = new Label();
            Picture.AddToClassList("Picture");
            Picture.style.backgroundImage = item.picture;
            PictureContainer.Add(Picture);

            InfoContainer = new VisualElement();
            InfoContainer.AddToClassList("InfoContainer");

            Name = new Label();
            Name.text = item.display_name;
            Name.AddToClassList("Name");

            Price = new Label();
            Price.text = "$" + item.PriceOf().ToString();
            Price.AddToClassList("Price");

            InfoContainer.Add(Name);
            InfoContainer.Add(Price);
            if(!IsBuyMode) InfoContainer.Add(InventoryName);

            StatusContainer = new VisualElement();
            StatusContainer.AddToClassList("Status");

            inCartLabel = new Label();
            inCartLabel.text = "In Cart";
            inCartLabel.AddToClassList("InCartLabel");

            StatusPicture = new Label();
            StatusPicture.name = "Status" + i.ToString();
            StatusPicture.AddToClassList("StatusPicture");

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
   
    void Update()
    {
        //root = GetComponent<UIDocument>().rootVisualElement;
        MoneyLabel.text = player.Balance().ToString();
        SubtotalLabel.text = total.ToString();
        SoldSubtotalLabel.text = SellTotal.ToString();
        CheckoutMoneyLabel.text = player.Balance().ToString();
    }


    void Pressed(EventBase obj)
    {
        var button = (Button)obj.target;

        if (!isBuy)
        {
            Pressed(SellList, button.name, SellQuantity);
            return;
        }

        if (currentTab.Equals("PlantTab")) Pressed(PlantBuyList, button.name, PlantQuantity);
        else if (currentTab.Equals("ToolTab")) Pressed(ToolBuyList, button.name, ToolQuantity);
        else Pressed(LivestockBuyList, button.name, LiveStockQuantity);

    }

    void Pressed(List<int> ItemType, string itemName, Dictionary<int, int> QuantityMap)
    {
        Label Status = root.Q<Label>("Status" + int.Parse(itemName));
        if (Status.style.backgroundImage == X)
        {
            Status.style.backgroundImage = Checkmark;
            ItemType.Add(int.Parse(itemName));
            QuantityMap.Add(int.Parse(itemName), 1);
        }
        else
        {
            Status.style.backgroundImage = X;
            ItemType.Remove(int.Parse(itemName));
            QuantityMap.Remove(int.Parse(itemName));
        }
    }

    void CheckNoItems()
    {
        if (PlantBuyList.Count == 0 && ToolBuyList.Count == 0
            && LivestockBuyList.Count == 0 && SellList.Count == 0)
        {
            root.Q<VisualElement>("CheckoutContent").style.display = DisplayStyle.None;
            root.Q<VisualElement>("NoItems").style.display = DisplayStyle.Flex;

        }
        else
        {
            root.Q<VisualElement>("NoItems").style.display = DisplayStyle.None;
            root.Q<VisualElement>("CheckoutContent").style.display = DisplayStyle.Flex;

        }
    }
    //Button to go to checkout
    void CheckoutButtonPressed()
    {
        root.Q<VisualElement>("MainContainer").style.display = DisplayStyle.None;
        root.Q<VisualElement>("CheckoutPage").style.display = DisplayStyle.Flex;

        CheckNoItems();

        Debug.Log(PlantBuyList.Count);
        total = 0;
        SellTotal = 0;
        CheckoutItemWrapper();
        SubtotalLabel.text = total.ToString();
        SoldSubtotalLabel.text = SellTotal.ToString();


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
        ItemsInCheckout.style.display = DisplayStyle.Flex;
        SuccessPurchase.style.display = DisplayStyle.None;


    }

    void checkoutItemsDisplay(List<int> ListType, string BuyOrSell, string typeofItem, List<MarketWrapper> items, Dictionary<int, int> QuantityMap)
    {
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
            else item = market.Sellables[i].wrap;

            CheckoutScrollView.AddToClassList("CheckoutScrollViewList");

            VisualElement checkoutCard = new VisualElement();
            checkoutCard.AddToClassList("CheckoutCard");

            Button RemoveItem = new Button();
            RemoveItem.text = "X";
            RemoveItem.AddToClassList("RemoveItemButton");
            RemoveItem.name = BuyOrSell + typeofItem + ListType[i];
            RemoveItem.clickable.clickedWithEventInfo += CancelCheckoutItem;
            checkoutCard.Add(RemoveItem);

            VisualElement CheckoutItemInfoContainer = new VisualElement();
            CheckoutItemInfoContainer.AddToClassList("CheckoutItemNameContainer");
            Label CheckoutItemInfoLabel = new Label();
            CheckoutItemInfoLabel.text = ChosenType + " " + item.display_name + " " + "$" + item.PriceOf().ToString();
            CheckoutItemInfoLabel.AddToClassList("CheckoutItemInfoLabel");
            CheckoutItemInfoContainer.Add(CheckoutItemInfoLabel);

            checkoutCard.Add(CheckoutItemInfoContainer);

            VisualElement QuantityContainer = new VisualElement();
            QuantityContainer.AddToClassList("QuantityContainer");

            TextField Quantity = new TextField();
            Quantity.AddToClassList("Quantity");
            Quantity.maxLength = 4;
            //Quantity.value = "1";
            Quantity.value = QuantityMap[ListType[i]].ToString();
            Quantity.name = "Q" + typeofItem + ListType[i];
            Quantity.RegisterValueChangedCallback((evt) => {

                int totaltmp = 0;
                if (!BuyOrSell.Equals("S")) totaltmp = total;
                else totaltmp = SellTotal;

                TextField tmp = (TextField)evt.target;

                VisualElement parent1 = tmp.GetFirstAncestorOfType<VisualElement>();
                VisualElement parent2 = parent1.GetFirstAncestorOfType<VisualElement>();

                int n = 0;

                int num = int.Parse(tmp.name.Substring(2));

                if (int.TryParse(tmp.value, out n))
                {
                    totaltmp -= (QuantityMap[num] * item.PriceOf());
                    QuantityMap[num] = int.Parse(tmp.value);
                    totaltmp += (QuantityMap[num] * item.PriceOf());

                }
                else
                {
                    totaltmp -= (QuantityMap[num] * item.PriceOf());
                    QuantityMap[num] = 0;
                    if (!tmp.value.Equals(""))
                        tmp.value = "0";


                }

                if (!BuyOrSell.Equals("S")) total = totaltmp;
                else SellTotal = totaltmp;
            });

            QuantityContainer.Add(Quantity);

            checkoutCard.Add(QuantityContainer);
            CheckoutScrollView.Add(checkoutCard);

            if (!BuyOrSell.Equals("S")) total += item.PriceOf() * QuantityMap[ListType[i]];
            else SellTotal += item.PriceOf() * QuantityMap[ListType[i]];

        }

    }
    
    void CheckoutItemWrapper()
    {
        checkoutItemsDisplay(PlantBuyList, "B", "P", Plants, PlantQuantity);
        checkoutItemsDisplay(ToolBuyList, "B", "T", Tools, ToolQuantity);
        checkoutItemsDisplay(LivestockBuyList, "B", "L", Animals, LiveStockQuantity);
        checkoutItemsDisplay(SellList, "S", "S", null, SellQuantity);

    }

    void CancelCheckoutItem(Button button, List<int> buyorSellList, List<MarketWrapper> typeOfItem, char type, Dictionary<int, int> QuantityMap)
    {
        int num = int.Parse(button.name.Substring(2));

        Debug.Log("current tab = " + currentTab[0]);
        Debug.Log("type = " + type);

        if ((currentTab[0].Equals(type) && isBuy) || (button.name[0].Equals('S') && !isBuy))
        {
            Label Change = root.Q<Label>("Status" + button.name.Substring(2));
            Debug.Log(Change);
            Change.style.backgroundImage = X;
        }

        buyorSellList.Remove(num);
        Debug.Log(button.GetFirstAncestorOfType<VisualElement>().name);
        VisualElement tmp = button.GetFirstAncestorOfType<VisualElement>();
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        CheckoutScrollView.Remove(tmp);

        if (type.Equals('S')) SellTotal -= market.Sellables[num].wrap.PriceOf() * QuantityMap[num];
        else total -= typeOfItem[num].PriceOf() * QuantityMap[num];

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


    void Checkout(List<int> BoughtList, List<MarketWrapper> BoughtCardInfo, Dictionary<int, int> QuantityMap, int StartInventory)
    {
        foreach (int element in BoughtList)
            market.BuyItem(BoughtCardInfo[element], QuantityMap[element]);

    }

    void checkoutSell()
    {
        foreach (int element in SellList)
            market.SellItem(market.Sellables[element], SellQuantity[element]);
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


            Checkout(PlantBuyList, Plants, PlantQuantity, 0);
            Checkout(ToolBuyList, Tools, ToolQuantity, 1);
            checkoutSell();
            //addtoInventory(LivestockBuyList, Animals,LiveStockQuantity);

            if (total != 0 || SellTotal != 0)
            {
                ItemsInCheckout.style.display = DisplayStyle.None;
                SuccessPurchase.style.display = DisplayStyle.Flex;
            }

            PlantBuyList.Clear();
            ToolBuyList.Clear();
            LivestockBuyList.Clear();
            SellList.Clear();

            LiveStockQuantity.Clear();
            PlantQuantity.Clear();
            ToolQuantity.Clear();
            SellQuantity.Clear();


        }
    }

    void BackButtonToFarm()
    {
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
