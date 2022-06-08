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


    private string highscoreFolder; //= "Assets/UI/TextFiles/HighScore.txt";
    private string QuestionFolder;

    public QuestionsList TriviaList = new QuestionsList();
    // Start is called before the first frame update
    void Start()
    {
        highscoreFolder = Application.persistentDataPath + "/HighScore.txt";
        QuestionFolder = Application.persistentDataPath + "/Questions.txt";

        //Read from web and if error is thrown then catch it and display
        //from questions file within our folder
        try
        {
            WebClient wc = new WebClient();
            var json = wc.DownloadString("https://mo-amin.github.io/AFVTriviaQuestions/Questions.txt");

            TriviaList = JsonUtility.FromJson<QuestionsList>(json);

            WriteQuestionTextFile(json.ToString());
        }
        catch
        {
            TriviaList = JsonUtility.FromJson<QuestionsList>(File.ReadAllText(QuestionFolder));
        }
        

        readTextFile();

    }

    //Read text file here only used to read highscore folder
    public void readTextFile()
    {
        StreamReader read = null;
        try
        {
            read = new StreamReader(highscoreFolder);
        }
        catch
        {
            WriteHighScoreTextFile("0");
            return;
        }
        while (!read.EndOfStream)
        {
            highscoreString = read.ReadLine();
            highscore = long.Parse(highscoreString);
        }

        read.Close();

    }

    public void WriteHighScoreTextFile(string information)
    {
        File.WriteAllText(highscoreFolder, information);

    }

    public void WriteQuestionTextFile(string information)
    {
        File.WriteAllText(QuestionFolder, information);

    }
}
