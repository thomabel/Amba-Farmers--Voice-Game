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

    public GameEventListener onDayChange;
    private int day = 1;
    private int month = 4;
    private int year = 2022;
    string dayString = "01";
    string monthString = "04";
    string YearString = "2022";



    [SerializeField]
    private Account player;
    // Start is called before the first frame update
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
        TriviaButton.clicked += TriviaButtonPressed;
        for(int i= 0; i<AnswerLabels.Length;++i)
            root.Q<Button>(AnswerLabels[i]).clickable.clickedWithEventInfo += AnswerClicked;
        /*
        root.Q<Button>("FirstAnswer").clickable.clickedWithEventInfo += AnswerClicked;
        root.Q<Button>("SecondAnswer").clickable.clickedWithEventInfo += AnswerClicked;
        root.Q<Button>("ThirdAnswer").clickable.clickedWithEventInfo += AnswerClicked;
        root.Q<Button>("FourthAnswer").clickable.clickedWithEventInfo += AnswerClicked;
        */

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
        /*
        UserAnsweredQuestion = false;
        reshuffle(AnswerLabels);
        Debug.Log(AnswerLabels[0]);
        TriviaQuestions getTriviaList = QuestionReader.GetComponent<TriviaQuestions>();
        int numOfQuestions = getTriviaList.TriviaList.Questions.Length;
        int r = Random.Range(0, numOfQuestions);
        TriviaQuestions.question RandomQuestionAsked = getTriviaList.TriviaList.Questions[r];

        root.Q<Label>("Question").text = RandomQuestionAsked.Actual_Question;
        root.Q<Label>(AnswerLabels[0] + "Label").text = RandomQuestionAsked.Correct_Answer;
        root.Q<Label>(AnswerLabels[1] + "Label").text = RandomQuestionAsked.Wrong_Answer1;
        root.Q<Label>(AnswerLabels[2] + "Label").text = RandomQuestionAsked.Wrong_Answer2;
        root.Q<Label>(AnswerLabels[3] + "Label").text = RandomQuestionAsked.Wrong_Answer3;
        StyleSetTrivia(DisplayStyle.Flex, DisplayStyle.None);

        root.Q<Label>("AppsLabel").text = "Trivia";
        */
    }
    void AnswerClicked(EventBase obj)
    {
        if (!UserAnsweredQuestion)
        {
            UserAnsweredQuestion = true;
            var button = (Button)obj.target;
            root.Q<Button>(AnswerLabels[0]).style.backgroundColor = Color.green;
            if (!button.name.Equals(AnswerLabels[0]))
                button.style.backgroundColor = new Color(1f, .84f, 0f);
        }

        //changeAnswerBackground(new Color(1f, .84f, 0f), Color.red);
    }
    /*
    void changeAnswerBackground(Color CorrectAnswer, Color WrongAnswer)
    {
        root.Q<Button>(AnswerLabels[0]).style.backgroundColor = CorrectAnswer;
        for (int i = 1; i < AnswerLabels.Length; ++i)
        {
            root.Q<Button>(AnswerLabels[i]).style.backgroundColor = WrongAnswer;

        }


    }
    */
    void resetAnswerBackgroundColor(Color DefaultColor)
    {
        for (int i = 0; i < AnswerLabels.Length; ++i)
        {
            root.Q<Button>(AnswerLabels[i]).style.backgroundColor = DefaultColor;

        }


    }


    void reshuffle(string[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
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
        StyleSetTrivia(DisplayStyle.None, DisplayStyle.Flex);
        resetAnswerBackgroundColor(Color.white);
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
    void StyleSetTrivia(DisplayStyle Trivia, DisplayStyle ScrollContainer)
    {
        root.Q<ScrollView>("RealPhoneScrollView").style.display = ScrollContainer;

        root.Q<VisualElement>("BackButtonContainer").style.display = Trivia;

        root.Q<VisualElement>("TriviaContent").style.display = Trivia;

        //AppScrollView.style.display = ScrollContainer;

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
