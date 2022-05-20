using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private Button NextQuestionButton;
    private Button SaveResultsButton;

    private long QuestionAmount;
    private long points;

    void OnEnable()
    {
        points = 0;
        QuestionAmount = 50;

        assignUItoVariables();
        root.Focus();
        assignButtonsToFunctions();

        PointsScored.text = "0";
        ValueOfQuestion.text = QuestionAmount.ToString() + " Point Question";
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
    }

    void assignButtonsToFunctions()
    {

        for (int i = 0; i < AnswerLabels.Length; ++i)
            root.Q<Button>(AnswerLabels[i]).clickable.clickedWithEventInfo += AnswerClicked;
        NextQuestionButton.clicked += NextQuestionClicked;
        SaveResultsButton.clicked += SaveResultsClicked;
    }
    void NextQuestionClicked()
    {

        //Hide buttons for next and done
        //Show question point value label
        ValueOfQuestion.style.display = DisplayStyle.Flex;
        NextQuestionContainer.style.display = DisplayStyle.None;
        QuestionGenerated();

    }
    void SaveResultsClicked()
    {

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
            else
            {
                points += QuestionAmount;
                QuestionAmount += 50;
                PointsScored.text = points.ToString();
                ValueOfQuestion.text = QuestionAmount.ToString() + " Point Question";

                //Show buttons for next and done
                //Hide question point value label
                ValueOfQuestion.style.display = DisplayStyle.None;
                NextQuestionContainer.style.display = DisplayStyle.Flex;
            }

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

        TriviaQuestions getTriviaList = QuestionReader.GetComponent<TriviaQuestions>();
        int numOfQuestions = getTriviaList.TriviaList.Questions.Length;
        int r = Random.Range(0, numOfQuestions);
        TriviaQuestions.question RandomQuestionAsked = getTriviaList.TriviaList.Questions[r];

        root.Q<Label>("Question").text = RandomQuestionAsked.Actual_Question;
        root.Q<Label>(AnswerLabels[0] + "Label").text = RandomQuestionAsked.Correct_Answer;
        root.Q<Label>(AnswerLabels[1] + "Label").text = RandomQuestionAsked.Wrong_Answer1;
        root.Q<Label>(AnswerLabels[2] + "Label").text = RandomQuestionAsked.Wrong_Answer2;
        root.Q<Label>(AnswerLabels[3] + "Label").text = RandomQuestionAsked.Wrong_Answer3;
    }

}
