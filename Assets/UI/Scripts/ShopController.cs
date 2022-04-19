
using UnityEngine;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{

    public VisualElement ScrollViewSection;
    public VisualElement CardBody;
    public Button PriceButton;
    public Label Picture;
    public Label Name;
    private VisualElement root;

    public Card[] PlantCards;
    // Start is called before the first frame update
    void Start()
    {
        //PlantCards = Resources.LoadAll<Card>("Cards/Plant");
        root = GetComponent<UIDocument>().rootVisualElement;

        ScrollViewSection = root.Q<VisualElement>("ScrollView");

        for (int i = 0; i < PlantCards.Length; ++i)
        {
            CardBody = new VisualElement();
            CardBody.AddToClassList("Card");

            PriceButton = new Button();
            PriceButton.name = PlantCards[i].name;
            PriceButton.text = "$"+PlantCards[i].cost.ToString();
            //PriceButton.clickable.clickedWithEventInfo += Pressed;

            PriceButton.AddToClassList("Price");
            Name = new Label();
            Name.text = PlantCards[i].name;
            Name.AddToClassList("Name");

            Picture = new Label();
            Picture.AddToClassList("Picture");
            Picture.style.backgroundImage = PlantCards[i].picture;
            Name.AddToClassList("Name");

            CardBody.Add(Name);
            CardBody.Add(Picture);
            CardBody.Add(PriceButton);

            //if (PlantCards[i].name.Equals("Banana"))
            //{
            //    CardBody.style.display = DisplayStyle.None;
            //}


            ScrollViewSection.Add(CardBody);

            //Debug.Log(PlantCards[0]);
        }

        //StartButton.clicked += StartButtonPressed;


    }

    /*
    void Pressed(EventBase obj)
    {
        //var button = (Button)obj.target;

    }
    */
    
}
