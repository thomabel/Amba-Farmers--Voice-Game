using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewShopController : MonoBehaviour
{
    private VisualElement ScrollViewSection;
    private Button CardButton;
    private VisualElement PictureContainer;
    private VisualElement InfoContainer;
    private VisualElement StatusContainer;
    private Label Picture;
    private Label Name;
    private Label Price;
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

    [SerializeField]
    private Card[] PlantCards;
    [SerializeField]
    private Card[] ToolCards;
    [SerializeField]
    private Card[] LivestockCards;

    [SerializeField]
    private Card[] SellCards;



    private int total=0;
    private int SellTotal = 0;

    [SerializeField]
    private GameObject PhoneGameObject;

    //public ScriptableObject player = ScriptableObject.CreateInstance("Account");
    private List<int> PlantBuyList;
    private List<int> ToolBuyList;
    private List<int> LivestockBuyList;

    private List<int> SellList;



    [SerializeField]
    private InventoryList Inventory;



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

        PlantBuyList = new List<int>();
        ToolBuyList = new List<int>();
        LivestockBuyList = new List<int>();
        SellList = new List<int>();

        //PlantCards = Resources.LoadAll<Card>("Cards/Plant");
        root = GetComponent<UIDocument>().rootVisualElement;
        root.Focus();
        MoneyLabel = root.Q<Label>("MoneyLabel");
        SubtotalLabel = root.Q<Label>("SubtotalLabel");
        SoldSubtotalLabel = root.Q<Label>("SoldSubtotalLabel");
        CheckoutMoneyLabel = root.Q<Label>("CheckoutPlayerBalance");

        SuccessPurchase = root.Q<VisualElement>("ItemsPurchased");
        ItemsInCheckout = root.Q<VisualElement>("ItemsListContainer");
        //quantityField = root.Q<TextField>("Quantity");
        //int n = 0;
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
        //Debug.Log(int.TryParse(quantityField.text, out n));
        //Debug.Log(n + 1);
        currentTab = "ToolTab";

        root.Q<Button>("ToolTab").style.opacity = 1;
        root.Q<Button>("ToolTab").clickable.clickedWithEventInfo += ClickedTabs;

        root.Q<Button>("PlantTab").clickable.clickedWithEventInfo += ClickedTabs;

        root.Q<Button>("LivestockTab").clickable.clickedWithEventInfo += ClickedTabs;


        sellButton = root.Q<Button>("SellButton");
        sellButton.clicked += SellButtonPressed;

        buyButton = root.Q<Button>("BuyButton");
        buyButton.clicked += BuyButtonPressed;

        //Setting up clickedTabs

        root.Q<Button>("CheckoutActualButton").clicked+=CheckoutOperation;

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

    public void roothide()
    {
        root.style.display = DisplayStyle.None;
    }
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
        DisplayCards(SellCards, SellList);
        isBuy = false;

        sellButton.RemoveFromClassList("ButtonUnActive");
        buyButton.AddToClassList("ButtonUnActive");
    }
    void DisplayCards(string ItemsToDisplay)
    {
        ScrollViewSection.Clear();
        if (ItemsToDisplay.Equals("P")) DisplayCards(PlantCards, PlantBuyList);
        else if (ItemsToDisplay.Equals("T")) DisplayCards(ToolCards, ToolBuyList);
        else if(ItemsToDisplay.Equals("L")) DisplayCards(LivestockCards, LivestockBuyList);
        else DisplayCards(SellCards, SellList);

    }
    void DisplayCards(Card [] ItemCards, List<int> BuyOrSellList)
    {
        for (int i = 0; i < ItemCards.Length; ++i)
        {
            CardButton = new Button();
            CardButton.name = i.ToString();
            CardButton.AddToClassList("CardButton");
            CardButton.clickable.clickedWithEventInfo += Pressed;

            PictureContainer = new VisualElement();
            PictureContainer.AddToClassList("PictureContainer");

            Picture = new Label();
            Picture.AddToClassList("Picture");
            Picture.style.backgroundImage = ItemCards[i].picture;
            PictureContainer.Add(Picture);

            InfoContainer = new VisualElement();
            InfoContainer.AddToClassList("InfoContainer");

            Name = new Label();
            Name.text = ItemCards[i].name;
            Name.AddToClassList("Name");

            Price = new Label();
            Price.text = "$" + ItemCards[i].cost.ToString();
            Price.AddToClassList("Price");

            InfoContainer.Add(Name);
            InfoContainer.Add(Price);

            StatusContainer = new VisualElement();
            StatusContainer.AddToClassList("Status");

            inCartLabel = new Label();
            inCartLabel.text = "In Cart";
            inCartLabel.AddToClassList("InCartLabel");

            StatusPicture = new Label();
            StatusPicture.name = "Status" + i.ToString();
            StatusPicture.AddToClassList("StatusPicture");
            if(BuyOrSellList.Contains(i))
                StatusPicture.style.backgroundImage = Checkmark;
            else StatusPicture.style.backgroundImage = X;

            StatusContainer.Add(inCartLabel);
            StatusContainer.Add(StatusPicture);


            CardButton.Add(PictureContainer);
            CardButton.Add(InfoContainer);
            CardButton.Add(StatusContainer);


            ScrollViewSection.Add(CardButton);
            ItemCards[i].quantity = 1;

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
        //Card tmp = ScriptableObject.CreateInstance<Card>();
        //tmp.name = PlantCards[int.Parse(button.name)].name;
        //Debug.Log("tmp = " + tmp.name);

        //Debug.Log(PlantCards[int.Parse(button.name)].name);
        /*
        if (currentTab.Equals("PlantTab")) Debug.Log(PlantCards[int.Parse(button.name)].name);
        else if (currentTab.Equals("ToolTab")) Debug.Log(ToolCards[int.Parse(button.name)].name);
        else Debug.Log(LivestockCards[int.Parse(button.name)].name);
        */
        if (!isBuy)
        {
            Pressed(SellList, button.name);
            return;
        }

        if (currentTab.Equals("PlantTab")) Pressed(PlantBuyList,button.name);
        else if (currentTab.Equals("ToolTab")) Pressed(ToolBuyList, button.name);
        else Pressed(LivestockBuyList, button.name);    

    }

    void Pressed(List<int> ItemType, string itemName)
    {
        Label Status = root.Q<Label>("Status" + int.Parse(itemName));
        if (Status.style.backgroundImage == X)
        {
            Status.style.backgroundImage = Checkmark;
            ItemType.Add(int.Parse(itemName));

        }
        else
        {
            Status.style.backgroundImage = X;
            ItemType.Remove(int.Parse(itemName));
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

    void checkoutItemsDisplay(List<int> ListType, string BuyOrSell, string typeofItem, Card[] items )
    {
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        string ChosenType = "";
        if (BuyOrSell.Equals("S")) ChosenType = "Selling";
        else ChosenType = "Buying";
        for (int i = 0; i < ListType.Count; ++i)
        {
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
            CheckoutItemInfoLabel.text = ChosenType + " " + items[ListType[i]].name +" "+ "$" + items[ListType[i]].cost.ToString();
            CheckoutItemInfoLabel.AddToClassList("CheckoutItemInfoLabel");
            CheckoutItemInfoContainer.Add(CheckoutItemInfoLabel);

            checkoutCard.Add(CheckoutItemInfoContainer);

            VisualElement QuantityContainer = new VisualElement();
            QuantityContainer.AddToClassList("QuantityContainer");

            TextField Quantity = new TextField();
            Quantity.AddToClassList("Quantity");
            Quantity.maxLength = 4;
            //Quantity.value = "1";
            Quantity.value = items[ListType[i]].quantity.ToString();
            Quantity.name = "Q"+ typeofItem + ListType[i];
            Quantity.RegisterValueChangedCallback((evt) => {

                int totaltmp = 0;
                if (!BuyOrSell.Equals("S")) totaltmp = total;
                else totaltmp = SellTotal;

                TextField tmp = (TextField)evt.target;
                Debug.Log(tmp.value);
                VisualElement parent1 = tmp.GetFirstAncestorOfType<VisualElement>();
                VisualElement parent2 = parent1.GetFirstAncestorOfType<VisualElement>();
                Debug.Log(parent2.ElementAt(0).name[0]);
                int n = 0;
                Debug.Log(int.TryParse(tmp.value, out n));
                Debug.Log("LENGTH = " + items.Length.ToString());
                int num = int.Parse(tmp.name.Substring(2));

                if (int.TryParse(tmp.value, out n))
                {
                    
                    totaltmp -= (items[num].quantity * items[num].cost);
                    items[num].quantity = int.Parse(tmp.value);
                    totaltmp += (items[num].quantity * items[num].cost);
                }
                else
                {
                    
                    totaltmp -= (items[num].quantity * items[num].cost);
                    Debug.Log("QUANTITY = " + items[num].quantity.ToString());
                    items[num].quantity = 0;
                    if(!tmp.value.Equals(""))
                        tmp.value = "0";
                    Debug.Log("Total = " + total.ToString());
                    
                }
                //Debug.Log(tmp.GetFirstAncestorOfType<Button>().name);
                if (!BuyOrSell.Equals("S")) total = totaltmp;
                else SellTotal = totaltmp;
            });

            QuantityContainer.Add(Quantity);

            checkoutCard.Add(QuantityContainer);
            CheckoutScrollView.Add(checkoutCard);

            if (!BuyOrSell.Equals("S")) total += items[ListType[i]].cost * items[ListType[i]].quantity;
            else SellTotal += items[ListType[i]].cost * items[ListType[i]].quantity;
            //total += items[ListType[i]].cost * items[ListType[i]].quantity;

        }

    }

    void CheckoutItemWrapper()
    {
        checkoutItemsDisplay(PlantBuyList, "B", "P",PlantCards);
        checkoutItemsDisplay(ToolBuyList, "B", "T", ToolCards);
        checkoutItemsDisplay(LivestockBuyList, "B", "L", LivestockCards);
        checkoutItemsDisplay(SellList, "S", "S", SellCards);

    }

    void CancelCheckoutItem(Button button, List<int> buyorSellList, Card[] typeOfItem, char type )
    {
        int num = int.Parse(button.name.Substring(2));

        if (currentTab[0].Equals(type) || type.Equals('S')){
            Label Change = root.Q<Label>("Status" + button.name.Substring(2));
            Change.style.backgroundImage = X;
        }

        buyorSellList.Remove(num);
        Debug.Log(button.GetFirstAncestorOfType<VisualElement>().name);
        VisualElement tmp = button.GetFirstAncestorOfType<VisualElement>();
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        CheckoutScrollView.Remove(tmp);

        if (type.Equals('S')) SellTotal -= typeOfItem[num].cost * typeOfItem[num].quantity;
        else total -= typeOfItem[num].cost * typeOfItem[num].quantity;

        //root.Q<Label>("SubtotalLabel").text = total.ToString();
        typeOfItem[num].quantity = 1;

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
            if (ItemType.Equals('P')) CancelCheckoutItem(button, PlantBuyList, PlantCards, 'P');
            else if (ItemType.Equals('T')) CancelCheckoutItem(button, ToolBuyList, ToolCards,'T');
            else CancelCheckoutItem(button, LivestockBuyList, LivestockCards, 'L');
        }
        else
        {
            CancelCheckoutItem(button, SellList, SellCards, 'S');
        }
        /*
        else
        {

            if (ItemType.Equals('P')) CancelCheckoutItem(button, PlantBuyList, PlantCards);
            else if (ItemType.Equals('T')) CancelCheckoutItem(button, ToolBuyList, ToolCards);
            else CancelCheckoutItem(button, LivestockBuyList, LivestockCards);
        }
        */

        //int num = int.Parse(button.name.Substring(2));
        /*
        Label Change = root.Q<Label>("Status" + button.name.Substring(1));
        Change.style.backgroundImage = X;

        PlantBuyList.Remove(num);
        Debug.Log(button.GetFirstAncestorOfType<VisualElement>().name);
        VisualElement tmp = button.GetFirstAncestorOfType<VisualElement>();
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        CheckoutScrollView.Remove(tmp);

        total -= PlantCards[num].cost * PlantCards[num].quantity;
        root.Q<Label>("SubtotalLabel").text = total.ToString();
        PlantCards[num].quantity = 1;
        */


        //CheckNoItems();

        //Button tmp = root.Q<Button>("ScrollView");

    }
    void addtoInventory(List<int> BoughtList, Card[] BoughtCardInfo )
    {
        for(int i =0; i< BoughtList.Count; ++i)
        {
            if (Inventory.CardExists(BoughtCardInfo[BoughtList[i]]))
            {
                BoughtCardInfo[BoughtList[i]].InventoryQuantity += BoughtCardInfo[BoughtList[i]].quantity;

            }
            else
            {
                if (BoughtCardInfo[BoughtList[i]].quantity != 0)
                {
                    Inventory.Add(BoughtCardInfo[BoughtList[i]]);
                    BoughtCardInfo[BoughtList[i]].InventoryQuantity += BoughtCardInfo[BoughtList[i]].quantity;

                }
            }
            
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
            player.Debit(total);
            player.Credit(SellTotal);

            addtoInventory(PlantBuyList, PlantCards);
            addtoInventory(ToolBuyList, ToolCards);
            addtoInventory(LivestockBuyList, LivestockCards);

            if (total != 0 || SellTotal != 0)
            {
                ItemsInCheckout.style.display = DisplayStyle.None;
                SuccessPurchase.style.display = DisplayStyle.Flex;
            }

            PlantBuyList.Clear();
            ToolBuyList.Clear();
            LivestockBuyList.Clear();
            SellList.Clear();


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

        if(!(currentTab.Equals(button.name))) {
            root.Q<Button>(currentTab).style.opacity = (StyleFloat).5;
            root.Q<Button>(button.name).style.opacity = 1;
            currentTab = button.name;

            if (currentTab.Equals("PlantTab")) DisplayCards("P");
            else if(currentTab.Equals("ToolTab")) DisplayCards("T");
            else DisplayCards("L");

        }
    }
}
