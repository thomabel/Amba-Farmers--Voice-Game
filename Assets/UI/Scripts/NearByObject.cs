using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NearByObject : MonoBehaviour
{
    VisualElement root;
    public StringVariable Name;

    [SerializeField]
    private Interact FindClosest;

    //Closest object to player
    private GameObject closestObject;

    [SerializeField]
    private Market market;

    private Label Picture;
    private Label ObjectName;

    private Label DaysLeft;
    private Label Status;
    private Label Ready;


    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        closestObject = null;
        AssignLabels();

    }
    
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        closestObject = null;
        AssignLabels();

    }

    void AssignLabels()
    {

        ObjectName = root.Q<Label>("ObjectName");
        Picture = root.Q<Label>("Picture");
        DaysLeft = root.Q<Label>("DaysLeft");
        Status = root.Q<Label>("Status");
        Ready = root.Q<Label>("Ready");

    }

    private void FixedUpdate()
    {
        //Debug.Log(FindClosest.get_closest());
        closestObject = FindClosest.get_closest();
        //If closest object is a plant then display the info bar
        if (closestObject != null && closestObject.tag.Equals("Plant"))
        {
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.Flex;

            Plant plant = closestObject.GetComponent<Plant>();
            MarketWrapper objectWrapper = market.Comparator[closestObject.GetComponent<TypeLabel>().Type];

            //Display Plant info to the popup bar

            Picture.style.backgroundImage = objectWrapper.picture;
            ObjectName.text = objectWrapper.display_name;

            DaysLeft.text = plant.DaysLeftTillHarvest().ToString();
            Status.text = convertStatusToString(plant.currentHealthStatus);

            if (Plant.growthStages.Harvest == plant.currentGrowthStage)
                Ready.text = "Yes";
            else Ready.text = "No";


        }

        else
        {
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.None;
        }

    }

    //Convert enum statuses to simple strings
    string convertStatusToString(Plant.healthStatus status)
    {
        if (Plant.healthStatus.Fine == status || Plant.healthStatus.Healthy == status ||
            Plant.healthStatus.Optimal == status)
        {
            return "Good";
        }
        else if (Plant.healthStatus.Dead == status) return "Dead";

        return "Not Good";


    }

    
}
