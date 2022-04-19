using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewShopController : MonoBehaviour
{
    public VisualElement ScrollViewSection;
    public Button CardButton;
    public VisualElement PictureContainer;
    public VisualElement InfoContainer;
    public VisualElement StatusContainer;
    public Label Picture;
    public Label Name;
    public Label Price;
    public Label inCartLabel;
    public Label StatusPicture;

    public Texture2D Checkmark;
    public Texture2D X;
    private VisualElement root;

    public Button checkout;

    public Button CheckoutBackButton;
    public Account player;

    public TextField quantityField;
    public Card[] PlantCards;
    public int total=0;

    //public ScriptableObject player = ScriptableObject.CreateInstance("Account");
    private List<int> buyList;

    // Start is called before the first frame update
    void OnEnable()
    {
        buyList = new List<int>();
        //PlantCards = Resources.LoadAll<Card>("Cards/Plant");
        root = GetComponent<UIDocument>().rootVisualElement;
        //quantityField = root.Q<TextField>("Quantity");
        //int n = 0;
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
        //Debug.Log(int.TryParse(quantityField.text, out n));
        //Debug.Log(n + 1);

        root.Q<Button>("CheckoutActualButton").clicked+=CheckoutOperation;

        ScrollViewSection = root.Q<VisualElement>("ScrollView");

        root.Q<Label>("MoneyLabel").text = player.Balance().ToString();
        for (int i = 0; i < PlantCards.Length; ++i)
        {
            CardButton = new Button();
            CardButton.name = i.ToString();
            CardButton.AddToClassList("CardButton");
            CardButton.clickable.clickedWithEventInfo += Pressed;

            PictureContainer = new VisualElement();
            PictureContainer.AddToClassList("PictureContainer");

            Picture = new Label();
            Picture.AddToClassList("Picture");
            Picture.style.backgroundImage = PlantCards[i].picture;
            PictureContainer.Add(Picture);

            InfoContainer = new VisualElement();
            InfoContainer.AddToClassList("InfoContainer");

            Name = new Label();
            Name.text = PlantCards[i].name;
            Name.AddToClassList("Name");

            Price = new Label();
            Price.text = "$" + PlantCards[i].cost.ToString();
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
            StatusPicture.style.backgroundImage = X;

            StatusContainer.Add(inCartLabel);
            StatusContainer.Add(StatusPicture);


            CardButton.Add(PictureContainer);
            CardButton.Add(InfoContainer);
            CardButton.Add(StatusContainer);


            ScrollViewSection.Add(CardButton);
            PlantCards[i].quantity = 1;

        }

        checkout = root.Q<Button>("CheckoutButton");
        checkout.clicked += CheckoutButtonPressed;

        CheckoutBackButton = root.Q<Button>("CheckoutBackButton");
        CheckoutBackButton.clicked += CheckoutBackButtonPressed;

    }
    void Update()
    {
        //root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Label>("MoneyLabel").text = player.Balance().ToString();
        root.Q<Label>("SubtotalLabel").text = total.ToString();
        //Debug.Log("hi");
    }


    void Pressed(EventBase obj)
    {
        var button = (Button)obj.target;
        Card tmp = ScriptableObject.CreateInstance<Card>();
        tmp.name = PlantCards[int.Parse(button.name)].name;
        Debug.Log("tmp = " + tmp.name);
        Debug.Log(PlantCards[int.Parse(button.name)].name);

        Label Status = root.Q<Label>("Status" + int.Parse(button.name));
        if (Status.style.backgroundImage == X)
        {
            Status.style.backgroundImage = Checkmark;
            buyList.Add(int.Parse(button.name));

        }
        else
        {
            Status.style.backgroundImage = X;
            buyList.Remove(int.Parse(button.name));
        }

    }

    void CheckNoItems()
    {
        if (buyList.Count == 0)
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

        Debug.Log(buyList.Count);
        total = 0;
        CheckoutItemWrapper();
        root.Q<Label>("SubtotalLabel").text = total.ToString();


    }

    //Back Button to go back to Main shop
    void CheckoutBackButtonPressed()
    {
        root.Q<VisualElement>("CheckoutPage").style.display = DisplayStyle.None;
        root.Q<VisualElement>("MainContainer").style.display = DisplayStyle.Flex;
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        Debug.Log(total);
        CheckoutScrollView.Clear();
    }

    void checkoutItemsDisplay(List<int> ListType, string BuyOrSell)
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
            RemoveItem.name = BuyOrSell + ListType[i];
            RemoveItem.clickable.clickedWithEventInfo += CancelCheckoutItem;
            checkoutCard.Add(RemoveItem);

            VisualElement CheckoutItemInfoContainer = new VisualElement();
            CheckoutItemInfoContainer.AddToClassList("CheckoutItemNameContainer");
            Label CheckoutItemInfoLabel = new Label();
            CheckoutItemInfoLabel.text = ChosenType + " " + PlantCards[ListType[i]].name +" "+ "$" + PlantCards[ListType[i]].cost.ToString();
            CheckoutItemInfoLabel.AddToClassList("CheckoutItemInfoLabel");
            CheckoutItemInfoContainer.Add(CheckoutItemInfoLabel);

            checkoutCard.Add(CheckoutItemInfoContainer);

            VisualElement QuantityContainer = new VisualElement();
            QuantityContainer.AddToClassList("QuantityContainer");

            TextField Quantity = new TextField();
            Quantity.AddToClassList("Quantity");
            Quantity.maxLength = 4;
            //Quantity.value = "1";
            Quantity.value = PlantCards[ListType[i]].quantity.ToString();
            Quantity.name = "Q"+ ListType[i];
            Quantity.RegisterValueChangedCallback((evt) => {
                TextField tmp = (TextField)evt.target;
                Debug.Log(tmp.value);
                VisualElement parent1 = tmp.GetFirstAncestorOfType<VisualElement>();
                VisualElement parent2 = parent1.GetFirstAncestorOfType<VisualElement>();
                Debug.Log(parent2.ElementAt(0).name[0]);
                int n = 0;
                Debug.Log(int.TryParse(tmp.value, out n));
                int num = int.Parse(tmp.name.Substring(1));

                if (int.TryParse(tmp.value, out n))
                {
                    
                    total -= (PlantCards[num].quantity * PlantCards[num].cost);
                    PlantCards[num].quantity = int.Parse(tmp.value);
                    total += (PlantCards[num].quantity * PlantCards[num].cost);
                }
                else
                {
                    
                    total -= (PlantCards[num].quantity * PlantCards[num].cost);
                    Debug.Log("QUANTITY = " + PlantCards[num].quantity.ToString());
                    PlantCards[num].quantity = 0;
                    Debug.Log("Total = " + total.ToString());
                    
                }
                //Debug.Log(tmp.GetFirstAncestorOfType<Button>().name);

            });

            QuantityContainer.Add(Quantity);

            checkoutCard.Add(QuantityContainer);
            CheckoutScrollView.Add(checkoutCard);
            total += PlantCards[ListType[i]].cost * PlantCards[ListType[i]].quantity;

        }

    }

    void CheckoutItemWrapper()
    {
        checkoutItemsDisplay(buyList, "B");
    }

    void CancelCheckoutItem(EventBase obj)
    {
        var button = (Button)obj.target;

        Debug.Log(button.name);


        int num = int.Parse(button.name.Substring(1));
        Label Change = root.Q<Label>("Status" + button.name.Substring(1));
        Change.style.backgroundImage = X;
        
        buyList.Remove(num);
        Debug.Log(button.GetFirstAncestorOfType<VisualElement>().name);
        VisualElement tmp = button.GetFirstAncestorOfType<VisualElement>();
        VisualElement CheckoutScrollView = root.Q<VisualElement>("CheckoutScrollViewList");
        CheckoutScrollView.Remove(tmp);

        total -= PlantCards[num].cost * PlantCards[num].quantity;
        root.Q<Label>("SubtotalLabel").text = total.ToString();
        PlantCards[num].quantity = 1;


        CheckNoItems();

        //Button tmp = root.Q<Button>("ScrollView");

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
        }
    }
}
