using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;

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

    public long highscore = 0;
    public string highscoreString = "";


    private string highscoreFolder = "Assets/UI/TextFiles/HighScore.txt";

    public QuestionsList TriviaList = new QuestionsList();
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            WebClient wc = new WebClient();
            var json = wc.DownloadString("https://mo-amin.github.io/hostJsonPrac/Questions.txt");
            Debug.Log(json);
            TriviaList = JsonUtility.FromJson<QuestionsList>(json);
        }
        catch
        {
            TriviaList = JsonUtility.FromJson<QuestionsList>(textJSON.text);
        }
        
        Debug.Log(TriviaList.Questions[TriviaList.Questions.Length - 1].Actual_Question);

        readTextFile(highscoreFolder);
        //WriteTextFile("128", highscoreFolder);
        readTextFile(highscoreFolder);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void readTextFile(string filePath = "Assets/UI/TextFiles/HighScore.txt")
    {
        StreamReader read = new StreamReader(filePath);
        while (!read.EndOfStream)
        {
            highscoreString = read.ReadLine();
            highscore = long.Parse(highscoreString);
            Debug.Log(highscoreString);
        }

        read.Close();

    }

    public void WriteTextFile(string information, string filePath = "Assets/UI/TextFiles/HighScore.txt")
    {
        File.WriteAllText(filePath, information);
        /*
        StreamWriter writetext = new StreamWriter(filepath);
        writetext.WriteLine(information);
        */
    }
}
