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

    private Button FinancialsApp;
    private Button Rewind;
    private Button FastForward;
    private Button StartPause;

    public GameObject ShopApp;

    private Label Time;
    private Label TimeMultiplier;

    [SerializeField]
    private DisplayTime DateModule;


    [SerializeField]
    private Account player;
    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.Focus();

        Phone = root.Q<Button>("RealPhoneButtonContainer");

        Phone.clicked += Pressed;

        PhoneButton = root.Q<Button>("phoneButton");
        PhoneButton.clicked += Pressed2;

        ShopButton = root.Q<Button>("ShopApp");
        ShopButton.clicked += ShopButtonPressed;

        FinancialsApp = root.Q<Button>("FinancialsApp");
        FinancialsApp.clicked += FinancialsAppPressed;

        root.Q<Button>("BackButtonToApps").clicked += BackButtonToAppsPressed;

        Time = root.Q<Label>("Time");
        TimeMultiplier = root.Q<Label>("Multiplier");
        Rewind = root.Q<Button>("BackTrack");
        Rewind.clicked += RewindPressed;
        FastForward = root.Q<Button>("FastForward");
        FastForward.clicked += FastForwardPressed;
        StartPause = root.Q<Button>("StartPause");
        StartPause.clicked += StartPausePressed;
    }

    private void Update()
    {
        Time.text = DateModule.TimeDisplay();
    }

    void Pressed()
    {
        Phone.style.display = DisplayStyle.None;
    }

    void Pressed2()
    {
        Phone.style.display = DisplayStyle.Flex;
    }

    void ShopButtonPressed()
    {
        
        Debug.Log(ShopApp);
        ShopApp.SetActive(true);
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
        TimeMultiplier.text = DateModule.timeMultiplier.ToString();
    }

    void FastForwardPressed()
    {
        Debug.Log("Fast Forward");
        DateModule.incrementMultiplier();
        TimeMultiplier.text = DateModule.timeMultiplier.ToString();
    }

    void StartPausePressed()
    {
        Debug.Log("Start/Pause");
        DateModule.resetMultiplier();
        TimeMultiplier.text = DateModule.timeMultiplier.ToString();
    }
}
