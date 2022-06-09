using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneButtonController : MonoBehaviour
{
    private VisualElement root;
    public Button Phone;
    public Button PhoneButton;

    private Button ShopButton;
    private Button InventoryButton;
    private Button TriviaButton;

    private Button FinancialsApp;
    private Button Rewind;
    private Button FastForward;
    private Button StartPause;

    [SerializeField]
    private GameObject ShopApp;

    [SerializeField]
    private GameObject TriviaWindow;

    [SerializeField]
    private GameObject PersonalInventoryApp;

    [SerializeField]
    private GameObject QuestionReader;

    private Label Time;
    private Label Date;
    private Label TimeMultiplier;

    [SerializeField]
    private DisplayTime DateModule;

    [SerializeField]
    private GameObject controls;

    private string[] AnswerLabels = new string[] { "FirstAnswer", "SecondAnswer", "ThirdAnswer", "FourthAnswer" };

    private bool UserAnsweredQuestion;

    private int day = 1;
    private int month = 4;
    private int year = 2022;
    string dayString = "01";
    string monthString = "04";
    string YearString = "2022";



    [SerializeField]
    private Account player;

    void OnEnable()
    {
        assignUItoVariables();
        root.Focus();
        assignButtonsToFunctions();
        Date.text = YearString + "-" + monthString + "-" + dayString;


    }
    public void incrementDay()
    {
        ++day;
        if (day < 10) dayString = "0" + day.ToString();
        else dayString = day.ToString();

        Date.text = YearString + "-" + monthString + "-" + dayString;
    }
    void assignUItoVariables()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        Phone = root.Q<Button>("RealPhoneButtonContainer");
        PhoneButton = root.Q<Button>("phoneButton");
        ShopButton = root.Q<Button>("ShopApp");
        InventoryButton = root.Q<Button>("InventoryButton");
        FinancialsApp = root.Q<Button>("FinancialsApp");
        Time = root.Q<Label>("Time");
        Date = root.Q<Label>("Date");
        TimeMultiplier = root.Q<Label>("Multiplier");
        Rewind = root.Q<Button>("BackTrack");
        FastForward = root.Q<Button>("FastForward");
        StartPause = root.Q<Button>("StartPause");
        TriviaButton = root.Q<Button>("TriviaButton");
        root.Q<Button>("BackButtonToApps").clicked += BackButtonToAppsPressed;

    }

    void assignButtonsToFunctions()
    {
        Phone.clicked += HidePhone;
        PhoneButton.clicked += ShowPhone;
        ShopButton.clicked += ShopButtonPressed;
        InventoryButton.clicked += InventoryButtonPressed;
        FinancialsApp.clicked += FinancialsAppPressed;
        Rewind.clicked += RewindPressed;
        FastForward.clicked += FastForwardPressed;
        StartPause.clicked += StartPausePressed;
        TriviaButton.clicked += TriviaButtonPressed;
        

    }
    private void Update()
    {
        Time.text = DateModule.TimeDisplay();
    }

    //When Phone is shown and user clicks something other than app
    // Then close the phone
    void HidePhone()
    {
        Phone.style.display = DisplayStyle.None;
    }

    //Phone Button Pressed, show phone
    public void ShowPhone()
    {
        Phone.style.display = DisplayStyle.Flex;
    }

    void TriviaButtonPressed()
    {
        TriviaWindow.SetActive(true);
        this.gameObject.SetActive(false);
        controls.SetActive(false);

    }
    
    void ShopButtonPressed()
    {

        Debug.Log(ShopApp);
        ShopApp.SetActive(true);
        this.gameObject.SetActive(false);
        controls.SetActive(false);
    }
    void InventoryButtonPressed()
    {
        PersonalInventoryApp.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void FinancialsAppPressed()
    {

        StyleSet(DisplayStyle.Flex, DisplayStyle.None);


        root.Q<Label>("AppsLabel").text = "Financials";
        root.Q<Label>("AccountingBalance").text = player.Balance().ToString();
    }

    //Set what to display within phone UI
    void StyleSet(DisplayStyle Financial, DisplayStyle ScrollContainer)
    {
        root.Q<ScrollView>("RealPhoneScrollView").style.display = ScrollContainer;

        root.Q<VisualElement>("BackButtonContainer").style.display = Financial;

        root.Q<VisualElement>("FinancialContent").style.display = Financial;


    }
    void BackButtonToAppsPressed()
    {
        //Display Apps, hide financials
        StyleSet(DisplayStyle.None, DisplayStyle.Flex);
        root.Q<Label>("AppsLabel").text = "Apps";
    }

    void RewindPressed()
    {
        Debug.Log("Rewind");
        DateModule.decrementMultiplier();
        switch (DateModule.currentSpeed) {
            case DisplayTime.GameSpeed.Normal:
                TimeMultiplier.text = ">";
                break;
            case DisplayTime.GameSpeed.Medium:
                TimeMultiplier.text = ">>";
                break;
            default:
                break;
        }
    }

    void FastForwardPressed()
    {
        Debug.Log("Fast Forward");
        DateModule.incrementMultiplier();
        switch (DateModule.currentSpeed) {
            case DisplayTime.GameSpeed.Medium:
                TimeMultiplier.text = ">>";
                break;
            case DisplayTime.GameSpeed.Fast:
                TimeMultiplier.text = ">>>";
                break;
            default:
                break;
        }
    }

    void StartPausePressed()
    {
        Debug.Log("Start/Pause");
        DateModule.resetMultiplier();
        TimeMultiplier.text = ">";
    }
}
