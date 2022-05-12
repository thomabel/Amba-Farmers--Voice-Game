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

    private Button FinancialsApp;
    private Button Rewind;
    private Button FastForward;
    private Button StartPause;

    [SerializeField]
    private GameObject ShopApp;

    [SerializeField]
    private GameObject PersonalInventoryApp;

    private Label Time;
    private Label TimeMultiplier;

    [SerializeField]
    private DisplayTime DateModule;

    [SerializeField]
    private GameObject controls;


    [SerializeField]
    private Account player;
    // Start is called before the first frame update
    void OnEnable()
    {
        assignUItoVariables();
        root.Focus();
        assignButtonsToFunctions();
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
        TimeMultiplier = root.Q<Label>("Multiplier");
        Rewind = root.Q<Button>("BackTrack");
        FastForward = root.Q<Button>("FastForward");
        StartPause = root.Q<Button>("StartPause");
    }

    void assignButtonsToFunctions()
    {
        Phone.clicked += HidePhone;
        PhoneButton.clicked += ShowPhone;
        ShopButton.clicked += ShopButtonPressed;
        InventoryButton.clicked += InventoryButtonPressed;
        FinancialsApp.clicked += FinancialsAppPressed;
        root.Q<Button>("BackButtonToApps").clicked += BackButtonToAppsPressed;
        Rewind.clicked += RewindPressed;
        FastForward.clicked += FastForwardPressed;
        StartPause.clicked += StartPausePressed;

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
    void ShowPhone()
    {
        Phone.style.display = DisplayStyle.Flex;
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

    void BackButtonToAppsPressed()
    {
        StyleSet(DisplayStyle.None, DisplayStyle.Flex);
        root.Q<Label>("AppsLabel").text = "Apps";
    }
    void FinancialsAppPressed()
    {

        StyleSet(DisplayStyle.Flex, DisplayStyle.None);


        root.Q<Label>("AppsLabel").text = "Financials";
        root.Q<Label>("AccountingBalance").text = player.Balance().ToString();
    }
    void StyleSet(DisplayStyle Financial, DisplayStyle ScrollContainer)
    {
        root.Q<ScrollView>("RealPhoneScrollView").style.display = ScrollContainer;

        root.Q<VisualElement>("BackButtonContainer").style.display = Financial;

        root.Q<VisualElement>("FinancialContent").style.display = Financial;

        //AppScrollView.style.display = ScrollContainer;

    }

    void RewindPressed()
    {
        Debug.Log("Rewind");
        DateModule.decrementMultiplier();
        TimeMultiplier.text = DateModule.timeMultiplier.ToString() + "x";
    }

    void FastForwardPressed()
    {
        Debug.Log("Fast Forward");
        DateModule.incrementMultiplier();
        TimeMultiplier.text = DateModule.timeMultiplier.ToString() +"x";
    }

    void StartPausePressed()
    {
        Debug.Log("Start/Pause");
        DateModule.resetMultiplier();
        TimeMultiplier.text = DateModule.timeMultiplier.ToString() + "x";
    }
}
