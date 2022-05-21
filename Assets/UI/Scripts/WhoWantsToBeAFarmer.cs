using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;


public class WhoWantsToBeAFarmer : MonoBehaviour
{
    private VisualElement root;
    [SerializeField]
    private GameObject QuestionReader;
    private string[] AnswerLabels = new string[] { "FirstAnswer", "SecondAnswer", "ThirdAnswer", "FourthAnswer" };
    private bool UserAnsweredQuestion;

    private Label PointsScored;

    private Label ValueOfQuestion;
    private VisualElement NextQuestionContainer;
    private VisualElement WrongAnswerContainer;
    private Button NextQuestionButton;
    private Button SaveResultsButton;
    private TriviaQuestions getTriviaList;
    private Label highscoreLabel;

    [SerializeField]
    private GameObject controls;
    [SerializeField]
    private GameObject PhoneGameObject;
    [SerializeField]
    private GameObject canvasControls;

    private long QuestionAmount;
    private long points;
    private Button BackButton;
    private Button RestartButton;

    private VisualElement Body;
    private VisualElement NewHighScoreMessage;

    private long StartingHighScore;

    private bool newhighScoreOccured;



    
    void OnEnable()
    {
        getTriviaList = QuestionReader.GetComponent<TriviaQuestions>();

        assignUItoVariables();
        root.Focus();
        assignButtonsToFunctions();
        initialize();
        StartingHighScore = getTriviaList.highscore;

    }
    void initialize()
    {
        points = 0;
        QuestionAmount = 50;
        newhighScoreOccured = false;
        PointsScored.text = "0";
        ValueOfQuestion.text = QuestionAmount.ToString() + " Point Question";
        highscoreLabel.text = getTriviaList.highscoreString;
        QuestionGenerated();

    }
    void assignUItoVariables()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        PointsScored = root.Q<Label>("PointsScored");
        ValueOfQuestion = root.Q<Label>("ValueOfQuestion");
        NextQuestionContainer = root.Q<VisualElement>("NextQuestionContainer");
        NextQuestionButton = root.Q<Button>("NextQuestionButton");
        SaveResultsButton = root.Q<Button>("SaveResultsButton");
        highscoreLabel = root.Q<Label>("HighScore");
        BackButton = root.Q<Button>("BackButton");
        WrongAnswerContainer = root.Q<VisualElement>("WrongAnswerContainer");
        RestartButton = root.Q<Button>("RestartButton");
        Body = root.Q<VisualElement>("Body");
        NewHighScoreMessage = root.Q<VisualElement>("NewHighScoreMessage");
    }

    void assignButtonsToFunctions()
    {

        for (int i = 0; i < AnswerLabels.Length; ++i)
            root.Q<Button>(AnswerLabels[i]).clickable.clickedWithEventInfo += AnswerClicked;
        NextQuestionButton.clicked += NextQuestionClicked;
        SaveResultsButton.clicked += EndGame;
        BackButton.clicked += EndGame;
        RestartButton.clicked += RestartButtonPressed;
    }
    void RestartButtonPressed()
    {
        ValueOfQuestion.style.display = DisplayStyle.Flex;
        NextQuestionContainer.style.display = DisplayStyle.None;
        WrongAnswerContainer.style.display = DisplayStyle.None;
        initialize();

    }
    void NextQuestionClicked()
    {

        //Hide buttons for next and done
        //Show question point value label
        ValueOfQuestion.style.display = DisplayStyle.Flex;
        NextQuestionContainer.style.display = DisplayStyle.None;
        QuestionGenerated();

    }
    

    IEnumerator showHighscoreMessage()
    {
        if (!newhighScoreOccured)
        {
            MainUI mainUI = new MainUI();
            mainUI.ShowOrHideVisualElements(ref NewHighScoreMessage, ref Body);
            BackButton.style.visibility = Visibility.Hidden;
            yield return new WaitForSeconds(2);
            mainUI.ShowOrHideVisualElements(ref Body, ref NewHighScoreMessage);
            BackButton.style.visibility = Visibility.Visible;

            newhighScoreOccured = true;
        }
    }
    void AnswerClicked(EventBase obj)
    {
        if (!UserAnsweredQuestion)
        {
            UserAnsweredQuestion = true;
            var button = (Button)obj.target;
            root.Q<Button>(AnswerLabels[0]).style.backgroundColor = Color.green;
            if (!button.name.Equals(AnswerLabels[0]))
            {
                button.style.backgroundColor = new Color(1f, .84f, 0f);
                ValueOfQuestion.style.display = DisplayStyle.None;
                NextQuestionContainer.style.display = DisplayStyle.None;
                WrongAnswerContainer.style.display = DisplayStyle.Flex;

            }
            else
            {
                points += QuestionAmount;
                QuestionAmount += 50;
                PointsScored.text = points.ToString();
                ValueOfQuestion.text = QuestionAmount.ToString() + " Point Question";

                if (points > getTriviaList.highscore)
                {
                    StartCoroutine(showHighscoreMessage());
                    highscoreLabel.text = points.ToString();
                    getTriviaList.highscore = points;
                    getTriviaList.highscoreString = points.ToString();
                }


                //Show buttons for next and done
                //Hide question point value label
                ValueOfQuestion.style.display = DisplayStyle.None;
                NextQuestionContainer.style.display = DisplayStyle.Flex;
            }

        }
    }

    void reshuffle(string[] texts)
    {
        // Knuth shuffle algorithm :: From Unity Forum
        for (int i = 0; i < texts.Length; i++)
        {
            string tmp = texts[i];
            int r = Random.Range(i, texts.Length);
            texts[i] = texts[r];
            texts[r] = tmp;
        }
    }

    void resetAnswerBackgroundColor(Color DefaultColor)
    {
        for (int i = 0; i < AnswerLabels.Length; ++i)
        {
            root.Q<Button>(AnswerLabels[i]).style.backgroundColor = DefaultColor;

        }


    }

    void QuestionGenerated()
    {
        resetAnswerBackgroundColor(Color.white);
        UserAnsweredQuestion = false;
        reshuffle(AnswerLabels);
        Debug.Log(AnswerLabels[0]);

        //TriviaQuestions getTriviaList = QuestionReader.GetComponent<TriviaQuestions>();
        int numOfQuestions = getTriviaList.TriviaList.Questions.Length;
        int r = Random.Range(0, numOfQuestions);
        TriviaQuestions.question RandomQuestionAsked = getTriviaList.TriviaList.Questions[r];

        root.Q<Label>("Question").text = RandomQuestionAsked.Actual_Question;
        root.Q<Label>(AnswerLabels[0] + "Label").text = RandomQuestionAsked.Correct_Answer;
        root.Q<Label>(AnswerLabels[1] + "Label").text = RandomQuestionAsked.Wrong_Answer1;
        root.Q<Label>(AnswerLabels[2] + "Label").text = RandomQuestionAsked.Wrong_Answer2;
        root.Q<Label>(AnswerLabels[3] + "Label").text = RandomQuestionAsked.Wrong_Answer3;
    }

    void SaveResults()
    {
        if (points > StartingHighScore)
        {
            getTriviaList.WriteTextFile(points.ToString());
            getTriviaList.highscore = points;
            getTriviaList.highscoreString = points.ToString();
        }
    }
    void EndGame()
    {
        SaveResults();
        canvasControls.SetActive(true);
        controls.SetActive(true);
        PhoneGameObject.SetActive(true);
        this.gameObject.SetActive(false);
        PhoneGameObject.GetComponent<PhoneButtonController>().ShowPhone();
    }

}
