using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaQuestions : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class question
    {
        public string Actual_Question;
        public string Correct_Answer;
        public string Wrong_Answer1;
        public string Wrong_Answer2;
        public string Wrong_Answer3;
    }

    [System.Serializable]
    public class QuestionsList
    {
        public question[] Questions;
    }

    public QuestionsList TriviaList = new QuestionsList();
    // Start is called before the first frame update
    void Start()
    {
        TriviaList = JsonUtility.FromJson<QuestionsList>(textJSON.text);
        Debug.Log(TriviaList.Questions[TriviaList.Questions.Length - 1].Actual_Question);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
