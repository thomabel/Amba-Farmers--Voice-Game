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
        if (closestObject != null && closestObject.tag.Equals("Plant"))
        {
            //Debug.Log(closestObject);
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.Flex;

            Plant plant = closestObject.GetComponent<Plant>();
            MarketWrapper objectWrapper = market.Comparator[closestObject.GetComponent<TypeLabel>().Type];

            Picture.style.backgroundImage = objectWrapper.picture;
            ObjectName.text = objectWrapper.display_name;

            DaysLeft.text = plant.DaysLeftTillHarvest().ToString();
            Status.text = convertStatusToString(plant.currentHealthStatus);

            if (Plant.growthStages.Harvest == plant.currentGrowthStage)
                Ready.text = "Yes";
            else Ready.text = "No";

            /*
            string healthStatus = plant.daysUntilMaturity.ToString();
            root.Q<Label>("ObjectName").text = closestObject.name +'\n' +
                "Days till \n Mature: " + healthStatus;
            */


        }

        else
        {
            root.Q<VisualElement>("HoverContainer").style.display = DisplayStyle.None;
        }

    }

    
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
